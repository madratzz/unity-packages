using System.Collections;
using NUnit.Framework;
using ProjectCore.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Madratzz.Tests.Addressables
{
    /// <summary>
    /// EditMode tests for the validation/rejection paths. The actual instantiate
    /// path is covered by integration tests in projects that ship Addressables
    /// entries — exercising it here would require a real AddressableAssetSettings
    /// and entries to instantiate, which is a project-config concern rather than
    /// a unit-test concern.
    /// </summary>
    public class AddressablesHelperValidationTests
    {
        [Test]
        public void Instantiate_NullAssetReference_InvokesOnFailureImmediately()
        {
            // The validation path logs an error — we expect it here so the
            // test runner doesn't flag the expected log as a failure.
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("Invalid/Null AssetReference"));

            var failureHandle = default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject>);
            var failed = false;

            var it = AddressablesHelper.Instantiate<Transform>(
                assetRef: null,
                onSuccess: (c, h) => Assert.Fail("onSuccess should not fire for null AssetReference"),
                onFailure: h => { failed = true; failureHandle = h; },
                debugContext: this);

            // Drive the coroutine one step (it should not yield — the validation
            // rejection happens before any yield). MoveNext() returns false on a
            // synchronous-complete enumerator.
            while (it.MoveNext()) { }

            Assert.IsTrue(failed, "onFailure must fire");
            Assert.IsFalse(failureHandle.IsValid(), "Failure handle must be invalid");
        }

        [Test]
        public void Instantiate_EmptyAssetReference_InvokesOnFailureImmediately()
        {
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("Invalid/Null AssetReference"));

            // An AssetReference created with an empty GUID has RuntimeKeyIsValid() == false.
            var empty = new AssetReferenceGameObject(string.Empty);
            var failureHandle = default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject>);
            var failed = false;

            var it = AddressablesHelper.Instantiate<Transform>(
                assetRef: empty,
                onSuccess: (c, h) => Assert.Fail("onSuccess should not fire for invalid AssetReference"),
                onFailure: h => { failed = true; failureHandle = h; },
                debugContext: this);

            while (it.MoveNext()) { }

            Assert.IsTrue(failed);
            Assert.IsFalse(failureHandle.IsValid());
        }

        [Test]
        public void InstantiateGameObject_NullAssetReference_InvokesOnFailureImmediately()
        {
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("Invalid/Null AssetReference"));

            var failureHandle = default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject>);
            var failed = false;

            var it = AddressablesHelper.InstantiateGameObject(
                assetRef: null,
                onSuccess: (go, h) => Assert.Fail("onSuccess should not fire"),
                onFailure: h => { failed = true; failureHandle = h; },
                debugContext: this);

            while (it.MoveNext()) { }

            Assert.IsTrue(failed);
            Assert.IsFalse(failureHandle.IsValid());
        }

        [Test]
        public void Instantiate_OnFailureIsOptional_DoesNotThrowWhenNull()
        {
            // Null callback must be tolerated — callers that don't care about
            // failure don't have to supply a stub.
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("Invalid/Null AssetReference"));

            var empty = new AssetReferenceGameObject(string.Empty);

            Assert.DoesNotThrow(() =>
            {
                var it = AddressablesHelper.Instantiate<Transform>(
                    assetRef: empty,
                    onSuccess: null,
                    onFailure: null,
                    debugContext: this);
                while (it.MoveNext()) { }
            });
        }
    }
}
