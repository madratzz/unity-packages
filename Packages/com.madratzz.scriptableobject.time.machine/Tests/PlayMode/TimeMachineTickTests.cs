using System.Collections;
using NUnit.Framework;
using ProjectCore.Events;
using UnityEngine;
using UnityEngine.TestTools;

namespace Madratzz.Tests.TimeMachine
{
    public class TimeMachineTickTests
    {
        private ProjectCore.TimeMachine.TimeMachine _timeMachine;
        private GameEvent _tickEvent;

        [SetUp]
        public void SetUp()
        {
            _timeMachine = ScriptableObject.CreateInstance<ProjectCore.TimeMachine.TimeMachine>();
            _tickEvent = ScriptableObject.CreateInstance<GameEvent>();

            var so = new UnityEditor.SerializedObject(_timeMachine);
            so.FindProperty("TickEvent").objectReferenceValue = _tickEvent;
            // Shrink the interval so tests don't wait whole seconds.
            so.FindProperty("TickInterval").floatValue = 0.05f;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        [TearDown]
        public void TearDown()
        {
            _timeMachine.StopTicking();
            Object.DestroyImmediate(_timeMachine);
            Object.DestroyImmediate(_tickEvent);
        }

        [UnityTest]
        public IEnumerator StartTicking_RaisesTickEventRepeatedly()
        {
            int ticks = 0;
            _tickEvent.Handler += () => ticks++;

            _timeMachine.StartTicking();

            // ~0.2s at a 0.05s interval — expect at least 2 ticks (timing-safe margin).
            yield return new WaitForSeconds(0.2f);

            Assert.GreaterOrEqual(ticks, 2);
            Assert.IsTrue(_timeMachine.IsTicking);
        }

        [UnityTest]
        public IEnumerator StopTicking_HaltsTicks()
        {
            int ticks = 0;
            _tickEvent.Handler += () => ticks++;

            _timeMachine.StartTicking();
            yield return new WaitForSeconds(0.12f);
            _timeMachine.StopTicking();

            int ticksAtStop = ticks;
            yield return new WaitForSeconds(0.12f);

            Assert.AreEqual(ticksAtStop, ticks);
            Assert.IsFalse(_timeMachine.IsTicking);
        }

        [UnityTest]
        public IEnumerator StartTicking_Twice_DoesNotDoubleTick()
        {
            int ticks = 0;
            _tickEvent.Handler += () => ticks++;

            _timeMachine.StartTicking();
            _timeMachine.StartTicking(); // second call must be a no-op

            yield return new WaitForSeconds(0.12f);

            // One loop at 0.05s: ~2 ticks. A double-started loop would yield ~4+.
            Assert.LessOrEqual(ticks, 3);
        }

        [UnityTest]
        public IEnumerator Tick_WithUnassignedEvent_DoesNotThrow()
        {
            var bare = ScriptableObject.CreateInstance<ProjectCore.TimeMachine.TimeMachine>();
            var so = new UnityEditor.SerializedObject(bare);
            so.FindProperty("TickInterval").floatValue = 0.05f;
            so.ApplyModifiedPropertiesWithoutUndo();

            bare.StartTicking();

            yield return new WaitForSeconds(0.12f);

            Assert.IsTrue(bare.IsTicking); // loop runs without the event asset
            bare.StopTicking();
            Object.DestroyImmediate(bare);
        }
    }
}
