using System;
using System.Collections.Generic;
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
        [SerializeField] private ApplicationBase ApplicationBase;

        [Header("Transitions (The Destinations)")]
        [SerializeField] private Transition GameStateTransition;
        [SerializeField] private Transition LevelFailTransition;
        [SerializeField] private Transition SettingsTransition;

        [Header("Events (The Triggers)")]
        [SerializeField] private GameEvent GotoGame;
        [SerializeField] private GameEvent GotoLevelFail;

        [Header("View Closed Events")]
        [SerializeField] private GameEventWithInt LevelFailViewClosed;

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
        private void OnGotoGame()        => ExecuteIntent(FlowIntent.GoToGame);
        private void OnGotoLevelFail()   => ExecuteIntent(FlowIntent.GoToLevelFail);
        private void OnLevelFailViewClose(int value) =>
            ResolveDecision(FlowContext.LevelFail, (UICloseReasons)value);

        private void SubscribeEvents()
        {
            if (GotoGame)        GotoGame.Handler       += OnGotoGame;
            if (GotoLevelFail)   GotoLevelFail.Handler  += OnGotoLevelFail;
            if (LevelFailViewClosed) LevelFailViewClosed.Handler += OnLevelFailViewClose;
        }

        private void UnsubscribeEvents()
        {
            if (GotoGame)        GotoGame.Handler       -= OnGotoGame;
            if (GotoLevelFail)   GotoLevelFail.Handler  -= OnGotoLevelFail;
            if (LevelFailViewClosed) LevelFailViewClosed.Handler -= OnLevelFailViewClose;
        }
    }
}
