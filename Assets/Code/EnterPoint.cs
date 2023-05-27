using System.Threading.Tasks;
using Code.CameraLogic;
using Code.Configs;
using Code.Platforms;
using Code.Player;
using UnityEngine;

namespace Code
{
    public class EnterPoint : MonoBehaviour
    {
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private Camera _camera;

        private Transform _playerSpawnPoint;
        private PlayerEntity _player;
        private PlatformsSpawningSystem _platformsSpawningSystem;
        private CameraFollowSystem _cameraSystem;

        private void Awake()
        {
            Initialize();
        }

        private async void Start()
        {
            await Task.Delay((int)(_levelConfigs.runDelaySec * 1000));

            StartLevel();
        }

        private void StartLevel()
        {
            // _player.canRun = true;
        }

        private void Initialize()
        {
            _poolsConfigs.Initialize();

            _platformsSpawningSystem = new PlatformsSpawningSystem(_levelConfigs);
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent).transform;
            _platformsSpawningSystem.StartSpawningCycle(_platformsParent);

            SpawnPlayer();

            var cameraCtx = new CameraFollowSystem.Ctx
            {
                transform = _camera.transform,
                player = _player.transform
            };

            _cameraSystem = new CameraFollowSystem(cameraCtx);
        }

        private void SpawnPlayer()
        {
            _player = Instantiate(_playersConfigs.playerPrefab);
            _player.Init(_playersConfigs);
        }

        private void LateUpdate()
        {
            _cameraSystem.Follow();
        }
    }
}