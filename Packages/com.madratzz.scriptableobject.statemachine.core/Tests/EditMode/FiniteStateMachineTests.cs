using System.Collections;
using NUnit.Framework;
using ProjectCore.StateMachine;
using UnityEngine;

namespace Madratzz.Tests.StateMachine
{
    public class FiniteStateMachineLifecycleTests
    {
        private FiniteStateMachine _fsm;
        private TestState _stateA;
        private TestState _stateB;
        private TestTransition _transition;

        [SetUp]
        public void SetUp()
        {
            _fsm = ScriptableObject.CreateInstance<FiniteStateMachine>();
            _stateA = ScriptableObject.CreateInstance<TestState>();
            _stateB = ScriptableObject.CreateInstance<TestState>();
            _transition = ScriptableObject.CreateInstance<TestTransition>();
            _transition.ToState = _stateB;

            var so = new UnityEditor.SerializedObject(_fsm);
            so.FindProperty("BootState").objectReferenceValue = _stateA;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_fsm);
            Object.DestroyImmediate(_stateA);
            Object.DestroyImmediate(_stateB);
            Object.DestroyImmediate(_transition);
        }

        [Test]
        public void Tick_RunsBootStateInitAndExecuteOnce()
        {
            var it = _fsm.Tick();
            Advance(it, 2); // Init, Execute

            Assert.AreEqual(_stateA, _fsm.RunningState);
            Assert.AreEqual(1, _stateA.InitCount);
            Assert.AreEqual(1, _stateA.ExecuteCount);
        }

        [Test]
        public void Tick_RunsStateTickEachFrame_UntilTransitionFires()
        {
            var it = _fsm.Tick();
            Advance(it, 3); // Init, Execute, Tick
            Advance(it, 2); // Tick, yield

            Assert.AreEqual(2, _stateA.TickCount);
        }

        [Test]
        public void Transition_ExitsCurrentState_RunsNewStateInitAndExecute()
        {
            var it = _fsm.Tick();
            Advance(it, 2); // Init/Execute on boot

            _fsm.Transition(_transition);
            Advance(it, 6); // Transition: Pause/Exit, Transition.Execute, Init, Execute, currentState.Tick, yield

            Assert.AreEqual(_stateB, _fsm.RunningState);
            Assert.AreEqual(1, _stateA.ExitCount);
            Assert.AreEqual(1, _stateB.InitCount);
            Assert.AreEqual(1, _stateB.ExecuteCount);
            Assert.AreEqual(1, _transition.ExecuteCount);
        }

        [Test]
        public void Transition_WithNullTransition_DoesNotChangeState()
        {
            var it = _fsm.Tick();
            Advance(it, 2); // boot path

            _fsm.Transition(null);
            Advance(it, 2); // tick + yield

            Assert.AreEqual(_stateA, _fsm.RunningState);
        }

        [Test]
        public void Transition_WithNullToState_DoesNotChangeState()
        {
            var t = ScriptableObject.CreateInstance<TestTransition>();
            t.ToState = null;
            try
            {
                var it = _fsm.Tick();
                Advance(it, 2);

                _fsm.Transition(t);
                Advance(it, 2);

                Assert.AreEqual(_stateA, _fsm.RunningState);
            }
            finally
            {
                Object.DestroyImmediate(t);
            }
        }

        [Test]
        public void Transition_CalledTwice_OnlyFirstTakesEffect()
        {
            var t2 = ScriptableObject.CreateInstance<TestTransition>();
            t2.ToState = _stateA; // valid, but queued behind _transition
            try
            {
                var it = _fsm.Tick();
                Advance(it, 2);

                _fsm.Transition(_transition);
                _fsm.Transition(t2);
                Advance(it, 6);

                Assert.AreEqual(_stateB, _fsm.RunningState);
            }
            finally
            {
                Object.DestroyImmediate(t2);
            }
        }

        [Test]
        public void PauseState_TransitionsToPausingState_PushesPreviousToStack()
        {
            _stateB.PausesPreviousState = true;
            var it = _fsm.Tick();
            Advance(it, 2);

            _fsm.Transition(_transition);
            Advance(it, 6);

            Assert.IsTrue(_fsm.IsStatePaused(_stateA));
            Assert.IsTrue(_stateA.PauseCount >= 1);
        }

        [Test]
        public void ShouldResumePreviousState_ExitsCurrentAndResumesPrevious()
        {
            _stateB.PausesPreviousState = true;
            var it = _fsm.Tick();
            Advance(it, 2);

            _fsm.Transition(_transition);
            Advance(it, 6);
            Assert.AreEqual(_stateB, _fsm.RunningState);

            _fsm.ShouldResumePreviousState();
            Advance(it, 3); // Exit, SetState, Resume

            Assert.AreEqual(_stateA, _fsm.RunningState);
            Assert.IsFalse(_fsm.IsStatePaused(_stateA));
        }

        [Test]
        public void ShouldResumePreviousState_WithEmptyStack_IsNoOp()
        {
            var it = _fsm.Tick();
            Advance(it, 2);

            _fsm.ShouldResumePreviousState();
            Advance(it, 3);

            Assert.AreEqual(_stateA, _fsm.RunningState);
        }

        [Test]
        public void CleanupAllPausedStates_OnlyRunsForCurrentOwner()
        {
            _stateB.PausesPreviousState = true;
            var it = _fsm.Tick();
            Advance(it, 2);
            _fsm.Transition(_transition);
            Advance(it, 6);

            // Caller passes a state that's not current — cleanup must not pop.
            ((IState)_fsm).CleanupAllPausedStates(_stateA).MoveNext();

            Assert.IsTrue(_fsm.IsStatePaused(_stateA));
        }

        [Test]
        public void Tick_WithUnassignedBootState_ExitsImmediately()
        {
            var empty = ScriptableObject.CreateInstance<FiniteStateMachine>();
            try
            {
                var it = empty.Tick();
                Assert.IsFalse(it.MoveNext());
            }
            finally
            {
                Object.DestroyImmediate(empty);
            }
        }

        // Drives the FSM coroutine forward N steps. Stops on the first
        // exception so a real failure surfaces rather than spinning.
        private static void Advance(IEnumerator it, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                if (!it.MoveNext())
                    return;
            }
        }

        [Test]
        public void ResetRuntimeState_ClearsCurrentState_SoTheMachineCanBootAgain()
        {
            // Drive the boot block once so CurrentState is populated.
            var tick = _fsm.Tick();
            tick.MoveNext();
            Assert.AreEqual(_stateA, _fsm.RunningState, "precondition: the FSM should have booted into stateA");

            _fsm.ResetRuntimeState();

            Assert.IsNull(_fsm.RunningState);
        }

        [Test]
        public void OnDisable_ResetsRuntimeState()
        {
            // A ScriptableObject's non-serialized fields survive exiting play mode,
            // so a leftover CurrentState would make the next Tick skip the boot
            // block and never enter (or Execute) the boot state again. Unity calls
            // OnDisable on play-mode exit; invoked here by reflection because
            // EditMode tests get no lifecycle callbacks.
            var tick = _fsm.Tick();
            tick.MoveNext();
            Assert.AreEqual(_stateA, _fsm.RunningState, "precondition: the FSM should have booted into stateA");

            typeof(FiniteStateMachine)
                .GetMethod("OnDisable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_fsm, null);

            Assert.IsNull(_fsm.RunningState);
        }

        [Test]
        public void AfterReset_TickRunsTheBootStateLifecycleAgain()
        {
            var first = _fsm.Tick();
            Advance(first, 2); // Init, Execute
            Assert.AreEqual(1, _stateA.InitCount);
            Assert.AreEqual(1, _stateA.ExecuteCount);

            _fsm.ResetRuntimeState();

            var second = _fsm.Tick();
            Advance(second, 2); // Init, Execute again

            Assert.AreEqual(2, _stateA.InitCount, "boot state should be re-initialised after a reset");
            Assert.AreEqual(2, _stateA.ExecuteCount, "boot state should re-execute after a reset");
        }
    }

    public class TestState : State
    {
        public int InitCount, ExecuteCount, TickCount, ExitCount, PauseCount, ResumeCount;

        public override IEnumerator Init(IState listener)
        {
            InitCount++;
            return base.Init(listener);
        }

        public override IEnumerator Execute()
        {
            ExecuteCount++;
            return base.Execute();
        }

        public override IEnumerator Tick()
        {
            TickCount++;
            return base.Tick();
        }

        public override IEnumerator Exit()
        {
            ExitCount++;
            return base.Exit();
        }

        public override IEnumerator Pause()
        {
            PauseCount++;
            return base.Pause();
        }

        public override IEnumerator Resume()
        {
            ResumeCount++;
            return base.Resume();
        }
    }

    public class TestTransition : Transition
    {
        public int ExecuteCount;

        public override IEnumerator Execute()
        {
            ExecuteCount++;
            return base.Execute();
        }
    }
}
