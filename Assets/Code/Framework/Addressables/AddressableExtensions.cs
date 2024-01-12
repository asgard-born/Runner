using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Framework.Addressables
{
    public static class AddressableExtensions
    {
        public static async UniTask<(T asset, AddressableRetain retain)> TryLoadAsync<T>(this AssetReference reference)
        {
            if (reference == null)
            {
                Debug.LogError("reference can't be null");

                return default;
            }

            Debug.Log($"AddressableExtensions, load asset by reference: {reference.SubObjectName}");

            return await LoadAssetAsync<T>(reference);
        }

        public static UniTask<(GameObject go, AddressableRetain release)> TryLoadGameObjAsync(this AssetReference reference) => reference.TryLoadAsync<GameObject>();

        private static async UniTask<(T asset, AddressableRetain retain)> LoadAssetAsync<T>(AssetReference reference)
        {
            const int RETRY_INTERVAL_SECONDS = 5;

            while (true)
            {
                try
                {
                    var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(reference);
                    T asset = await handle.Task;

                    if (handle.OperationException != null)
                    {
                        Debug.LogError($"can't instantiate addressable asset {reference} by exception");
                        Debug.LogException(handle.OperationException);
                        UnityEngine.AddressableAssets.Addressables.Release(handle);

                        await UniTask.Delay(TimeSpan.FromSeconds(RETRY_INTERVAL_SECONDS));

                        continue;
                    }

                    if (!handle.IsDone)
                    {
                        Debug.LogError($"can't instantiate addressable asset {reference}, it's undone");
                        UnityEngine.AddressableAssets.Addressables.Release(handle);

                        await UniTask.Delay(TimeSpan.FromSeconds(RETRY_INTERVAL_SECONDS));

                        continue;
                    }

                    if (asset == null)
                    {
                        Debug.LogError($"can't instantiate addressable asset {reference}");
                        UnityEngine.AddressableAssets.Addressables.Release(handle);

                        await UniTask.Delay(TimeSpan.FromSeconds(RETRY_INTERVAL_SECONDS));

                        continue;
                    }

                    return (asset, new AddressableRetain(handle));
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e.Message}");
                    await UniTask.Delay(TimeSpan.FromSeconds(RETRY_INTERVAL_SECONDS));
                }
            }
        }
    }
}