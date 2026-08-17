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
        [SerializeField] private int iOSTargetFrameRate = 60;
        [SerializeField] private int androidTargetFrameRate = 60;

        [Header("Time Machine")]
        [SerializeField] private TimeMachine.TimeMachine applicationTimeMachine;

        [Header("App Lifecycle Events")]
        [SerializeField] private GameEvent appPaused;
        [SerializeField] private GameEvent appResumed;
        [SerializeField] private DBInt appPausedTime;

        [Header("State Machine")]
        [SerializeField] private FiniteStateMachine applicationStateMachine;

        private FiniteStateMachine _applicationStateMachine;
        private Coroutine _stateMachineRoutine;
        private Coroutine _timeMachineRoutine;
        private bool _appPaused;

        private void Awake()
        {
            // Fall back to a Resources-loaded FSM if the SerializeField wasn't
            // wired in the Inspector — useful for project-wide singletons.
            _applicationStateMachine = applicationStateMachine;
            if (_applicationStateMachine == null)
                _applicationStateMachine = Resources.Load<FiniteStateMachine>("StateMachine");
        }

        private void Start()
        {
            Application.targetFrameRate = Application.platform switch
            {
                RuntimePlatform.Android    => androidTargetFrameRate,
                RuntimePlatform.IPhonePlayer => iOSTargetFrameRate,
                _                          => Application.targetFrameRate
            };
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (applicationTimeMachine != null)
                _timeMachineRoutine = StartCoroutine(applicationTimeMachine.Tick());

            if (_applicationStateMachine != null)
                _stateMachineRoutine = StartCoroutine(_applicationStateMachine.Tick());
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

            if (appPausedTime != null)
                appPausedTime.SetValue((int)DateTimeOffset.Now.ToUnixTimeSeconds());
            appPaused?.Invoke();
        }

        private void ApplicationResumed()
        {
            if (!_appPaused) return;
            _appPaused = false;
            appResumed?.Invoke();
        }
    }
}
