using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Logging;

namespace Framework.Async
{
    public class AsyncOperations
    {
        public class CreateOperation
        {
            public object instance;
            public object metadata;
            public bool isCreated;
            public bool isCreateCompleted;
            public bool isDeleted;
        }
        
        private readonly Dictionary<object, CreateOperation> _createOperations;
        
        private const int TIMEOUT_SECONDS = 600;

        public AsyncOperations()
        {
            _createOperations = new Dictionary<object, CreateOperation>();
        }

        public async Task<T> Create<T>(Container<T> toContainer, Action<CreateOperation> onCreateCallback,
            int timeoutSec = TIMEOUT_SECONDS) where T: class
        {
            Debug.Log($"AsyncOperations, <color=green>enter</color> to Create for type: {typeof(T)}");
            CreateOperation createOperation;
            if (_createOperations.ContainsKey(toContainer))
            {
                Debug.Log($"AsyncOperations, key is exists for type: {typeof(T)}");
                createOperation = _createOperations[toContainer];
                if (createOperation.isCreated)
                {
                    Debug.Log($"AsyncOperations, <color=orange>exit</color>, already created for type: {typeof(T)}");
                    return createOperation.instance as T;
                }
            }
            else
            {
                createOperation = new CreateOperation
                {
                    instance = null,
                    metadata = null,
                    isCreated = false,
                    isDeleted = false,
                };
                _createOperations.Add(toContainer, createOperation);
                onCreateCallback.Invoke(createOperation);
                
                Debug.Log($"AsyncOperations, create operation add for type: {typeof(T)}");
            }
            
            System.Diagnostics.StackTrace trace = null;
            #if DEBUG_ENABLE_LOG
            trace = new System.Diagnostics.StackTrace();
            #endif
            await AsyncExtensions.WaitForCondition(() => createOperation.isCreated, timeoutSec, trace);

            if (createOperation.instance == null)
            {
                Debug.LogError($"AsyncOperations, createOperation.instance is null for type: {typeof(T)}");
            }
            if (createOperation.metadata == null)
            {
                Debug.LogError($"AsyncOperations, createOperation.metadata is null for type: {typeof(T)}");
            }

            createOperation.isCreateCompleted = true;
            
            Debug.Log($"AsyncOperations, <color=orange>exit</color> from Create for type: {typeof(T)}");

            return createOperation.instance as T;
        }

        public async Task Delete<T>(Container<T> fromContainer, Action<CreateOperation> doDestroy) where T: class
        {
            Debug.Log($"AsyncOperations, <color=green>enter</color> to Delete for type: {typeof(T)}");
            CreateOperation createOperation;
            if (_createOperations.ContainsKey(fromContainer))
            {
                Debug.Log($"AsyncOperations, key exists for type: {typeof(T)}");
                createOperation = _createOperations[fromContainer];
                if (createOperation.isCreateCompleted)
                {
                    Debug.Log($"AsyncOperations, key exists, instance created for type: {typeof(T)}");
                }
                else
                {
                    await AsyncExtensions.WaitForCondition(() => createOperation.isCreateCompleted, TIMEOUT_SECONDS);
                }

                if (createOperation.isDeleted)
                {
                    Debug.Log($"AsyncOperations, operation isDeleted==true for type: {typeof(T)}, exit");
                    return;
                }

                Debug.Log($"AsyncOperations, operation removed for type: {typeof(T)}");
                _createOperations.Remove(fromContainer);

                doDestroy.Invoke(createOperation);
            }
            Debug.Log($"AsyncOperations, <color=orange>exit</color> from Delete for type: {typeof(T)}");
        }
    }
}