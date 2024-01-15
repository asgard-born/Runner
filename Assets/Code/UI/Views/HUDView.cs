using TMPro;
using UniRx;
using UnityEngine;

namespace UI.Views
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lives;
        [SerializeField] private TextMeshProUGUI _coins;

        public struct Ctx
        {
            public ReactiveProperty<int> lives;
            public ReactiveProperty<int> coins;
        }

        public void SetContext(Ctx ctx)
        {
            ctx.lives.Subscribe(OnLivesChanges).AddTo(this);
            ctx.coins.Subscribe(OnLivesChanges).AddTo(this);
        }

        private void OnLivesChanges(int newValue)
        {
            _lives.text = newValue.ToString();
        }

        private void OnCoinsChanges(int newValue)
        {
            _coins.text = newValue.ToString();
        }
    }
}