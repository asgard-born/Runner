using System;
using Behaviour;
using CameraLogic;
using Character;
using Configs;
using Cysharp.Threading.Tasks;
using Framework;
using Framework.Reactive;
using Interactions;
using Obstacles;
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

        private ReactiveTrigger _onGameRun;
        private ReactiveCommand<SwipeDirection> _onSwipeDirection;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Collider> _onInterraction;
        private ReactiveCommand<Obstacle> _onCrashIntoObstacle;
        private ReactiveCommand<BehaviourConfigs> _onBehaviourAdded;
        private ReactiveCommand<CharacterBehaviourPm> _onBehaviourCreated;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public LevelConfigs levelConfigs;
            public ResourcesConfigs resourcesConfigs;
            public CameraConfigs cameraConfigs;
            public RectTransform uiRoot;
            public Transform spawnPoint;
            public Camera camera;
            public OrientationAxises orientationAxises;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeInput(ctx);
            InitializeCharacter();
            InitializeServices();
        }

        private void InitializeRx()
        {
            _onGameRun = AddUnsafe(new ReactiveTrigger());
            _onSwipeDirection = AddUnsafe(new ReactiveCommand<SwipeDirection>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onCrashIntoObstacle = AddUnsafe(new ReactiveCommand<Obstacle>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());
            _onBehaviourAdded = AddUnsafe(new ReactiveCommand<BehaviourConfigs>());
            _onBehaviourCreated = AddUnsafe(new ReactiveCommand<CharacterBehaviourPm>());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
        }

        private void InitializeInput(Ctx ctx)
        {
            var virtualPadEntityCtx = new VirtualPadRoot.Ctx
            {
                rootTransform = ctx.uiRoot,
                virtualPadViewReference = ctx.resourcesConfigs.virtualPadReference,
                onSwipeDirection = _onSwipeDirection,
            };

            AddUnsafe(new VirtualPadRoot(virtualPadEntityCtx));
        }

        private void InitializeCharacter()
        {
            var stats = new CharacterStats(_ctx.playersConfigs.initialSpeed, _ctx.playersConfigs.jumpForce);

            var characterCtx = new CharacterRoot.Ctx
            {
                stats = stats,
                viewReference = _ctx.resourcesConfigs.characterReference,
                spawnPoint = _ctx.spawnPoint,
                orientationAxises = _ctx.orientationAxises,

                onCharacterInitialized = _onCharacterInitialized,
                onInterraction = _onInterraction,
                onBehaviourCreated = _onBehaviourCreated,
                onSwipeDirection = _onSwipeDirection
            };

            AddUnsafe(new CharacterRoot(characterCtx));
        }

        private void InitializeServices()
        {
            var characterCtx = new InteractionReporterService.Ctx
            {
                onInterraction = _onInterraction,
                onBehaviourAdded = _onBehaviourAdded,
                onCrashIntoObstacle = _onCrashIntoObstacle
            };

            AddUnsafe(new InteractionReporterService(characterCtx));
        }

        private async void RunGameAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_ctx.levelConfigs.startDelaySec));

            _onGameRun?.Notify();
        }

        private void InitializeCamera(Transform characterTransform)
        {
            var ctx = new CameraPm.Ctx
            {
                characterTransform = characterTransform,
                cameraTransform = _ctx.camera.transform,
                cameraSmooth = _ctx.cameraConfigs.smooth,
                positionOffset = _ctx.cameraConfigs.positionOffset,
                rotationOffset = _ctx.cameraConfigs.rotationOffset
            };

            AddUnsafe(new CameraPm(ctx));
        }
    }
}