using System;
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
        
        private ReactiveCommand<SwipeDirection> _onMovementUpdated;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Collider> _onInterraction;
        private ReactiveEvent<BehaviourType, Type> _onBehaviourAdded;
        private ReactiveCommand<GameObject> _onCrashIntoObstacle;

        public struct Ctx
        {
            public LevelConfigs levelConfigs;
            public PlayersConfigs playersConfigs;
            public PoolsConfigs poolsConfigs;
            public ResourcesConfigs resourcesConfigs;
            public RectTransform uiRoot;
            public Transform spawnPoint;
            public Camera camera;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeInput(ctx);
            InitializeCharacter();
            InitializeInteractionHandler();
        }

        private void InitializeCharacter()
        {
            var stats = new CharacterStats(_ctx.playersConfigs.initialSpeed, _ctx.playersConfigs.jumpForce);

            var characterCtx = new CharacterRoot.Ctx
            {
                stats = stats,
                viewReference = _ctx.resourcesConfigs.characterReference,
                spawnPoint = _ctx.spawnPoint,
                onBehaviourAdded = _onBehaviourAdded,
                onCharacterInitialized = _onCharacterInitialized,
                onInterraction = _onInterraction
            };

            AddUnsafe(new CharacterRoot(characterCtx));
        }

        private void InitializeInteractionHandler()
        {
            var characterCtx = new InteractionHandlerRoot.Ctx
            {
                onInterraction = _onInterraction
            };

            AddUnsafe(new InteractionHandlerRoot(characterCtx));
        }

        private void InitializeRx()
        {
            _onMovementUpdated = AddUnsafe(new ReactiveCommand<SwipeDirection>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onBehaviourAdded = AddUnsafe(new ReactiveEvent<BehaviourType, Type>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
        }

        // private async void RunGameAsync()
        // {
        //     await UniTask.Delay(TimeSpan.FromSeconds(_ctx.levelConfigs.startDelaySec));
        // }

        private void InitializeInput(Ctx ctx)
        {
            var virtualPadEntityCtx = new VirtualPadRoot.Ctx
            {
                rootTransform = ctx.uiRoot,
                virtualPadViewReference = ctx.resourcesConfigs.virtualPadReference,
                onMovementUpdated = _onMovementUpdated,
            };

            AddUnsafe(new VirtualPadRoot(virtualPadEntityCtx));
        }

        private void InitializeCamera(Transform characterTransform)
        {
            var ctx = new CameraRoot.Ctx
            {
                characterTransform = characterTransform,
                camera = _ctx.camera,
                cameraSmooth = _ctx.playersConfigs.cameraSmooth,
                positionOffset = _ctx.levelConfigs.cameraPositionOffset,
                rotationOffset = _ctx.levelConfigs.cameraRotationOffset
            };

            AddUnsafe(new CameraRoot(ctx));
        }
    }
}