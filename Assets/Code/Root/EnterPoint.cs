using Configs;
using Shared;
using UnityEngine;

namespace Root
{
    public class EnterPoint : MonoBehaviour
    {
        [Header("Configs")] [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private ResourcesConfigs _resourcesConfigs;
        [SerializeField] private CameraConfigs _cameraConfigs;
        [Space, Header("UI")] [SerializeField] private RectTransform _uiRoot;
        [SerializeField] private OrientationAxises _orientationAxises;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Camera _camera;

        private GameRoot _root;

        private void Start()
        {
            var rootCtx = new GameRoot.Ctx
            {
                playersConfigs = _playersConfigs,
                levelConfigs = _levelConfigs,
                resourcesConfigs = _resourcesConfigs,
                cameraConfigs = _cameraConfigs,
                uiRoot = _uiRoot,
                orientationAxises = _orientationAxises,
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