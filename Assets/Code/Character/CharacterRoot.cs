using Behaviour;
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
    /// Доменная зона персонажа. Является входной точкой, которая создает Presentation Model и View персонажа,
    /// а также реализует их слабую связанность посредством реактивных команд.
    /// </summary>
    public class CharacterRoot : BaseDisposable
    {
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveTrigger<float> _onSpeedChangedCallback;
        private CharacterView _view;

        private Ctx _ctx;

        public struct Ctx
        {
            public CharacterState state;
            public AssetReference viewReference;
            public Transform spawnPoint;
            public BehaviourInfo defaultBehaviourInfo;
            
            public ReactiveCommand<Transform> onCharacterInitialized;
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
            public ReactiveCommand<BehaviourInfo> onBehaviourAdded;
            public ReactiveCommand<CharacterBehaviourPm> onBehaviourCreated;
        }

        public CharacterRoot(Ctx ctx)
        {
            _ctx = ctx;
            _onCharacterInitialized = ctx.onCharacterInitialized;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializeCharacterAsync();
            InitializeBehaviourFactory();
        }
       

        private async UniTask InitializeCharacterAsync()
        {
            var prefab = await LoadAndTrackPrefab<CharacterView>(_ctx.viewReference);

            _view = Object.Instantiate(prefab);

            var viewCtx = new CharacterView.Ctx
            {
                onInterraction = _ctx.onInterraction
            };

            _view.SetCtx(viewCtx);

            var ctx = new CharacterPm.Ctx
            {
                characterTransform = _view.transform,
                spawnPoint = _ctx.spawnPoint,

                onBehaviourAdded = _ctx.onBehaviourAdded,
                onBehaviourCreated = _ctx.onBehaviourCreated,
                onCharacterInitialized = _onCharacterInitialized
            };

            AddUnsafe(new CharacterPm(ctx));
        }
        
        private void InitializeBehaviourFactory()
        {
            var behaviourFactoryCtx = new BehaviourFactory.Ctx
            {
                state = _ctx.state,
                animator = _view.animator,
                rigidbody = _view.rigidbody,
                characterTransform = _view.transform,
                
                onSwipeDirection = _ctx.onSwipeDirection,
                onBehaviourAdded = _ctx.onBehaviourAdded,
                onBehaviourCreated = _ctx.onBehaviourCreated
            };
            
            AddUnsafe(new BehaviourFactory(behaviourFactoryCtx));
        }
    }
}