using UnityEngine;

namespace Code.ObjectsPool
{
    public static class PoolsManager
    {
        private static PoolInstance[] pools;
        private static GameObject objectsParent;

        [System.Serializable]
        public struct PoolInstance
        {
            public string name;
            public PoolObject prefab;
            public int count;
            public Pool pool;
        }

        public static void Initialize(PoolInstance[] newPools)
        {
            pools = newPools;
            objectsParent = new GameObject();
            objectsParent.name = "Pool";

            for (var i = 0; i < pools.Length; i++)
            {
                if (pools[i].prefab != null)
                {
                    pools[i].pool = new Pool();
                    pools[i].pool.Initialize(pools[i].count, pools[i].prefab, objectsParent.transform);
                }
            }
        }

        public static GameObject GetObject(string name, Vector3 position, Quaternion rotation)
        {
            GameObject result = null;

            if (pools != null)
            {
                for (var i = 0; i < pools.Length; i++)
                {
                    if (string.Compare(pools[i].name, name) == 0)
                    {
                        result = pools[i].pool.GetObject().gameObject;
                        result.transform.position = position;
                        result.transform.rotation = rotation;
                        result.SetActive(true);

                        return result;
                    }
                }
            }

            return result;
        }
    }
}