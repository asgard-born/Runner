using System;

namespace Code.ObjectsPool
{
    [Serializable]
    public class PoolInfo
    {
        public PoolObject prefab;
        public int count;
        public Pool pool;
    }
}