using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Code.CameraLogic;
using Code.Configs;
using Code.Platforms;
using Code.Platforms.Abstract;
using Code.Platforms.Essences;
using Code.PlatformsBehaviour;
using Code.PlatformsBehaviour.Abstract;
using Code.Player;
using Code.PlayersInput;
using Code.Session;
using Code.UI.Screens;
using Code.UI.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private Vector3 _playerSpawnOffset;
        [SerializeField] private CameraFollowSystem _cameraSystem;
        [SerializeField] private InputSystem _inputSystem;

        [Space] [Header("Configs")]
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;

        [Space] [Header("UI")]
        [SerializeField] private HUDView _hudView;
        [SerializeField] private WinView _winView;
        [SerializeField] private LooseView _looseView;
        [SerializeField] private Canvas _canvas;

        private SessionListener _sessionListener;
        private Transform _playerSpawnPoint;
        private PlayerController _player;
        private PlatformsSpawningSystem _platformsSpawningSystem;
        private PlatformsDestroyingSystem _platformsDestroyingSystem;
        private readonly LinkedList<Platform> _platforms = new LinkedList<Platform>();
        private HashSet<PlatformInteractingBehaviour> _interactingBehaviours;

        private HUDScreen _hudScreen;
        private LooseScreen _looseScreen;
        private WinScreen _winScreen;

        private void Awake()
        {
            StartLevel();
        }

        private void Start()
        {
            StartLevelAsync();
        }

        private async void StartLevelAsync()
        {
            await UniTask.Delay((int)(_levelConfigs.runDelaySec * 1000));

            _player.StartRun();
        }

        private void StartLevel()
        {
            InitPlatforms();
            InitSession();
            InitPlayer();
            InitCamera();
            InitInputSystem();
            InitUI();
            InitInteractingWithPlatformBehaviours();
        }

        private void RestartLevel()
        {
        }

        private void FinishLevel()
        {
        }

        private void InitUI()
        {
            var hudCtx = new HUDScreen.Ctx
            {
                viewPrefab = _hudView,
                canvas = _canvas.transform,
                initLives = _playersConfigs.lives
            };

            _hudScreen = new HUDScreen(hudCtx);
            _hudScreen.Show();

            var winCtx = new WinScreen.Ctx
            {
                blocksToCalculateOnFinish = _levelConfigs.blocksToCalculateOnFinish,
                viewPrefab = _winView,
                canvas = _canvas.transform,
            };

            _winScreen = new WinScreen(winCtx);

            var looseCtx = new LooseScreen.Ctx
            {
                viewPrefab = _looseView,
                canvas = _canvas.transform,
            };

            _looseScreen = new LooseScreen(looseCtx);
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

            var spawningSystemCtx = new PlatformsSpawningSystem.Ctx
            {
                platforms = _platforms,
                levelConfigs = _levelConfigs
            };

            _platformsSpawningSystem = new PlatformsSpawningSystem(spawningSystemCtx);
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent, OnPlayerInterracted, OnPlayerPassed).transform;
            _platformsSpawningSystem.StartSpawningCycleAsync(_platformsParent, OnPlayerInterracted, OnPlayerPassed);

            var destroyingSystemCtx = new PlatformsDestroyingSystem.Ctx
            {
                platforms = _platforms,
                levelConfigs = _levelConfigs
            };

            _platformsDestroyingSystem = new PlatformsDestroyingSystem(destroyingSystemCtx);
            // _platformsDestroyingSystem.StartDestroyingCycleAsync();
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
            _player.Stop(true);
            _winScreen.Show(passedPlatforms);
        }

        private void OnGameLoose()
        {
            _player.Stop();
            _looseScreen.Show();
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

        private void InitInteractingWithPlatformBehaviours()
        {
            var fenceCtx = new FenceInteractingBehaviour.Ctx
            {
                player = _player,
                hudScreen = _hudScreen
            };

            var sawCtx = new SawInteractingBehaviour.Ctx
            {
                player = _player,
                hudScreen = _hudScreen
            };

            _interactingBehaviours = new HashSet<PlatformInteractingBehaviour>
            {
                new AbyssInteractingBehaviour(OnGameLoose),
                new AbyssLargeInteractingBehaviour(OnGameLoose),
                new FenceInteractingBehaviour(fenceCtx),
                new SawInteractingBehaviour(sawCtx),
                new TurnLeftInteractingBehaviour(_player),
                new TurnRightInteractingBehaviour(_player),
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

            var ctx = new PlayerController.Ctx
            {
                playersConfigs = _playersConfigs,
                sessionListener = _sessionListener,
                deathCallback = OnGameLoose
            };

            _player.Init(ctx);
        }
    }
}