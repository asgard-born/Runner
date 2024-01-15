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
    public class GameRoot : BaseDisposable
    {
        private CameraPm _cameraPm;
        private UIRoot _uiRoot;

        private readonly Ctx _ctx;

        private ReactiveProperty<int> _lives;
        private ReactiveProperty<int> _coins;

        private ReactiveCommand<Direction> _onSwipeDirection;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Collider> _onInterraction;
        private ReactiveCommand<GameObject> _onInteractWithObstacle;
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private ReactiveCommand<Transform> _onInteractWithSaveZone;
        private ReactiveTrigger _onFinishReached;
        private ReactiveTrigger _onCoinTaken;
        private ReactiveTrigger _onGameLoose;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public GlobalConfigs globalConfigs;
            public ResourcesConfigs resourcesConfigs;
            public CameraConfigs cameraConfigs;
            public RectTransform uiTransform;
            public List<RoadlinePoint> roadlinePoints;
            public Camera camera;
            public RoadlinePoint spawnPoint;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeInteractionHandler();
            InitializeUI(ctx);
            InitializeCharacter();
        }

        private void InitializeRx()
        {
            _lives = AddUnsafe(new ReactiveProperty<int>(_ctx.playersConfigs.initialLives));
            _coins = AddUnsafe(new ReactiveProperty<int>());

            _onInteractWithSaveZone = AddUnsafe(new ReactiveCommand<Transform>());
            _onSwipeDirection = AddUnsafe(new ReactiveCommand<Direction>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onBehaviourTaken = AddUnsafe(new ReactiveCommand<BehaviourInfo>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());
            _onInteractWithObstacle = AddUnsafe(new ReactiveCommand<GameObject>());
            _onFinishReached = AddUnsafe(new ReactiveTrigger());
            _onCoinTaken = AddUnsafe(new ReactiveTrigger());
            _onGameLoose = AddUnsafe(new ReactiveTrigger());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
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

                onCharacterInitialized = _onCharacterInitialized,
                onInterraction = _onInterraction,
                onSwipeDirection = _onSwipeDirection,
                onBehaviourTaken = _onBehaviourTaken,
                onInteractWithObstacle = _onInteractWithObstacle,
                onInteractWithSaveZone = _onInteractWithSaveZone,
                onFinishReached = _onFinishReached,
                onCoinTaken = _onCoinTaken
            };

            AddUnsafe(new CharacterRoot(ctx));
        }

        private void InitializeInteractionHandler()
        {
            var characterCtx = new InteractionHandlerPm.Ctx
            {
                layersDictionary = _ctx.globalConfigs.layersDictionary,

                onInterraction = _onInterraction,
                onBehaviourTaken = _onBehaviourTaken,
                onInteractWithObstacle = _onInteractWithObstacle,
                onInteractWithSaveZone = _onInteractWithSaveZone,
                onFinishReached = _onFinishReached,
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
                coins = _coins
            };

            AddUnsafe(new UIRoot(uiRootCtx));
        }
    }
}