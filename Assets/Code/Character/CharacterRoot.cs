using System.Collections.Generic;
using Behaviour;
using Behaviour.Behaviours.Abstract;
using Configs;
using Cysharp.Threading.Tasks;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Character
{
    /// <summary>
    /// Доменная зона персонажа. Является входной точкой, которая создает
    /// PresentationModel, View и Фабрику поведений персонажа и связывает их посредством реактивных команд
    /// </summary>
    public class CharacterRoot : BaseDisposable
    {
        private Ctx _ctx;
        private CharacterView _view;
        private CharacterState _state;

        private ReactiveCommand<BehaviourType> _onBehaviourAdded;
        private ReactiveTrigger<BehaviourType, CharacterBehaviourPm> _onNewBehaviourProduced;
        private ReactiveCommand<BehaviourType> _onBehaviourFinished;
        private ReactiveTrigger _onRespawned;

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public AssetReference viewReference;
            public BehaviourInfo initialBehaviourInfo;
            public RoadlinePoint spawnPoint;
            public List<RoadlinePoint> roadlinePoints;

            public ReactiveProperty<int> lives;
            public ReactiveProperty<int> coins;

            public ReactiveCommand<Transform> onCharacterInitialized;
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<Direction> onSwipeDirection;
            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveCommand<GameObject> onInteractWithObstacle;
            public ReactiveCommand<Transform> onInteractWithSaveZone;
            public ReactiveTrigger onFinishZoneReached;
            public ReactiveTrigger onCoinTaken;
            public ReactiveTrigger onGameRun;
        }

        public CharacterRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            InitializeRx();
            await InitializeCharacterView();
            InitializeCharacterState();
            InitializeBehaviourFactory();
            InitializeCharacterPm();
            InitializeCharacterInventory();
        }

        private void InitializeCharacterInventory()
        {
            var ctx = new CharacterInventoryPm.Ctx
            {
                coins = _ctx.coins,
                onCoinTaken = _ctx.onCoinTaken
            };

            AddUnsafe(new CharacterInventoryPm(ctx));
        }

        private void InitializeRx()
        {
            _onNewBehaviourProduced = AddUnsafe(new ReactiveTrigger<BehaviourType, CharacterBehaviourPm>());
            _onBehaviourAdded = AddUnsafe(new ReactiveCommand<BehaviourType>());
            _onBehaviourFinished = AddUnsafe(new ReactiveCommand<BehaviourType>());
            _onRespawned = AddUnsafe(new ReactiveTrigger());
        }

        private async UniTask InitializeCharacterView()
        {
            var prefab = await LoadAndTrackPrefab<CharacterView>(_ctx.viewReference);
            _view = Object.Instantiate(prefab);

            var viewCtx = new CharacterView.Ctx
            {
                onInterraction = _ctx.onInterraction,
                onInteractedWIthObstacle = _ctx.onInteractWithObstacle,
                onRespawned = _onRespawned
            };

            _view.SetContext(viewCtx);
        }

        private void InitializeCharacterPm()
        {
            var ctx = new CharacterPm.Ctx
            {
                characterTransform = _view.transform,
                spawnPoint = _ctx.spawnPoint,
                initialBehaviourInfo = _ctx.initialBehaviourInfo,

                onGameRun = _ctx.onGameRun,
                onCharacterInitialized = _ctx.onCharacterInitialized,
                onBehaviourTaken = _ctx.onBehaviourTaken,
                onNewBehaviourProduced = _onNewBehaviourProduced,
                onBehaviourAdded = _onBehaviourAdded,
                onBehaviourFinished = _onBehaviourFinished,
                onFinishZoneReached = _ctx.onFinishZoneReached
            };

            AddUnsafe(new CharacterPm(ctx));
        }

        private void InitializeCharacterState()
        {
            var currentRoadline = new LinkedList<RoadlinePoint>(_ctx.roadlinePoints).Find(_ctx.spawnPoint);

            _state = new CharacterState
            {
                initialSpeed = _ctx.playersConfigs.initialSpeed,
                speed = _ctx.playersConfigs.initialSpeed,
                height = _ctx.initialBehaviourInfo.configs.height,
                lives = _ctx.lives,
                currentSavePoint = currentRoadline.Value.transform,
                currentAction = CharacterAction.Moving,
                currentRoadline = currentRoadline
            };
        }

        private void InitializeBehaviourFactory()
        {
            var behaviourFactoryCtx = new CharacterBehaviourFactoryPm.Ctx
            {
                state = _state,
                animator = _view.animator,
                rigidbody = _view.rigidbody,
                collider = _view.collider,
                landingMask = _ctx.playersConfigs.landingMask,
                characterTransform = _view.transform,
                toleranceDistance = _ctx.playersConfigs.toleranceDistance,
                crashDelay = _ctx.playersConfigs.crashDelay,

                onSwipeDirection = _ctx.onSwipeDirection,
                onBehaviourTaken = _ctx.onBehaviourTaken,
                onNewBehaviourProduced = _onNewBehaviourProduced,
                onBehaviourAdded = _onBehaviourAdded,
                onBehaviourFinished = _onBehaviourFinished,
                onInteractWithObstacle = _ctx.onInteractWithObstacle,
                onInteractWithSaveZone = _ctx.onInteractWithSaveZone,
                onFinishZoneReached = _ctx.onFinishZoneReached,
                onRespawned = _onRespawned
            };

            AddUnsafe(new CharacterBehaviourFactoryPm(behaviourFactoryCtx));
        }
    }
}