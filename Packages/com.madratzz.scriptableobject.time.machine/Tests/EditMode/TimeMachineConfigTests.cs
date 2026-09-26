using NUnit.Framework;
using ProjectCore.Events;
using ProjectCore.TimeMachine;
using UnityEngine;

namespace Madratzz.Tests.TimeMachine
{
    public class TimeMachineConfigTests
    {
        private ProjectCore.TimeMachine.TimeMachine _timeMachine;

        [SetUp]
        public void SetUp()
        {
            _timeMachine = ScriptableObject.CreateInstance<ProjectCore.TimeMachine.TimeMachine>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_timeMachine);
        }

        [Test]
        public void Defaults_NotTicking_OnCreate()
        {
            Assert.IsFalse(_timeMachine.IsTicking);
        }

        [Test]
        public void DefaultInterval_IsOneSecond()
        {
            Assert.AreEqual(1f, _timeMachine.Interval);
        }

        [Test]
        public void TickEvent_CanBeAssignedViaSerializedField()
        {
            var tickEvent = ScriptableObject.CreateInstance<GameEvent>();
            var so = new UnityEditor.SerializedObject(_timeMachine);
            so.FindProperty("TickEvent").objectReferenceValue = tickEvent;
            so.ApplyModifiedPropertiesWithoutUndo();

            Assert.AreEqual(tickEvent, so.FindProperty("TickEvent").objectReferenceValue);
            Object.DestroyImmediate(tickEvent);
        }
    }
}
