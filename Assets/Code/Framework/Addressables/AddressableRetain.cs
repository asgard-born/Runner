using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.Addressables
{
    public readonly struct AddressableRetain : IDisposable
    {
        private readonly AsyncOperationHandle _handle;

        public AddressableRetain(AsyncOperationHandle handle)
        {
            _handle = handle;
        }

        public void Dispose()
        {
            UnityEngine.AddressableAssets.Addressables.ReleaseInstance(_handle);
        }
    }
}