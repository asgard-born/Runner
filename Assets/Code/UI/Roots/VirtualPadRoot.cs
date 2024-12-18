﻿using Framework;
using Shared;
using UI.Pms;
using UI.Views;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Roots
{
    public class VirtualPadRoot : BaseDisposable
    {
        /// <summary>
        /// Входная точка или доменная область панели ввода. Создает представление, presenter'a
        /// и организовывает слабое сцепление между ними с помощью реактивных команд 
        /// </summary>
        private ReactiveCommand<(Vector2, Vector2)> _onSwipeRaw;
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public RectTransform uiTransform;
            public AssetReference virtualPadViewReference;
            public ReactiveCommand<Direction> onSwipeDirection;
        }

        public VirtualPadRoot(Ctx ctx)
        {
            _ctx = ctx;
            _onSwipeRaw = AddUnsafe(new ReactiveCommand<(Vector2, Vector2)>());

            InitializePm();
            InitializeViewAsync();
        }

        private void InitializePm()
        {
            var ctx = new VirtualPadPm.Ctx
            {
                onSwipeRaw = _onSwipeRaw,
                onSwipeDirection = _ctx.onSwipeDirection
            };

            AddUnsafe(new VirtualPadPm(ctx));
        }

        private async void InitializeViewAsync()
        {
            var ctx = new VirtualPadView.Ctx
            {
                onSwipeRaw = _onSwipeRaw
            };

            var prefab = await LoadAndTrackPrefab<VirtualPadView>(_ctx.virtualPadViewReference);

            VirtualPadView virtualPadView = Object.Instantiate(prefab, _ctx.uiTransform);
            virtualPadView.SetContext(ctx);
        }
    }
}