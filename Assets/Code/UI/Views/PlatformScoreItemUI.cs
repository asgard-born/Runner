using System;
using System.Linq;
using Code.Platforms.Essences;
using TMPro;
using UnityEngine;

namespace Code.UI.Views
{
    public class PlatformScoreItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _platformName;
        [SerializeField] private TextMeshProUGUI _amount;

        public void Init(PlatformType type, int amount)
        {
            var val = type.ToString();
            val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            _platformName.text = val;
            _amount.text = amount.ToString();
        }
    }
}