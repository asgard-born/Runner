using Framework.Reactive;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Views
{
    /// <summary>
    /// Дисплей выигрыша
    /// </summary>
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TextMeshProUGUI _coins;
        
        public struct Ctx
        {
            public ReactiveTrigger onNextLevel;
            public int coinsCount;
        }

        public void SetContext(Ctx ctx)
        {
            _nextLevelButton.onClick.AddListener(() => ctx.onNextLevel.Notify());
            _coins.text = ctx.coinsCount.ToString();
        }

        private void OnValidate()
        {
            Assert.IsTrue(_nextLevelButton != null, "Next level button cannot be null");
            Assert.IsTrue(_coins != null, "Coins button cannot be null");
        }
    }
}