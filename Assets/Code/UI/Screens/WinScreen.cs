using System.Collections.Concurrent;
using Code.Platforms.Essences;
using Code.UI.Views;
using UnityEngine;

namespace Code.UI.Screens
{
    public class WinScreen
    {
        private readonly Ctx _ctx;
        private WinView _view;

        public struct Ctx
        {
            public WinView viewPrefab;
            public Transform canvas;
        }

        public WinScreen(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Show(ConcurrentDictionary<PlatformType, int> platformsCount)
        {
            _view = Object.Instantiate(_ctx.viewPrefab, _ctx.canvas);
            _view.Init(platformsCount);
        }

        public void Hide()
        {
            Object.Destroy(_view);
        }
    }
}