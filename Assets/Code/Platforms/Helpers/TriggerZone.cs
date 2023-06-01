using System;
using Code.Player;
using UnityEngine;

namespace Code.Platforms.Helpers
{
    public class TriggerZone : MonoBehaviour
    {
        public Action OnPlayerEntered;
        public Action<PlayerController> OnPlayerEnteredWithFeedback;
        [SerializeField] private Collider _trigger;

        public void SetEnable(bool isEnable)
        {
            _trigger.enabled = isEnable;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                OnPlayerEntered?.Invoke();
                OnPlayerEnteredWithFeedback?.Invoke(player);
            }
        }
    }
}