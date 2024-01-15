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
        private ReactiveCommand<GameObject> _onInteractedWithObstacle;
        private ReactiveCommand<BehaviourInfo> _onBehaviourTaken;
        private ReactiveCommand<Transform> _onInteractedWithSaveZone;
        private ReactiveTrigger _onGameWin;
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
            InitializeInput(ctx);
            InitializeCharacter();
            InitializeInteractionReporter();
        }

        private void InitializeRx()
        {
            _onSwipeDirection = AddUnsafe(new ReactiveCommand<Direction>());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onBehaviourTaken = AddUnsafe(new ReactiveCommand<BehaviourInfo>());
            _onInterraction = AddUnsafe(new ReactiveCommand<Collider>());
            _onInteractedWithObstacle = AddUnsafe(new ReactiveCommand<GameObject>());
            _onInteractedWithSaveZone = AddUnsafe(new ReactiveCommand<Transform>());
            _onGameWin = AddUnsafe(new ReactiveTrigger());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
        }

        private void InitializeInput(Ctx ctx)
        {
            var virtualPadEntityCtx = new VirtualPadRoot.Ctx
            {
                uiTransform = ctx.uiTransform,
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
                onInteractedWithObstacle = _onInteractedWithObstacle,
                onInteractedWithSaveZone = _onInteractedWithSaveZone,
                onFinish = _onGameWin
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