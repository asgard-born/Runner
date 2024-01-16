using System.Collections.Generic;
using CameraLogic;
using Character;
using Configs;
using Framework;
using Framework.Reactive;
using Interactions;
using Shared;
using UI.Roots;
using UniRx;
using UnityEngine;

namespace Root
{
    /// <summary>
    /// Главный Root-объект ветвления Composition-Tree. Создает реактивные объекты (ReactiveTrigger/ReactiveCommand),
    /// для осуществление слабого зацепления другими классами, идущими от него, такими как:
    /// другие Roots - ветки дерева, которые будут создавать свои Pms и Views, слабо зацепленные между собой;
    /// а также отдельно живущие Presentation Model'и (Pm), для которых Root's не нужны.
    /// Производит базовую инициализацию. 
    /// </summary>
    public class GameRoot : BaseDisposable
    {
        private CameraPm _cameraPm;
        private UIRoot _uiRoot;

        private readonly Ctx _ctx;

        private ReactiveProperty<int> _lives;
        private ReactiveProperty<int> _coins;

        private ReactiveTrigger _onGameRun;
        private ReactiveTrigger _onGameOver;
        private ReactiveTrigger _onGameWin;
        private ReactiveTrigger _onInitialized;
        private ReactiveTrigger _onFinishZoneReached;
        private ReactiveTrigger _onCoinTaken;
        private ReactiveTrigger _onRestartLevel;
        private ReactiveTrigger _onNextLevel;
        private ReactiveCommand<Direction> _onSwipeDirection;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Collider> _onInterraction;
        private ReactiveCommand<GameObject> _onInteractWithObstacle;
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private ReactiveCommand<Transform> _onInteractWithSaveZone;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public LevelConfigs levelConfigs;
            public ResourcesConfigs resourcesConfigs;
            public CameraConfigs cameraConfigs;
            public AudioConfigs audioConfigs;

            public RectTransform uiTransform;
            public List<RoadlinePoint> roadlinePoints;
            public Camera camera;
            public RoadlinePoint spawnPoint;
            public AudioSource audioSource;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeGameStateListenerPm();
            InitializeInteractionHandler();
            InitializeCharacter();
        }

        private void AfterCharacterInitizlied(Transform characterTransform)
        {
            InitializeCamera(characterTransform);
            InitializeUI(_ctx);
            InitializeSoundPlayer();
            InitializeSceneLoaderPm();

            _onInitialized?.Notify();
        }

        private void InitializeRx()
        {
            _lives = AddUnsafe(new ReactiveProperty<int>(_ctx.playersConfigs.initialLives));
            _coins = AddUnsafe(new ReactiveProperty<int>());

            _onGameWin = AddUnsafe(new ReactiveTrigger());
            _onGameOver = AddUnsafe(new ReactiveTrigger());
            _onInitialized = AddUnsafe(new ReactiveTrigger());
            _onCoinTaken = AddUnsafe(new ReactiveTrigger());
            _onGameRun = AddUnsafe(new ReactiveTrigger());
            _onRestartLevel = AddUnsafe(new ReactiveTrigger());
            _onNextLevel = AddUnsafe(new ReactiveTrigger());

            _onInteractWithSaveZone = AddUnsafe(new ReactiveCommand<Transform>());
            _onSwipeDirection = AddUnsafe(new ReactiveCommand<Direction>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onBehaviourTaken = AddUnsafe(new ReactiveCommand<BehaviourInfo>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());
            _onInteractWithObstacle = AddUnsafe(new ReactiveCommand<GameObject>());
            _onFinishZoneReached = AddUnsafe(new ReactiveTrigger());

            AddUnsafe(_onCharacterInitialized.Subscribe(AfterCharacterInitizlied));
        }

        private void InitializeCharacter()
        {
            var ctx = new CharacterRoot.Ctx
            {
                playersConfigs = _ctx.playersConfigs,
                viewReference = _ctx.resourcesConfigs.characterReference,
                initialBehaviourInfo = _ctx.playersConfigs.initialBehaviourInfo,
                spawnPoint = _ctx.spawnPoint,
                roadlinePoints = _ctx.roadlinePoints,
                lives = _lives,
                coins = _coins,

                onGameRun = _onGameRun,
                onCharacterInitialized = _onCharacterInitialized,
                onInterraction = _onInterraction,
                onSwipeDirection = _onSwipeDirection,
                onBehaviourTaken = _onBehaviourTaken,
                onInteractWithObstacle = _onInteractWithObstacle,
                onInteractWithSaveZone = _onInteractWithSaveZone,
                onFinishZoneReached = _onFinishZoneReached,
                onCoinTaken = _onCoinTaken,
                onGameWin = _onGameWin,
                onGameOver = _onGameOver
            };

            AddUnsafe(new CharacterRoot(ctx));
        }

        private void InitializeInteractionHandler()
        {
            var characterCtx = new InteractionHandlerPm.Ctx
            {
                layersDictionary = _ctx.levelConfigs.layersDictionary,

                onInterraction = _onInterraction,
                onBehaviourTaken = _onBehaviourTaken,
                onInteractWithObstacle = _onInteractWithObstacle,
                onInteractWithSaveZone = _onInteractWithSaveZone,
                onFinishZoneReached = _onFinishZoneReached,
                onCoinTaken = _onCoinTaken
            };

            AddUnsafe(new InteractionHandlerPm(characterCtx));
        }

        private void InitializeCamera(Transform characterTransform)
        {
            var ctx = new CameraPm.Ctx
            {
                characterTransform = characterTransform,
                cameraTransform = _ctx.camera.transform,
                speed = _ctx.cameraConfigs.speed,
                positionOffset = _ctx.cameraConfigs.positionOffset
            };

            AddUnsafe(new CameraPm(ctx));
        }

        private void InitializeUI(Ctx ctx)
        {
            var uiRootCtx = new UIRoot.Ctx
            {
                uiTransform = ctx.uiTransform,
                resourcesConfigs = ctx.resourcesConfigs,
                onSwipeDirection = _onSwipeDirection,

                lives = _lives,
                coins = _coins,

                onGameOver = _onGameOver,
                onGameWin = _onGameWin,
                onRestartLevel = _onRestartLevel,
                onNextLevel = _onNextLevel
            };

            AddUnsafe(new UIRoot(uiRootCtx));
        }

        private void InitializeSoundPlayer()
        {
            var ctx = new AudioPlayerPm.Ctx
            {
                configs = _ctx.audioConfigs,
                audioSource = _ctx.audioSource,
                onGameWin = _onGameWin,
                onGameOver = _onGameOver,
                onGameRun = _onGameRun
            };

            AddUnsafe(new AudioPlayerPm(ctx));
        }

        private void InitializeGameStateListenerPm()
        {
            var ctx = new GameStateListenerPm.Ctx
            {
                startDelaySec = _ctx.levelConfigs.startDelaySec,
                onGameInitialized = _onInitialized,
                onGameRun = _onGameRun,
                onGameOver = _onGameOver,
                onGameWin = _onGameWin,
                onFinishZoneReached = _onFinishZoneReached,
                lives = _lives
            };

            AddUnsafe(new GameStateListenerPm(ctx));
        }

        private void InitializeSceneLoaderPm()
        {
            var ctx = new SceneLoaderPm.Ctx
            {
                onRestartLevel = _onRestartLevel,
                onNextLevel = _onNextLevel
            };

            AddUnsafe(new SceneLoaderPm(ctx));
        }
    }
}