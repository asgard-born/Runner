using System.Collections.Generic;
using Behaviour;
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

        private ReactiveTrigger<float> _onSpeedChangedCallback;
        private ReactiveCommand<BehaviourType> _onBehaviourAdded;
        private ReactiveTrigger<BehaviourType, CharacterBehaviourPm> _onNewBehaviourProduced;
        private ReactiveCommand<BehaviourType> _onBehaviourFinished;
        
        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public AssetReference viewReference;
            public BehaviourInfo initialBehaviourInfo;
            public RoadlinePoint spawnPoint;
            public List<RoadlinePoint> roadlinePoints;

            public ReactiveCommand<Transform> onCharacterInitialized;
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
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
        }

        private void InitializeRx()
        {
            _onNewBehaviourProduced = AddUnsafe(new ReactiveTrigger<BehaviourType, CharacterBehaviourPm>());
            _onBehaviourAdded = AddUnsafe(new ReactiveCommand<BehaviourType>());
            _onBehaviourFinished = AddUnsafe(new ReactiveCommand<BehaviourType>());
        }

        private async UniTask InitializeCharacterView()
        {
            var prefab = await LoadAndTrackPrefab<CharacterView>(_ctx.viewReference);
            _view = Object.Instantiate(prefab);

            var viewCtx = new CharacterView.Ctx
            {
                onInterraction = _ctx.onInterraction
            };

            _view.SetCtx(viewCtx);
        }

        private void InitializeCharacterPm()
        {
            var ctx = new CharacterPm.Ctx
            {
                characterTransform = _view.transform,
                spawnPoint = _ctx.spawnPoint,
                initialBehaviourInfo = _ctx.initialBehaviourInfo,

                onCharacterInitialized = _ctx.onCharacterInitialized,
                onBehaviourTaken = _ctx.onBehaviourTaken,
                onNewBehaviourProduced = _onNewBehaviourProduced,
                onBehaviourAdded = _onBehaviourAdded,
                onBehaviourFinished = _onBehaviourFinished
            };

            AddUnsafe(new CharacterPm(ctx));
        }

        private void InitializeCharacterState()
        {
            _state = new CharacterState
            {
                initialVelocity = _ctx.playersConfigs.initialSpeed,
                velocity = _ctx.playersConfigs.initialSpeed,
                currentRoadline = new LinkedList<RoadlinePoint>(_ctx.roadlinePoints).Find(_ctx.spawnPoint),
                jumpForce = _ctx.playersConfigs.jumpForce
            };
        }

        private void InitializeBehaviourFactory()
        {
            var behaviourFactoryCtx = new CharacterBehaviourFactoryPm.Ctx
            {
                state = _state,
                animator = _view.animator,
                rigidbody = _view.rigidbody,
                characterTransform = _view.transform,

                onSwipeDirection = _ctx.onSwipeDirection,
                onBehaviourTaken = _ctx.onBehaviourTaken,
                onNewBehaviourProduced = _onNewBehaviourProduced,
                onBehaviourAdded = _onBehaviourAdded
            };

            AddUnsafe(new CharacterBehaviourFactoryPm(behaviourFactoryCtx));
        }
    }
}