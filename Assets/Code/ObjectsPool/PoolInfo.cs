using System;
using Code.Platforms.Abstract;

namespace Code.ObjectsPool
{
    [Serializable]
    public class PoolInfo
    {
        public Platform prefab;
        public int count;
        public Pool pool;
    }
}