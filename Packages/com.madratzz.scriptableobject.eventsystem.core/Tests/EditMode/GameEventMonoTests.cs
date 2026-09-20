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

        [Test]
        public void GameEventListener_UnassignedEvent_EnableLogsErrorInsteadOfThrowing()
        {
            _go = new GameObject();
            _go.SetActive(false);
            _go.AddComponent<GameEventListener>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventListener.*not assigned"));
            Assert.DoesNotThrow(() => _go.SetActive(true));
        }

        [Test]
        public void GameEventListener_UnassignedEvent_DisableDoesNotThrow()
        {
            _go = new GameObject();
            _go.SetActive(false);
            _go.AddComponent<GameEventListener>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventListener.*not assigned"));
            _go.SetActive(true);

            Assert.DoesNotThrow(() => _go.SetActive(false));
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
            _go.SetActive(false);
            _go.AddComponent<GameEventRaiserOnEnable>();

            LogAssert.Expect(LogType.Error, new Regex("GameEventRaiserOnEnable.*not assigned"));
            Assert.DoesNotThrow(() => _go.SetActive(true));
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
                .GetField("GameEvent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(raiser, _gameEvent);

            raiser.InvokeEvent();

            Assert.AreEqual(1, calls);
        }
    }
}
