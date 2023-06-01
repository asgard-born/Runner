using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Code.Platforms.Essences;
using Code.UI.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.UI.Screens
{
    public class WinScreen
    {
        private readonly Ctx _ctx;
        private WinView _view;
        private Action _onNextLevelClicked;

        public struct Ctx
        {
            public HashSet<PlatformType> blocksToCalculateOnFinish;
            public string viewPath;
            public Transform canvas;
            public Action nextLevelCallback;
        }

        public WinScreen(Ctx ctx)
        {
            _ctx = ctx;
            _onNextLevelClicked = _ctx.nextLevelCallback;
        }

        public void Show(ConcurrentDictionary<PlatformType, int> platformsCount)
        {
            var prefab = Resources.Load<WinView>(_ctx.viewPath);
            _view = Object.Instantiate(prefab, _ctx.canvas);
            
            var blocksToShow = platformsCount.Where(x => _ctx.blocksToCalculateOnFinish.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            _view.Init(blocksToShow);
            _view.nextLevelButton.onClick.AddListener(() => _onNextLevelClicked?.Invoke());
        }

        public void Hide()
        {
            if (_view != null)
            {
                Object.Destroy(_view.gameObject);
            }
        }
    }
}