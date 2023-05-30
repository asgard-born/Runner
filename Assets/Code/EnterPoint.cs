using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.CameraLogic;
using Code.Configs;
using Code.Platforms;
using Code.Platforms.Essences;
using Code.PlatformsBehaviour;
using Code.PlatformsBehaviour.Abstract;
using Code.Player;
using Code.PlayersInput;
using Code.Session;
using Code.UI;
using Code.UI.Presenters;
using UnityEngine;

namespace Code
{
    public class EnterPoint : MonoBehaviour
    {
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private Vector3 _playerSpawnOffset;
        [SerializeField] private CameraFollowSystem _cameraSystem;
        [SerializeField] private InputSystem _inputSystem;

        [Space] [Header("Configs")] [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;

        [Space] [Header("UI")] [SerializeField] private WinView _winView;
        [SerializeField] private LooseView _looseView;

        private SessionListener _sessionListener;
        private Transform _playerSpawnPoint;
        private PlayerEntity _player;
        private PlatformsSpawningSystem _platformsSpawningSystem;
        private PlatformInteractingBehaviour _platformInteractingBehaviour;

        private HashSet<PlatformInteractingBehaviour> _interactingBehaviours;
        private SessionPresenter _sessionPresenter;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            StartLevelAsync();
        }

        private async void StartLevelAsync()
        {
            await Task.Delay((int)(_levelConfigs.runDelaySec * 1000));

            _player.StartRun();
        }

        private void Initialize()
        {
            InitPlatforms();
            InitSession();
            InitPlayer();
            InitCamera();
            InitPlatformInteractingSystems();
            InitInputSystem();
            InitUI();
        }

        private void InitUI()
        {
            var ctx = new SessionPresenter.Ctx
            {
                winView = _winView,
                looseView = _looseView
            };

            _sessionPresenter = new SessionPresenter(ctx);
        }

        private void InitInputSystem()
        {
            _inputSystem.hasTouched += OnInput;
        }

        private void OnInput()
        {
            _player.TryJump();
        }

        private void InitPlatforms()
        {
            _poolsConfigs.Initialize();

            _platformsSpawningSystem = new PlatformsSpawningSystem(_levelConfigs);
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent, OnPlayerInterracted, OnPlayerPassed).transform;
            _platformsSpawningSystem.StartSpawningCycleAsync(_platformsParent, OnPlayerInterracted, OnPlayerPassed);
        }

        private void OnPlayerPassed(PlatformType passedType)
        {
            _sessionListener.AddPassedPlatform(passedType);
        }

        private void InitSession()
        {
            var ctx = new SessionListener.Ctx
            {
                playersConfigs = _playersConfigs,
                levelConfigs = _levelConfigs
            };

            _sessionListener = new SessionListener(ctx);
            _sessionListener.SetYValueToFallOut(_playerSpawnPoint.position.y - 2f);

            _sessionListener.onWin += OnGameWin;
        }

        private void OnGameWin(ConcurrentDictionary<PlatformType, int> passedPlatforms)
        {
        }

        private void InitCamera()
        {
            var cameraCtx = new CameraFollowSystem.Ctx
            {
                player = _player.transform,
                cameraSmooth = _playersConfigs.cameraSmooth,
            };

            _cameraSystem.Initialize(cameraCtx);
        }

        private void InitPlatformInteractingSystems()
        {
            var ctx = new PlatformInteractingBehaviour.Ctx
            {
                playerEntity = _player,
                sessionListener = _sessionListener
            };

            _interactingBehaviours = new HashSet<PlatformInteractingBehaviour>
            {
                new AbyssInteractingBehaviour(ctx),
                new AbyssLargeInteractingBehaviour(ctx),
                new FenceInteractingBehaviour(ctx),
                new SawInteractingBehaviour(ctx),
                new TurnLeftInteractingBehaviour(ctx),
                new TurnRightInteractingBehaviour(ctx),
            };
        }

        private void OnPlayerInterracted(Type behaviourType)
        {
            var behaviour = _interactingBehaviours.FirstOrDefault(s => s.GetType() == behaviourType);
            behaviour.InteractWithPlayer();
        }

        private void InitPlayer()
        {
            _player = Instantiate(_playersConfigs.playerPrefab);
            _player.transform.position = _playerSpawnPoint.position + _playerSpawnOffset;

            var ctx = new PlayerEntity.Ctx
            {
                sessionListener = _sessionListener
            };

            _player.Init(ctx);
        }
    }
}