using System;
using Code.UI.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.UI.Screens
{
    public class LooseScreen
    {
        private readonly Ctx _ctx;
        private LooseView _view;
        private Action _onRestartClicked;
        private Action _onContinueClicked;

        public struct Ctx
        {
            public string viewPath;
            public Transform canvas;
            public Action restartCallback;
            public Action continueCallback;
        }

        public LooseScreen(Ctx ctx)
        {
            _ctx = ctx;
            _onRestartClicked = _ctx.restartCallback;
            _onContinueClicked = _ctx.continueCallback;
        }

        public void Show()
        {
            var prefab = Resources.Load<LooseView>(_ctx.viewPath);
            _view = Object.Instantiate(prefab, _ctx.canvas);
            _view.restartButton.onClick.AddListener(() => _onRestartClicked?.Invoke());
            _view.continueButton.onClick.AddListener(() => _onContinueClicked?.Invoke());
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