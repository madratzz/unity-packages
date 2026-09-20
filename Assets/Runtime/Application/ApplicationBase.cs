using System;
using UnityEngine;
using ProjectCore.Events;
using ProjectCore.StateMachine;
using ProjectCore.TimeMachine;
using ProjectCore.Variables;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// MonoBehaviour driver for the application flow. Owns the
    /// <see cref="FiniteStateMachine"/> + <see cref="TimeMachine"/> coroutine
    /// loops and the <see cref="GameEvent"/>/transition wiring. Wire one into
    /// your boot scene, populate the SerializeFields, call <see cref="Boot"/>
    /// from a <c>GameEventRaiserOnEnable</c> (or any startup hook).
    ///
    /// Dependencies (state machine, time machine, events) are wired via
    /// [SerializeField] in the Inspector — no DI container required.
    /// </summary>
    public class ApplicationBase : MonoBehaviour
    {
        [Header("Frame Rate")]
        [SerializeField] private int IOSTargetFrameRate = 60;
        [SerializeField] private int AndroidTargetFrameRate = 60;

        [Header("Time Machine")]
        [SerializeField] private TimeMachine.TimeMachine ApplicationTimeMachine;

        [Header("App Lifecycle Events")]
        [SerializeField] private GameEvent AppPaused;
        [SerializeField] private GameEvent AppResumed;
        [SerializeField] private DBInt AppPausedTime;

        [Header("State Machine")]
        [SerializeField] private FiniteStateMachine ApplicationStateMachine;

        /// <summary>
        /// The FSM this application drives. <see cref="ApplicationFlowController"/>
        /// reads this instead of holding its own duplicate reference, so there is
        /// exactly one place an FSM asset is wired for a given application.
        /// </summary>
        public FiniteStateMachine StateMachine => ApplicationStateMachine;

        private Coroutine _stateMachineRoutine;
        private Coroutine _timeMachineRoutine;
        private bool _appPaused;

        private void OnValidate()
        {
            if (!name.Equals(nameof(ApplicationBase)))
            {
                name = nameof(ApplicationBase);
            }
        }

        private void Awake()
        {
            if (ApplicationStateMachine == null)
                Debug.LogWarning("[ApplicationBase] applicationStateMachine is not assigned — the FSM will not run.", this);
        }

        private void Start()
        {
            Application.targetFrameRate = Application.platform switch
            {
                RuntimePlatform.Android    => AndroidTargetFrameRate,
                RuntimePlatform.IPhonePlayer => IOSTargetFrameRate,
                _                          => Application.targetFrameRate
            };
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (ApplicationTimeMachine != null)
                _timeMachineRoutine = StartCoroutine(ApplicationTimeMachine.Tick());

            if (ApplicationStateMachine != null)
                _stateMachineRoutine = StartCoroutine(ApplicationStateMachine.Tick());
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus) ApplicationResumed();
            else       ApplicationPaused();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused) ApplicationPaused();
            else        ApplicationResumed();
        }

        private void OnApplicationQuit()
        {
            ApplicationPaused();
        }

        private void ApplicationPaused()
        {
            if (_appPaused) return;
            _appPaused = true;

            if (AppPausedTime != null)
                AppPausedTime.SetValue((int)DateTimeOffset.Now.ToUnixTimeSeconds());
            AppPaused?.Invoke();
        }

        private void ApplicationResumed()
        {
            if (!_appPaused) return;
            _appPaused = false;
            AppResumed?.Invoke();
        }
    }
}
