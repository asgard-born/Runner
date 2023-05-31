using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Code.Boosters;
using Code.CameraLogic;
using Code.Configs;
using Code.Platforms;
using Code.Platforms.Abstract;
using Code.Platforms.Concrete;
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

        [Space, Header("Configs")]
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;

        [Space, Header("UI")]
        [SerializeField] private HUDView _hudView;
        [SerializeField] private WinView _winView;
        [SerializeField] private LooseView _looseView;
        [SerializeField] private Canvas _canvas;

        private SessionListener _sessionListener;
        private Transform _playerSpawnPoint;
        private PlayerController _player;
        private PlatformsSpawningSystem _platformsSpawningSystem;
        private PlatformsDestroyingSystem _platformsDestroyingSystem;
        private SpawnBonusSystem _spawnBonusSystem;
        
        private HashSet<PlatformInteractingBehaviour> _interactingBehaviours;
        private readonly LinkedList<Platform> _platforms = new LinkedList<Platform>();

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
            InitBonuses();
            InitPlatforms();
            InitSession();
            InitPlayer();
            InitCamera();
            InitInputSystem();
            InitUI();
            InitInteractingBehaviours();
        }

        private void InitBonuses()
        {
            var ctx = new SpawnBonusSystem.Ctx
            {
                levelConfigs = _levelConfigs
            };
                
            _spawnBonusSystem = new SpawnBonusSystem(ctx);
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
                blocksToCalculateOnFinish = _levelConfigs.platformsToCalculateOnFinish,
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
                levelConfigs = _levelConfigs,
                spawnedCallback = OnPlatformSpawned,
                interactionCallback = OnPlayerInterracted,
                passingCallback = OnPlayerPassed
            };

            _platformsSpawningSystem = new PlatformsSpawningSystem(spawningSystemCtx);
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent).transform;
            _platformsSpawningSystem.StartSpawningCycleAsync(_platformsParent);

            var destroyingSystemCtx = new PlatformsDestroyingSystem.Ctx
            {
                platforms = _platforms,
                levelConfigs = _levelConfigs
            };

            _platformsDestroyingSystem = new PlatformsDestroyingSystem(destroyingSystemCtx);
            _platformsDestroyingSystem.StartDestroyingCycleAsync();
        }

        private void OnPlatformSpawned(Platform platform)
        {
            if (platform is StandardPlatform standardPlatform)
            {
                _spawnBonusSystem.TrySpawnBonus(standardPlatform);
            }
        }

        private void OnPlayerPassed(PlatformType passedType)
        {
            _sessionListener.AddPassedPlatform(passedType);
        }

        private void InitSession()
        {
            var ctx = new SessionListener.Ctx
            {
                levelConfigs = _levelConfigs
            };

            _sessionListener = new SessionListener(ctx);

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

        private void InitInteractingBehaviours()
        {
            _interactingBehaviours = new HashSet<PlatformInteractingBehaviour>
            {
                new AbyssInteractingBehaviour(OnGameLoose),
                new AbyssLargeInteractingBehaviour(OnGameLoose),
                new FenceInteractingBehaviour(_player),
                new SawInteractingBehaviour(_player),
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
                playerSpawnPoint = _playerSpawnPoint,
                onLivesChangedCallback = OnLivesChanged,
                deathCallback = OnGameLoose
            };

            _player.Init(ctx);
        }

        private void OnLivesChanged(float value)
        {
            _hudScreen.UpdateLives((int)value);
        }
    }
}