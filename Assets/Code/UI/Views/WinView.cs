using System.Collections.Generic;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.UI.Views
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private PlatformScoreItemUI _platformScoreItem;

        public void Init(Dictionary<PlatformType, int> blocksToShow)
        {
            foreach (var platformCount in blocksToShow)
            {
                var item = Instantiate(_platformScoreItem, _content);
                item.Init(platformCount.Key, platformCount.Value);
            }
        }
    }
}