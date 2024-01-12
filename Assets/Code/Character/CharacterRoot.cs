using Behaviour;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Character
{
    public class CharacterRoot : BaseDisposable
    {
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveTrigger<float> _onSpeedChangedCallback;
        private CharacterView _view;

        private Ctx _ctx;

        public struct Ctx
        {
            public CharacterStats stats;
            public AssetReference viewReference;
            public Transform spawnPoint;
            public OrientationAxises orientationAxises;
            public ReactiveCommand<Transform> onCharacterInitialized;
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
            public ReactiveCommand<CharacterBehaviourPm> onBehaviourCreated;
        }

        public CharacterRoot(Ctx ctx)
        {
            _ctx = ctx;
            _onCharacterInitialized = ctx.onCharacterInitialized;

            InitializeCharacterAsync();
        }

        private async void InitializeCharacterAsync()
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

                onCharacterInitialized = _onCharacterInitialized,
                onBehaviourCreated = _ctx.onBehaviourCreated
            };

            AddUnsafe(new CharacterPm(ctx));
        }
    }
}