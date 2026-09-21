using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ProjectCore.Events;
using UnityEngine;
using UnityEngine.TestTools;

namespace Madratzz.Tests.EventSystemCore
{
    public class GameEventMonoTests
    {
        private GameObject _go;
        private GameEvent _gameEvent;

        [TearDown]
        public void TearDown()
        {
            if (_go != null)
                Object.DestroyImmediate(_go);
            if (_gameEvent != null)
                Object.DestroyImmediate(_gameEvent);
        }

        // EditMode tests never receive Unity's lifecycle callbacks: OnEnable does not fire for a
        // component added to an active GameObject, nor when one is activated, outside play mode.
        // (Adding [ExecuteAlways] to these components is not an option — it would raise their
        // GameEvents inside the Editor.) Invoke the handler directly instead, which still
        // exercises the guard logic the tests are about, deterministically.
        private static void InvokeLifecycle(MonoBehaviour behaviour, string methodName)
        {
            MethodInfo method = behaviour.GetType().GetMethod(
                methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(method, $"{behaviour.GetType().Name} has no {methodName} to invoke");
            method.Invoke(behaviour, null);
        }

        [Test]
        public void GameEventListener_UnassignedEvent_EnableLogsErrorInsteadOfThrowing()
        {
            _go = new GameObject();
            GameEventListener listener = _go.AddComponent<GameEventListener>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventListener.*not assigned"));
            Assert.DoesNotThrow(() => InvokeLifecycle(listener, "OnEnable"));
        }

        [Test]
        public void GameEventListener_UnassignedEvent_DisableDoesNotThrow()
        {
            _go = new GameObject();
            GameEventListener listener = _go.AddComponent<GameEventListener>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventListener.*not assigned"));
            InvokeLifecycle(listener, "OnEnable");

            Assert.DoesNotThrow(() => InvokeLifecycle(listener, "OnDisable"));
        }

        [Test]
        public void GameEventRaiser_UnassignedEvent_InvokeLogsErrorInsteadOfThrowing()
        {
            _go = new GameObject();
            var raiser = _go.AddComponent<GameEventRaiser>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventRaiser.*not assigned"));
            Assert.DoesNotThrow(() => raiser.InvokeEvent());
        }

        [Test]
        public void GameEventRaiserOnEnable_UnassignedEvent_EnableLogsErrorInsteadOfThrowing()
        {
            _go = new GameObject();
            GameEventRaiserOnEnable raiser = _go.AddComponent<GameEventRaiserOnEnable>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventRaiserOnEnable.*not assigned"));
            Assert.DoesNotThrow(() => InvokeLifecycle(raiser, "OnEnable"));
        }

        [Test]
        public void GameEventRaiser_AssignedEvent_InvokeRaisesHandler()
        {
            _gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            int calls = 0;
            _gameEvent.Handler += () => calls++;

            _go = new GameObject();
            var raiser = _go.AddComponent<GameEventRaiser>();
            typeof(GameEventRaiser)
                .GetField("GameEvent", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(raiser, _gameEvent);

            raiser.InvokeEvent();

            Assert.AreEqual(1, calls);
        }
    }
}