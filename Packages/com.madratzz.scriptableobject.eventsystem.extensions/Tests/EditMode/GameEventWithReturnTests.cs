using NUnit.Framework;
using ProjectCore.Events;
using UnityEngine;

namespace Madratzz.Tests.EventSystemExtensions
{
    public class GameEventWithReturnTests
    {
        [Test]
        public void Raise_WithSubscriber_ReturnsHandlerResult()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventReturnsVector3>();
            gameEvent.Handler += () => Vector3.up;

            var result = gameEvent.Raise();

            Assert.AreEqual(Vector3.up, result);
            Object.DestroyImmediate(gameEvent);
        }

        [Test]
        public void Raise_WithNoSubscribers_ReturnsDefault_InsteadOfThrowing()
        {
            var gameEvent = ScriptableObject.CreateInstance<GameEventReturnsVector3>();

            // Regression: archived base threw NullReferenceException (the subclass
            // used to swallow it with try/catch); now returns default (null).
            Vector3? result = null;
            Assert.DoesNotThrow(() => result = gameEvent.Raise());
            Assert.IsNull(result);
            Object.DestroyImmediate(gameEvent);
        }
    }
}
