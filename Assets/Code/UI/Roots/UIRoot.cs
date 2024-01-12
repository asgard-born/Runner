using Cysharp.Threading.Tasks;
using Framework;
using UI.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Roots
{
    public class UIRoot : BaseDisposable
    {
        private readonly Ctx _ctx;
        private WinView _winView;

        public struct Ctx
        {
            public AssetReference winViewReference;
            public AssetReference looseViewReference;
            public Transform uiRoot;
        }

        public UIRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitPmsAsync();
        }

        private async UniTask InitPmsAsync()
        {
            var winViewprefab = await LoadAndTrackPrefab<WinView>(_ctx.winViewReference);
            _winView = Object.Instantiate(winViewprefab, _ctx.uiRoot);

            var looseViewprefab = await LoadAndTrackPrefab<WinView>(_ctx.winViewReference);
            _winView = Object.Instantiate(looseViewprefab, _ctx.uiRoot);
        }
    }
}