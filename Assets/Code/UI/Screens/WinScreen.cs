using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            public HashSet<PlatformType> blocksToCalculateOnFinish; 
            public WinView viewPrefab;
            public Transform canvas;
        }

        public WinScreen(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Show(ConcurrentDictionary<PlatformType, int> platformsCount)
        {
            var blocksToShow = platformsCount.Where(x => _ctx.blocksToCalculateOnFinish.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

            _view = Object.Instantiate(_ctx.viewPrefab, _ctx.canvas);
            _view.Init(blocksToShow);
        }

        public void Hide()
        {
            Object.Destroy(_view);
        }
    }
}