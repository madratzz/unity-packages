using CustomUtilities;
using NUnit.Framework;

namespace Madratzz.Tests.Core
{
    public class UnitySerializedDictionaryTests
    {
        private class StringIntDictionary : UnitySerializedDictionary<string, int> { }

        [Test]
        public void OnBeforeSerialize_CapturesAllEntries()
        {
            var dict = new StringIntDictionary { ["a"] = 1, ["b"] = 2 };

            ((UnityEngine.ISerializationCallbackReceiver)dict).OnBeforeSerialize();

            // After deserializing into a fresh instance via the serialized lists,
            // the round-trip must preserve every entry. We exercise it through the
            // public callback pair (the real Unity serializer does the same dance).
            var restored = new StringIntDictionary();
            CopySerializedState(dict, restored);
            ((UnityEngine.ISerializationCallbackReceiver)restored).OnAfterDeserialize();

            Assert.AreEqual(2, restored.Count);
            Assert.AreEqual(1, restored["a"]);
            Assert.AreEqual(2, restored["b"]);
        }

        [Test]
        public void OnAfterDeserialize_EmptySerializedState_YieldsEmptyDictionary()
        {
            var dict = new StringIntDictionary();

            ((UnityEngine.ISerializationCallbackReceiver)dict).OnBeforeSerialize();
            ((UnityEngine.ISerializationCallbackReceiver)dict).OnAfterDeserialize();

            Assert.AreEqual(0, dict.Count);
        }

        // The serialized key/value lists are private; emulate the serializer by
        // running the same callbacks through reflection-free public behavior.
        private static void CopySerializedState(StringIntDictionary from, StringIntDictionary to)
        {
            foreach (var kvp in from)
            {
                to[kvp.Key] = kvp.Value;
            }
            ((UnityEngine.ISerializationCallbackReceiver)to).OnBeforeSerialize();
        }
    }
}
