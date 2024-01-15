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
        private WinView _winView;
        private LooseView _looseView;
        private HUDView _hudView;

        public struct Ctx
        {
            public Transform uiRoot;
            public RectTransform uiTransform;
            public ResourcesConfigs resourcesConfigs;

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
                
            };
            
            var hudViewPrefab = await LoadAndTrackPrefab<HUDView>(_ctx.resourcesConfigs.hudViewReference);
            _hudView = Object.Instantiate(hudViewPrefab, _ctx.uiRoot);
        }

        private async void InitWinViewAsync()
        {
            var winViewprefab = await LoadAndTrackPrefab<WinView>(_ctx.resourcesConfigs.winViewReference);
            _winView = Object.Instantiate(winViewprefab, _ctx.uiRoot);
        }

        private async void InitWinLooseAsync()
        {
            var looseViewprefab = await LoadAndTrackPrefab<LooseView>(_ctx.resourcesConfigs.looseViewReference);
            _looseView = Object.Instantiate(looseViewprefab, _ctx.uiRoot);
        }
    }
}