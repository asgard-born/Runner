using System;
using System.Collections.Generic;
using CameraLogic;
using Character;
using Configs;
using Cysharp.Threading.Tasks;
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

        private ReactiveTrigger _onGameRun;
        private ReactiveCommand<Direction> _onSwipeDirection;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Collider> _onInterraction;
        private ReactiveCommand<GameObject> _onInteractedWithObstacle;
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public GlobalConfigs globalConfigs;
            public ResourcesConfigs resourcesConfigs;
            public CameraConfigs cameraConfigs;
            public RectTransform uiRoot;
            public List<RoadlinePoint> roadlinePoints;
            public Camera camera;
            public RoadlinePoint spawnPoint;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeInput(ctx);
            InitializeCharacter();
            InitializeInteractionReporter();
        }

        private void InitializeRx()
        {
            _onGameRun = AddUnsafe(new ReactiveTrigger());
            _onSwipeDirection = AddUnsafe(new ReactiveCommand<Direction>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onInteractedWithObstacle = AddUnsafe(new ReactiveCommand<GameObject>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());
            _onBehaviourTaken = AddUnsafe(new ReactiveCommand<BehaviourInfo>());

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
                onInteractedWithObstacle = _onInteractedWithObstacle
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
                onInteractedWithObstacle = _onInteractedWithObstacle
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

        private async void RunGameAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_ctx.globalConfigs.startDelaySec));

            _onGameRun?.Notify();
        }
    }
}