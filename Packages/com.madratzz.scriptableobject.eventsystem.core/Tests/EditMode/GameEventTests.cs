using NUnit.Framework;
using ProjectCore.Events;
using UnityEngine;

namespace Madratzz.Tests.EventSystemCore
{
    public class GameEventTests
    {
        private GameEvent _gameEvent;

        [SetUp]
        public void SetUp()
        {
            _gameEvent = ScriptableObject.CreateInstance<GameEvent>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_gameEvent);
        }

        [Test]
        public void Invoke_WithSubscriber_CallsHandler()
        {
            int calls = 0;
            _gameEvent.Handler += () => calls++;

            _gameEvent.Invoke();

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Invoke_WithMultipleSubscribers_CallsAll()
        {
            int calls = 0;
            _gameEvent.Handler += () => calls++;
            _gameEvent.Handler += () => calls += 10;

            _gameEvent.Invoke();

            Assert.AreEqual(11, calls);
        }

        [Test]
        public void Invoke_WithNoSubscribers_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _gameEvent.Invoke());
        }

        [Test]
        public void UnsubscribedHandler_IsNoLongerCalled()
        {
            int calls = 0;
            void Listener() => calls++;
            _gameEvent.Handler += Listener;
            _gameEvent.Handler -= Listener;

            _gameEvent.Invoke();

            Assert.AreEqual(0, calls);
        }
    }
}
