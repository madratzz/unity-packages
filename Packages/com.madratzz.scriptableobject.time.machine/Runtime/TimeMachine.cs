using System.Collections;
using CustomUtilities;
using ProjectCore.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectCore.TimeMachine
{
    /// <summary>
    /// ScriptableObject timer that raises a <see cref="GameEvent"/> every second
    /// while ticking. The loop runs through <see cref="CoroutineHandler"/> (the
    /// persistent coroutine runner from com.madratzz.utilities.coroutines), so the
    /// timer survives scene loads and needs no MonoBehaviour ownership.
    ///
    /// By default the tick uses scaled time (<see cref="WaitForSeconds"/>) — it
    /// pauses when <see cref="Time.timeScale"/> is 0. Set <see cref="UseRealTime"/>
    /// to tick on unscaled wall-clock time instead.
    /// </summary>
    [CreateAssetMenu(fileName = "TimeMachine", menuName = "ProjectCore/TimeMachine")]
    public class TimeMachine : ScriptableObject
    {
        [FormerlySerializedAs("OnTick")]
        [Header("Events Raised")]
        [SerializeField] private GameEvent TickEvent;

        [Header("Timing")]
        [Tooltip("Interval between ticks in seconds.")]
        [SerializeField] private float TickInterval = 1f;

        [Tooltip("When true, ticks on unscaled (real) time — ignores Time.timeScale. When false, scaled time: ticking pauses while timeScale is 0.")]
        [SerializeField] private bool UseRealTime;

        private Coroutine _tickCoroutine;

        /// <summary>Interval between ticks in seconds.</summary>
        public float Interval => TickInterval;

        /// <summary>True while a tick loop is running.</summary>
        public bool IsTicking => _tickCoroutine != null;

        /// <summary>
        /// Starts the per-second tick loop. Safe to call while already ticking —
        /// the existing loop keeps running and no second loop is started.
        /// </summary>
        public void StartTicking()
        {
            if (_tickCoroutine != null)
                return;

            _tickCoroutine = CoroutineHandler.StartStaticCoroutine(TickLoop());
        }

        /// <summary>
        /// Stops the tick loop. Safe to call when not ticking.
        /// </summary>
        public void StopTicking()
        {
            if (_tickCoroutine == null)
                return;

            CoroutineHandler.StopStaticCoroutine(_tickCoroutine);
            _tickCoroutine = null;
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                if (UseRealTime)
                    yield return new WaitForSecondsRealtime(TickInterval);
                else
                    yield return new WaitForSeconds(TickInterval);

                if (TickEvent != null)
                    TickEvent.Invoke();
            }
        }

        // Kept for backward compatibility with callers that drive the coroutine
        // themselves: prefer StartTicking()/StopTicking().
        public IEnumerator Tick()
        {
            return TickLoop();
        }
    }
}
