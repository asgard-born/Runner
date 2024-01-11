using System;

namespace ObjectsPool
{
    [Serializable]
    public class PoolInfo
    {
        public PoolObject prefab;
        public int count;
        public Pool pool;
    }
}