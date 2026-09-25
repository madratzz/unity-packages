using System;
using System.Collections.Generic;
using CustomUtilities.Attributes;
using UnityEngine;
using ProjectCore.Events;
using ProjectCore.StateMachine;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Translates <see cref="FlowIntent"/> decisions from <see cref="IFlowLogic"/>
    /// into concrete <see cref="Transition"/> fires on the FSM, and routes
    /// <see cref="GameEvent"/> close callbacks back through the decision logic.
    ///
    /// All dependencies are wired via [SerializeField] — no DI container
    /// required. The default <see cref="IFlowLogic"/> is
    /// <see cref="ApplicationFlowLogic"/>; replace it for richer flows.
    /// </summary>
    public class ApplicationFlowController : MonoBehaviour
    {
        [Header("Application")]
        [Tooltip("The ApplicationBase that owns the FSM this controller fires transitions on. There is no separate state-machine field here on purpose — both components must always target the same FSM instance.")]
        [RequireReference]
        [SerializeField] private ApplicationBase ApplicationBase;

        [Header("Transitions (The Destinations)")]
        [SerializeField] private Transition GameStateTransition;
        [SerializeField] private Transition LevelFailTransition;
        [SerializeField] private Transition SettingsTransition;
        [SerializeField] private Transition MainMenuTransition;
        [SerializeField] private Transition StoreTransition;

        [Header("Events (The Triggers)")]
        [SerializeField] private GameEvent GotoGame;
        [SerializeField] private GameEvent GotoLevelFail;
        [SerializeField] private GameEvent GotoMainMenu;
        [SerializeField] private GameEvent GotoStore;

        [Tooltip("Each carries a UICloseReasons value as its int payload — the view raises it with the reason it was dismissed, and the decision table turns (context, reason) into an intent.")]
        [Header("View Closed Events")]
        [SerializeField] private GameEventWithInt LevelFailViewClosed;
        [SerializeField] private GameEventWithInt MainMenuViewClosed;
        [SerializeField] private GameEventWithInt SettingsViewClosed;
        [SerializeField] private GameEventWithInt StoreViewClosed;
        [SerializeField] private GameEventWithInt GameplayViewClosed;

        [Tooltip("Replace with a richer IFlowLogic to override the default Boot/LevelFail→GoToGame strategy.")]
        [SerializeField] private bool UseCustomLogic;

        private IFlowLogic _logicBrain;
        private FiniteStateMachine _stateMachine;
        private Dictionary<FlowIntent, Action> _commandMap;

        private void OnValidate()
        {
            if (!name.Equals(nameof(ApplicationFlowController)))
            {
                name = nameof(ApplicationFlowController);
            }
        }
        
        private void Awake()
        {
            _stateMachine = ApplicationBase != null ? ApplicationBase.StateMachine : null;
            if (_stateMachine == null)
            {
                Debug.LogError("[ApplicationFlowController] No FSM available — either ApplicationBase is not assigned, or its own ApplicationStateMachine is not assigned.", this);
                enabled = false;
                return;
            }

            _logicBrain = UseCustomLogic
                ? gameObject.GetComponent<IFlowLogic>()
                : new ApplicationFlowLogic();

            if (_logicBrain == null)
            {
                Debug.LogError("[ApplicationFlowController] No IFlowLogic implementation found. Enable UseCustomLogic with a component on this GameObject, or leave it off to use the default ApplicationFlowLogic.");
                enabled = false;
                return;
            }

            InitializeCommands();
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        /// <summary>
        /// Kicks the flow off. Call once from a startup hook (e.g. an
        /// <c>GameEventRaiserOnEnable</c> or your boot scene's first frame).
        /// </summary>
        public void Boot()
        {
            Debug.Log("[Flow] Booting Application...");
            ResolveDecision(FlowContext.Boot, UICloseReasons.Game);
        }

        private void InitializeCommands()
        {
            _commandMap = new Dictionary<FlowIntent, Action>
            {
                // Navigation
                { FlowIntent.GoToGame,        () => PerformTransition(GameStateTransition) },
                { FlowIntent.GoToLevelFail,   () => PerformTransition(LevelFailTransition) },
                { FlowIntent.OpenSettings,    () => PerformTransition(SettingsTransition) },
                { FlowIntent.GoToMainMenu,    () => PerformTransition(MainMenuTransition) },
                { FlowIntent.OpenStore,       () => PerformTransition(StoreTransition) },

                // Logic actions
                { FlowIntent.ResumePrevious,  () => _stateMachine.ShouldResumePreviousState() },

                // Defaults
                { FlowIntent.DefaultToGame,   () => PerformTransition(GameStateTransition) }
            };
        }

        private void ResolveDecision(FlowContext context, UICloseReasons reason)
        {
            FlowIntent intent = _logicBrain.GetDecision(context, reason);
            ExecuteIntent(intent);
        }

        private void ExecuteIntent(FlowIntent intent)
        {
            if (_commandMap.TryGetValue(intent, out Action command))
            {
                command.Invoke();
            }
            else
            {
                Debug.LogError($"[Flow] Missing binding for Intent: {intent}. Falling back to DefaultToGame.");
                _commandMap[FlowIntent.DefaultToGame]?.Invoke();
            }
        }

        private void PerformTransition(Transition transition)
        {
            if (transition == null)
            {
                Debug.LogWarning("[Flow] Null transition requested.");
                return;
            }

            _stateMachine.Transition(transition);
        }

        // Event handlers — wire to [SerializeField] GameEvents in the Inspector.
        // The Goto* events are direct commands; the *ViewClosed events carry a
        // UICloseReasons payload and go through the decision table instead.
        private void OnGotoGame()        => ExecuteIntent(FlowIntent.GoToGame);
        private void OnGotoLevelFail()   => ExecuteIntent(FlowIntent.GoToLevelFail);
        private void OnGotoMainMenu()    => ExecuteIntent(FlowIntent.GoToMainMenu);
        private void OnGotoStore()       => ExecuteIntent(FlowIntent.OpenStore);

        private void OnLevelFailViewClose(int value) =>
            ResolveDecision(FlowContext.LevelFail, (UICloseReasons)value);
        private void OnMainMenuViewClose(int value) =>
            ResolveDecision(FlowContext.MainMenu, (UICloseReasons)value);
        private void OnSettingsViewClose(int value) =>
            ResolveDecision(FlowContext.Settings, (UICloseReasons)value);
        private void OnStoreViewClose(int value) =>
            ResolveDecision(FlowContext.Store, (UICloseReasons)value);
        private void OnGameplayViewClose(int value) =>
            ResolveDecision(FlowContext.Gameplay, (UICloseReasons)value);

        private void SubscribeEvents()
        {
            if (GotoGame)        GotoGame.Handler       += OnGotoGame;
            if (GotoLevelFail)   GotoLevelFail.Handler  += OnGotoLevelFail;
            if (GotoMainMenu)    GotoMainMenu.Handler   += OnGotoMainMenu;
            if (GotoStore)       GotoStore.Handler      += OnGotoStore;

            if (LevelFailViewClosed) LevelFailViewClosed.Handler += OnLevelFailViewClose;
            if (MainMenuViewClosed)  MainMenuViewClosed.Handler  += OnMainMenuViewClose;
            if (SettingsViewClosed)  SettingsViewClosed.Handler  += OnSettingsViewClose;
            if (StoreViewClosed)     StoreViewClosed.Handler     += OnStoreViewClose;
            if (GameplayViewClosed)  GameplayViewClosed.Handler  += OnGameplayViewClose;
        }

        private void UnsubscribeEvents()
        {
            if (GotoGame)        GotoGame.Handler       -= OnGotoGame;
            if (GotoLevelFail)   GotoLevelFail.Handler  -= OnGotoLevelFail;
            if (GotoMainMenu)    GotoMainMenu.Handler   -= OnGotoMainMenu;
            if (GotoStore)       GotoStore.Handler      -= OnGotoStore;

            if (LevelFailViewClosed) LevelFailViewClosed.Handler -= OnLevelFailViewClose;
            if (MainMenuViewClosed)  MainMenuViewClosed.Handler  -= OnMainMenuViewClose;
            if (SettingsViewClosed)  SettingsViewClosed.Handler  -= OnSettingsViewClose;
            if (StoreViewClosed)     StoreViewClosed.Handler     -= OnStoreViewClose;
            if (GameplayViewClosed)  GameplayViewClosed.Handler  -= OnGameplayViewClose;
        }
    }
}
