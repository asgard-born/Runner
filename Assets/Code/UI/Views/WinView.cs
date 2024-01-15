using Framework.Reactive;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Views
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        
        public struct Ctx
        {
            public ReactiveTrigger onNextLevel;
        }

        public void SetContext(Ctx ctx)
        {
            _nextLevelButton.onClick.AddListener(() => ctx.onNextLevel.Notify());
        }

        private void OnValidate()
        {
            Assert.IsTrue(_nextLevelButton != null, "Next level button cannot be null");
        }
    }
}