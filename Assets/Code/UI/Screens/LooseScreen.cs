using Code.UI.Views;
using UnityEngine;

namespace Code.UI.Screens
{
    public class LooseScreen
    {
        private readonly Ctx _ctx;
        private LooseView _view;

        public struct Ctx
        {
            public LooseView viewPrefab;
            public Transform canvas;
        }

        public LooseScreen(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Show()
        {
            _view = Object.Instantiate(_ctx.viewPrefab, _ctx.canvas);
        }

        public void Hide()
        {
            Object.Destroy(_view);
        }
    }
}