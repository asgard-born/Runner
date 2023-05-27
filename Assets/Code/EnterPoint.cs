using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.CameraLogic;
using Code.Configs;
using Code.Platforms;
using Code.Platforms.Concrete;
using Code.PlatformsBehaviour;
using Code.PlatformsBehaviour.Abstract;
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
        [SerializeField] private Vector3 _playerSpawnOffset;

        private SessionStats _sessionStats;
        private Transform _playerSpawnPoint;
        private PlayerEntity _player;
        private PlatformsSpawningSystem _platformsSpawningSystem;
        private CameraFollowSystem _cameraSystem;
        private PlatformInteractingBehaviour _platformInteractingBehaviour;

        private Dictionary<Type, PlatformInteractingBehaviour> _interactingSystems;

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
            _player.canRun = true;
        }

        private void Initialize()
        {
            _poolsConfigs.Initialize();
            
            _sessionStats = new SessionStats();
            _platformsSpawningSystem = new PlatformsSpawningSystem(_levelConfigs);
            
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent, OnPlayerInterracted).transform;
            _platformsSpawningSystem.StartSpawningCycle(_platformsParent, OnPlayerInterracted);

            InitializePlayer();
            InitializeCamera();
            InitializeInteractingSystems();
        }

        private void InitializeCamera()
        {
            var cameraCtx = new CameraFollowSystem.Ctx
            {
                cameraTransform = _camera.transform,
                player = _player.transform,
                cameraSmooth = _playersConfigs.cameraSmooth,
            };

            _cameraSystem = new CameraFollowSystem(cameraCtx);
        }

        private void InitializeInteractingSystems()
        {
            var ctx = new PlatformInteractingBehaviour.Ctx
            {
                playerEntity = _player,
                sessionStats = _sessionStats
            };

            _interactingSystems = new Dictionary<Type, PlatformInteractingBehaviour>
            {
                { typeof(StandardPlatform), new StandardInteractingBehaviour(ctx) },
                { typeof(AbyssPlatfrom), new AbyssInteractingBehaviour(ctx) },
                { typeof(AbyssLargePlatform), new AbyssLargeInteractingBehaviour(ctx) },
                { typeof(FencePlatform), new FenceInteractingBehaviour(ctx) },
                { typeof(SawPlatform), new SawInteractingBehaviour(ctx) },
                { typeof(TurnLeftPlatform), new TurnLeftInteractingBehaviour(ctx) },
                { typeof(TurnRightPlatform), new TurnRightInteractingBehaviour(ctx) },
            };
        }

        private void OnPlayerInterracted(Type platformType)
        {
            if (_interactingSystems.TryGetValue(platformType, out PlatformInteractingBehaviour interactSystem))
            {
                interactSystem.InteractWithPlayer();
            }
        }

        private void InitializePlayer()
        {
            _player = Instantiate(_playersConfigs.playerPrefab);
            _player.transform.position = _playerSpawnPoint.position + _playerSpawnOffset;
            _player.Init(_playersConfigs);
        }

        private void LateUpdate()
        {
            _cameraSystem.Follow();
        }
    }
}