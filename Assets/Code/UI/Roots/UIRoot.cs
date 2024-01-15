using Configs;
using Framework;
using Shared;
using UI.Views;
using UniRx;
using UnityEngine;

namespace UI.Roots
{
    public class UIRoot : BaseDisposable
    {
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public RectTransform uiTransform;
            public ResourcesConfigs resourcesConfigs;

            public ReactiveProperty<int> lives;
            public ReactiveProperty<int> coins;

            public ReactiveCommand<Direction> onSwipeDirection;
        }

        public UIRoot(Ctx ctx)
        {
            _ctx = ctx;

            var virtualPadEntityCtx = new VirtualPadRoot.Ctx
            {
                uiTransform = ctx.uiTransform,
                virtualPadViewReference = ctx.resourcesConfigs.virtualPadReference,
                onSwipeDirection = _ctx.onSwipeDirection
            };

            AddUnsafe(new VirtualPadRoot(virtualPadEntityCtx));

            InitHUDViewAsync();
        }

        private async void InitHUDViewAsync()
        {
            var ctx = new HUDView.Ctx
            {
                lives = _ctx.lives,
                coins = _ctx.coins
            };

            var hudViewPrefab = await LoadAndTrackPrefab<HUDView>(_ctx.resourcesConfigs.hudViewReference);
            var hudView = Object.Instantiate(hudViewPrefab, _ctx.uiTransform);
            
            hudView.SetContext(ctx);
        }

        private async void InitWinViewAsync()
        {
            var winViewprefab = await LoadAndTrackPrefab<WinView>(_ctx.resourcesConfigs.winViewReference);
            Object.Instantiate(winViewprefab, _ctx.uiTransform);
        }

        private async void InitWinLooseAsync()
        {
            var looseViewprefab = await LoadAndTrackPrefab<LooseView>(_ctx.resourcesConfigs.looseViewReference);
            Object.Instantiate(looseViewprefab, _ctx.uiTransform);
        }
    }
}