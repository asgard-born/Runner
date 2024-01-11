using Configs;
using UnityEngine;

namespace Root
{
    public class EnterPoint : MonoBehaviour
    {
        [Space, Header("Configs")] [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;
        [SerializeField] private ResourcesConfigs _resourcesConfigs;
        [Space, Header("UI")] [SerializeField] private RectTransform _uiRoot;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Camera _camera;

        private GameRoot _root;

        private void Start()
        {
            var rootCtx = new GameRoot.Ctx
            {
                levelConfigs = _levelConfigs,
                playersConfigs = _playersConfigs,
                poolsConfigs = _poolsConfigs,
                resourcesConfigs = _resourcesConfigs,
                uiRoot = _uiRoot,
                spawnPoint = _spawnPoint,
                camera = _camera
            };

            _root = new GameRoot(rootCtx);
        }

        private void OnDestroy()
        {
            _root.Dispose();
        }
    }
}