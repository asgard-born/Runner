using Code.ObjectsPool;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Configs/PoolsConfigs", fileName = "PoolsConfigs", order = 0)]
    public class PoolsConfigs : ScriptableObject
    {
        [SerializeField] private PoolsManager.PoolInstance[] pools;

        private void OnValidate()
        {
            for (var i = 0; i < pools.Length; i++)
            {
                pools[i].name = pools[i].prefab.name;
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            PoolsManager.Initialize(pools);
        }
    }
}