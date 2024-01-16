using Framework.Reactive;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Views
{
    public class LooseView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _coins;

        public struct Ctx
        {
            public ReactiveTrigger onRestartLevel;
            public int coinsCount;
        }

        public void SetContext(Ctx ctx)
        {
            _restartButton.onClick.AddListener(() => ctx.onRestartLevel.Notify());
            _coins.text = ctx.coinsCount.ToString();
        }

        private void OnValidate()
        {
            Assert.IsTrue(_restartButton != null, "Restart button cannot be null");
        }
    }
}