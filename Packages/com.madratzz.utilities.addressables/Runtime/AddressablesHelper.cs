using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectCore.Utilities
{
    /// <summary>
    /// Coroutine-based Addressables helpers. Every method validates the
    /// <see cref="AssetReference"/> before instantiating and surfaces a
    /// structured error log (including the calling object as Unity's log
    /// context, so clicking the log highlights the caller in the Hierarchy).
    /// </summary>
    public static class AddressablesHelper
    {
        /// <summary>
        /// Safe Instantiate: validates, instantiates, extracts a component of
        /// type <typeparamref name="T"/>, and invokes <paramref name="onSuccess"/>
        /// on success. On any failure path the instance is released and
        /// <paramref name="onFailure"/> is invoked with the handle so the
        /// caller can inspect status / exception details.
        /// </summary>
        public static IEnumerator Instantiate<T>(
            AssetReference assetRef,
            Action<T, AsyncOperationHandle<GameObject>> onSuccess,
            Action<AsyncOperationHandle<GameObject>> onFailure = null,
            object debugContext = null) where T : Component
        {
            if (!ValidateAsset(assetRef, debugContext))
            {
                onFailure?.Invoke(default);
                yield break;
            }

            var handle = Addressables.InstantiateAsync(assetRef);
            yield return handle;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                LogError($"Instantiate Failed: {assetRef.AssetGUID}", debugContext);
                onFailure?.Invoke(handle);
                yield break;
            }

            // Type check: if the loaded object isn't a T, release and report.
            if (!handle.Result.TryGetComponent(out T component))
            {
                LogError($"Loaded object '{handle.Result.name}' is missing component: {typeof(T).Name}", debugContext);
                Addressables.ReleaseInstance(handle);
                onFailure?.Invoke(handle);
                yield break;
            }

            try
            {
                onSuccess?.Invoke(component, handle);
            }
            catch (Exception e)
            {
                // Caller's success handler threw — don't leak the instance.
                Addressables.ReleaseInstance(handle);
                Debug.LogException(e, debugContext as UnityEngine.Object);
            }
        }

        /// <summary>
        /// Safe Instantiate returning the <see cref="GameObject"/> directly
        /// (no component extraction). Same validation, release, and failure
        /// semantics as <see cref="Instantiate{T}"/>.
        /// </summary>
        public static IEnumerator InstantiateGameObject(
            AssetReference assetRef,
            Action<GameObject, AsyncOperationHandle<GameObject>> onSuccess,
            Action<AsyncOperationHandle<GameObject>> onFailure = null,
            object debugContext = null)
        {
            if (!ValidateAsset(assetRef, debugContext))
            {
                onFailure?.Invoke(default);
                yield break;
            }

            var handle = Addressables.InstantiateAsync(assetRef);
            yield return handle;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                LogError($"Instantiate Failed: {assetRef.AssetGUID}", debugContext);
                onFailure?.Invoke(handle);
                yield break;
            }

            try
            {
                onSuccess?.Invoke(handle.Result, handle);
            }
            catch (Exception e)
            {
                Addressables.ReleaseInstance(handle);
                Debug.LogException(e, debugContext as UnityEngine.Object);
            }
        }

        private static bool ValidateAsset(AssetReference assetRef, object debugContext)
        {
            if (assetRef != null && assetRef.RuntimeKeyIsValid()) return true;

            LogError("Invalid/Null AssetReference. Did you forget to assign it in the Inspector?", debugContext);
            return false;
        }

        private static void LogError(string message, object context)
        {
            string sender = context != null ? context.GetType().Name : "Unknown Caller";
            // By passing 'context' as the second argument, Unity highlights the
            // object in the Hierarchy when the user clicks the log.
            Debug.LogError($"[AddressablesHelper] [{sender}] {message}", context as UnityEngine.Object);
        }
    }
}
