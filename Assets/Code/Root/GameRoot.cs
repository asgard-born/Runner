using System;
using System.Collections.Generic;
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
        private ReactiveCommand<BehaviourInfo> _onBehaviourAdded;
        private ReactiveCommand<CharacterBehaviourPm> _onBehaviourCreated;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public LevelConfigs levelConfigs;
            public ResourcesConfigs resourcesConfigs;
            public CameraConfigs cameraConfigs;
            public RectTransform uiRoot;
            public LinkedListNode<Roadline> spawnRoadlineNode;
            public Camera camera;
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
            _onBehaviourAdded = AddUnsafe(new ReactiveCommand<BehaviourInfo>());
            _onBehaviourCreated = AddUnsafe(new ReactiveCommand<CharacterBehaviourPm>());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
        }

        private void InitializeInput(Ctx ctx)
        {
            var virtualPadEntityCtx = new VirtualPadRoot.Ctx
            {
                rootTransform = ctx.uiRoot,
                virtualPadViewReference = ctx.resourcesConfigs.virtualPadReference,
                onSwipeDirection = _onSwipeDirection
            };

            AddUnsafe(new VirtualPadRoot(virtualPadEntityCtx));
        }

        private void InitializeCharacter()
        {
            var state = new CharacterState
            {
                initialSpeed = _ctx.playersConfigs.initialSpeed,
                speed = _ctx.playersConfigs.initialSpeed,
                currentRoadline = _ctx.spawnRoadlineNode,
                jumpForce = _ctx.playersConfigs.jumpForce
            };

            var ctx = new CharacterRoot.Ctx
            {
                state = state,
                viewReference = _ctx.resourcesConfigs.characterReference,
                spawnPoint = state.currentRoadline.Value.transform,

                onCharacterInitialized = _onCharacterInitialized,
                onInterraction = _onInterraction,
                onSwipeDirection = _onSwipeDirection,
                onBehaviourAdded = _onBehaviourAdded,
                onBehaviourCreated = _onBehaviourCreated
            };

            AddUnsafe(new CharacterRoot(ctx));
        }

        private void InitializeServices()
        {
            var characterCtx = new InteractionReporter.Ctx
            {
                onInterraction = _onInterraction,
                onBehaviourAdded = _onBehaviourAdded,
                onCrashIntoObstacle = _onCrashIntoObstacle
            };

            AddUnsafe(new InteractionReporter(characterCtx));
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
                smoothFactor = _ctx.cameraConfigs.smoothFactor,
                positionOffset = _ctx.cameraConfigs.positionOffset,
                rotationOffset = _ctx.cameraConfigs.rotationOffset
            };

            AddUnsafe(new CameraPm(ctx));
        }
    }
}