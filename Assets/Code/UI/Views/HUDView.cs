using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI.Views
{
    /// <summary>
    /// Дисплей, отвечающий только за отображения основной информацию об игровой сессии:
    /// количество жизней и монет  
    /// </summary>
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
            ctx.coins.Subscribe(OnCoinsChanges).AddTo(this);
        }

        private void OnLivesChanges(int newValue)
        {
            _lives.text = newValue.ToString();
        }

        private void OnCoinsChanges(int newValue)
        {
            _coins.text = newValue.ToString();
        }
        
        private void OnValidate()
        {
            Assert.IsTrue(_lives != null, "Lives cannot be null");
            Assert.IsTrue(_coins != null, "Coins button cannot be null");
        }
    }
}