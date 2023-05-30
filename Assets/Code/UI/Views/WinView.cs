using System.Collections.Concurrent;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.UI.Views
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private PlatformScoreItemUI _platformScoreItem;

        public void Init(ConcurrentDictionary<PlatformType, int> concurrentDictionary)
        {
            foreach (var platformCount in concurrentDictionary)
            {
                var item = Instantiate(_platformScoreItem, _content);
                item.Init(platformCount.Key, platformCount.Value);
            }
        }
    }
}