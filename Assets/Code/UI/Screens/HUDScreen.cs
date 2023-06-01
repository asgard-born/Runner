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
            public string viewPath;
            public Transform canvas;
            public int initLives;
        }

        public HUDScreen(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Show()
        {
            var prefab = Resources.Load<HUDView>(_ctx.viewPath);
            _view = Object.Instantiate(prefab, _ctx.canvas);
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
            if (_view != null)
            {
                Object.Destroy(_view.gameObject);
            }
        }
    }
}