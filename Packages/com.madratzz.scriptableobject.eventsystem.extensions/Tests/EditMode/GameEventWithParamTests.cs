using NUnit.Framework;
using ProjectCore.Events;
using UnityEngine;

namespace Madratzz.Tests.EventSystemExtensions
{
    public class GameEventWithParamTests
    {
        [Test]
        public void GameEventWithInt_Invoke_PassesValueToSubscribers()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventWithInt>();
            int received = 0;
            gameEvent.Handler += v => received = v;

            gameEvent.Invoke(42);

            Assert.AreEqual(42, received);
            Object.DestroyImmediate(gameEvent);
        }

        [Test]
        public void GameEventWithString_Invoke_PassesValue()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventWithString>();
            string received = null;
            gameEvent.Handler += v => received = v;

            gameEvent.Invoke("hello");

            Assert.AreEqual("hello", received);
            Object.DestroyImmediate(gameEvent);
        }

        [Test]
        public void GameEventWithBool_Invoke_PassesValue()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventWithBool>();
            bool received = false;
            gameEvent.Handler += v => received = v;

            gameEvent.Invoke(true);

            Assert.IsTrue(received);
            Object.DestroyImmediate(gameEvent);
        }

        [Test]
        public void GameEventWithFloat_Invoke_PassesValue()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventWithFloat>();
            float received = 0f;
            gameEvent.Handler += v => received = v;

            gameEvent.Invoke(1.5f);

            Assert.AreEqual(1.5f, received, 1e-6f);
            Object.DestroyImmediate(gameEvent);
        }

        [Test]
        public void GameEventWithIntStringBool_Invoke_PassesAllThreeValues()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventWithIntStringBool>();
            int ri = 0; string rs = null; bool rb = false;
            gameEvent.Handler += (i, s, b) => { ri = i; rs = s; rb = b; };

            gameEvent.Invoke(7, "seven", true);

            Assert.AreEqual(7, ri);
            Assert.AreEqual("seven", rs);
            Assert.IsTrue(rb);
            Object.DestroyImmediate(gameEvent);
        }

        [Test]
        public void Invoke_WithNoSubscribers_DoesNotThrow()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventWithInt>();

            Assert.DoesNotThrow(() => gameEvent.Invoke(1));
            Object.DestroyImmediate(gameEvent);
        }
    }
}
