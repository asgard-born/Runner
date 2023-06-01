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
using UnityEngine.SceneManagement;

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
        private Platform _playerSpawnPoint;
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
            await UniTask.Delay((int)(_levelConfigs.startDelaySec * 1000));

            _player.StartRun();
        }

        private void StartLevel()
        {
            InitBonuses();
            InitSession();
            InitPlatforms();
            InitPlayer();
            InitCamera();
            InitInputSystem();
            InitScreens();
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
        
        private void Respawn()
        {
            _player.Respawn(_platforms);
            _platformsSpawningSystem.Resume();
            _platformsDestroyingSystem.Resume();
            
            _looseScreen.Hide();
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void InitScreens()
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
                nextLevelCallback = NextLevel
            };

            _winScreen = new WinScreen(winCtx);

            var looseCtx = new LooseScreen.Ctx
            {
                viewPrefab = _looseView,
                canvas = _canvas.transform,
                continueCallback = Respawn,
                restartCallback = RestartLevel
            };

            _looseScreen = new LooseScreen(looseCtx);
        }

        private void NextLevel()
        {
            RestartLevel();
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
                sessionListener = _sessionListener,
                interactionCallback = OnPlayerInterracted,
                passingCallback = OnPlayerPassed
            };

            _platformsSpawningSystem = new PlatformsSpawningSystem(spawningSystemCtx);
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent);
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

        private void OnPlayerPassed(Platform passedPlatform)
        {
            _sessionListener.AddPassedPlatform(passedPlatform.platformType);
            _player.currentPlatform = passedPlatform;
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

        private void GameOver()
        {
            _player.Stop();
            _looseScreen.Show();
            
            _platformsSpawningSystem.Pause();
            _platformsDestroyingSystem.Pause();
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
                new FenceInteractingBehaviour(_player),
                new SawInteractingBehaviour(_player),
                new TurnLeftInteractingBehaviour(_player),
                new TurnRightInteractingBehaviour(_player),
            };
        }

        private void OnPlayerInterracted(Platform platform)
        {
            var behaviour = _interactingBehaviours.FirstOrDefault(s => s.GetType() == platform.behaviourType);
            behaviour.InteractWithPlayer(platform);
        }

        private void InitPlayer()
        {
            _player = Instantiate(_playersConfigs.playerPrefab);
            _player.transform.position = _playerSpawnPoint.transform.position + _playerSpawnOffset;

            var ctx = new PlayerController.Ctx
            {
                playersConfigs = _playersConfigs,
                playerSpawnPoint = _playerSpawnPoint,
                onLivesChangedCallback = OnLivesChanged,
                onSpeedChangedCallback = OnSpeedChanged,
                deathCallback = GameOver
            };

            _player.Init(ctx);
        }

        private void OnSpeedChanged(float previousValue, float factor)
        {
            _sessionListener.OnSpeedChanged(previousValue, factor);
        }

        private void OnLivesChanged(float value)
        {
            _hudScreen.UpdateLives((int)value);
        }
    }
}