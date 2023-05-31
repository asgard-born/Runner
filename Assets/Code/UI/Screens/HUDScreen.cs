using Code.UI.Views;
using UnityEngine;

namespace Code.UI.Screens
{
    public class HUDScreen
    {
        private readonly Ctx _ctx;
        private HUDView _view;

        public struct Ctx
        {
            public HUDView viewPrefab;
            public Transform canvas;
            public int initLives;
        }

        public HUDScreen(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Show()
        {
            _view = Object.Instantiate(_ctx.viewPrefab, _ctx.canvas);
            _view.Show(_ctx.initLives);
        }

        public void UpdateLives(int value)
        {
            if (value > 0)
            {
                _view.AddLives(value);
            }
            else
            {
                _view.RemoveLives(value);
            }
        }

        public void Hide()
        {
            Object.Destroy(_view);
        }
    }
}