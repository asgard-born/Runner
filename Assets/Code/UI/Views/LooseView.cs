using Framework.Reactive;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Views
{
    public class LooseView : MonoBehaviour
    {
        [SerializeField] private  Button _restartButton;

        public struct Ctx
        {
            public ReactiveTrigger onRestartLevel;
        }

        public void SetContext(Ctx ctx)
        {
            _restartButton.onClick.AddListener(() => ctx.onRestartLevel.Notify());
        }
        private void OnValidate()
        {
            Assert.IsTrue(_restartButton != null, "Restart button cannot be null");
        }
    }
}