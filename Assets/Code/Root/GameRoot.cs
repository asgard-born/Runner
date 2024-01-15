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

        private ReactiveCommand<Direction> _onSwipeDirection;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Collider> _onInterraction;
        private ReactiveCommand<GameObject> _onInteractWithObstacle;
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private ReactiveCommand<Transform> _onInteractWithSaveZone;
        private ReactiveTrigger _onFinishReached;
        private ReactiveTrigger _onGameLoose;
        private ReactiveProperty<int> _lives;

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
            InitializeUI(ctx);
            InitializeCharacter();
            InitializeInteractionReporter();
        }

        private void InitializeRx()
        {
            _lives = AddUnsafe(new ReactiveProperty<int>(_ctx.playersConfigs.initialLives));

            _onSwipeDirection = AddUnsafe(new ReactiveCommand<Direction>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onBehaviourTaken = AddUnsafe(new ReactiveCommand<BehaviourInfo>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());
            _onInteractWithObstacle = AddUnsafe(new ReactiveCommand<GameObject>());
            _onInteractWithSaveZone = AddUnsafe(new ReactiveCommand<Transform>());
            _onFinishReached = AddUnsafe(new ReactiveTrigger());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
        }

        private void InitializeUI(Ctx ctx)
        {
            var uiRootCtx = new UIRoot.Ctx
            {
                uiTransform = ctx.uiTransform,
                resourcesConfigs = ctx.resourcesConfigs,
                onSwipeDirection = _onSwipeDirection
            };

            AddUnsafe(new UIRoot(uiRootCtx));
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

                onCharacterInitialized = _onCharacterInitialized,
                onInterraction = _onInterraction,
                onSwipeDirection = _onSwipeDirection,
                onBehaviourTaken = _onBehaviourTaken,
                onInteractWithObstacle = _onInteractWithObstacle,
                onFinishReached = _onFinishReached
            };

            AddUnsafe(new CharacterRoot(ctx));
        }

        private void InitializeInteractionReporter()
        {
            var characterCtx = new InteractionReporterPm.Ctx
            {
                layersDictionary = _ctx.globalConfigs.layersDictionary,

                onInterraction = _onInterraction,
                onBehaviourTaken = _onBehaviourTaken,
                onInteractWithObstacle = _onInteractWithObstacle,
                onInteractWithSaveZone = _onInteractWithSaveZone,
                onFinishReached = _onFinishReached
            };

            AddUnsafe(new InteractionReporterPm(characterCtx));
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
    }
}