﻿using Configs;
using Framework;
using Framework.Reactive;
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

            public ReactiveTrigger onGameWin;
            public ReactiveTrigger onGameOver;
            public ReactiveTrigger onRestartLevel;
            public ReactiveTrigger onNextLevel;
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

            AddUnsafe(_ctx.onGameWin.Subscribe(InitWinViewAsync));
            AddUnsafe(_ctx.onGameOver.Subscribe(InitLooseViewAsync));
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
            var winView = Object.Instantiate(winViewprefab, _ctx.uiTransform);

            var ctx = new WinView.Ctx
            {
                onNextLevel = _ctx.onNextLevel,
                coinsCount = _ctx.coins.Value
            };

            winView.SetContext(ctx);
        }

        private async void InitLooseViewAsync()
        {
            var looseViewprefab = await LoadAndTrackPrefab<LooseView>(_ctx.resourcesConfigs.looseViewReference);
            var looseView = Object.Instantiate(looseViewprefab, _ctx.uiTransform);
            
            var ctx = new LooseView.Ctx
            {
                onRestartLevel = _ctx.onRestartLevel,
                coinsCount = _ctx.coins.Value
            };

            looseView.SetContext(ctx);
        }
    }
}