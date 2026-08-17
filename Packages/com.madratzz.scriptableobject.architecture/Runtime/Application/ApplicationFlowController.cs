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
        [Header("State Machine")]
        [SerializeField] private FiniteStateMachine applicationStateMachine;

        [Header("Transitions (The Destinations)")]
        [SerializeField] private Transition gameStateTransition;
        [SerializeField] private Transition levelFailTransition;
        [SerializeField] private Transition settingsTransition;

        [Header("Events (The Triggers)")]
        [SerializeField] private GameEvent gotoGame;
        [SerializeField] private GameEvent gotoLevelFail;

        [Header("View Closed Events")]
        [SerializeField] private GameEventWithInt levelFailViewClosed;

        [Tooltip("Replace with a richer IFlowLogic to override the default Boot/LevelFail→GoToGame strategy.")]
        [SerializeField] private bool useCustomLogic;

        private IFlowLogic _logicBrain;
        private FiniteStateMachine _stateMachine;
        private Dictionary<FlowIntent, Action> _commandMap;

        private void Awake()
        {
            _stateMachine = applicationStateMachine;
            if (_stateMachine == null)
            {
                Debug.LogError("[ApplicationFlowController] applicationStateMachine is not assigned.");
                enabled = false;
                return;
            }

            _logicBrain = useCustomLogic
                ? gameObject.GetComponent<IFlowLogic>()
                : new ApplicationFlowLogic();

            if (_logicBrain == null)
            {
                Debug.LogError("[ApplicationFlowController] No IFlowLogic implementation found. Enable useCustomLogic with a component on this GameObject, or leave it off to use the default ApplicationFlowLogic.");
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
                { FlowIntent.GoToGame,        () => PerformTransition(gameStateTransition) },
                { FlowIntent.GoToLevelFail,   () => PerformTransition(levelFailTransition) },
                { FlowIntent.OpenSettings,    () => PerformTransition(settingsTransition) },

                // Logic actions
                { FlowIntent.ResumePrevious,  () => _stateMachine.ShouldResumePreviousState() },

                // Defaults
                { FlowIntent.DefaultToGame,   () => PerformTransition(gameStateTransition) }
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
            if (gotoGame)        gotoGame.Handler       += OnGotoGame;
            if (gotoLevelFail)   gotoLevelFail.Handler  += OnGotoLevelFail;
            if (levelFailViewClosed) levelFailViewClosed.Handler += OnLevelFailViewClose;
        }

        private void UnsubscribeEvents()
        {
            if (gotoGame)        gotoGame.Handler       -= OnGotoGame;
            if (gotoLevelFail)   gotoLevelFail.Handler  -= OnGotoLevelFail;
            if (levelFailViewClosed) levelFailViewClosed.Handler -= OnLevelFailViewClose;
        }
    }
}
