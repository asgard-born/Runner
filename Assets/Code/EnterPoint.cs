﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.CameraLogic;
using Code.Configs;
using Code.Platforms;
using Code.Platforms.Concrete;
using Code.PlatformsBehaviour;
using Code.PlatformsBehaviour.Abstract;
using Code.Player;
using Code.PlayersInput;
using UnityEngine;

namespace Code
{
    public class EnterPoint : MonoBehaviour
    {
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private Vector3 _playerSpawnOffset;
        [SerializeField] private CameraFollowSystem _cameraSystem;
        [SerializeField] private InputSystem _inputSystem;

        private SessionStats _sessionStats;
        private Transform _playerSpawnPoint;
        private PlayerEntity _player;
        private PlatformsSpawningSystem _platformsSpawningSystem;
        private PlatformInteractingBehaviour _platformInteractingBehaviour;

        private HashSet<PlatformInteractingBehaviour> _interactingBehaviours;

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
            InitSession();
            InitPlatforms();
            InitPlayer();
            InitCamera();
            InitPlatformInteractingSystems();
            InitInputSystem();
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
            _playerSpawnPoint = _platformsSpawningSystem.SpawnImmediately(_platformsParent, OnPlayerInterracted).transform;
            _platformsSpawningSystem.StartSpawningCycleAsync(_platformsParent, OnPlayerInterracted);
        }

        private void InitSession()
        {
            var ctx = new SessionStats.Ctx
            {
                playersConfigs = _playersConfigs,
                levelConfigs = _levelConfigs
            };

            _sessionStats = new SessionStats(ctx);
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
                sessionStats = _sessionStats
            };

            _interactingBehaviours = new HashSet<PlatformInteractingBehaviour>
            {
                new StandardInteractingBehaviour(ctx),
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
                sessionStats = _sessionStats
            };

            _player.Init(ctx);
        }
    }
}