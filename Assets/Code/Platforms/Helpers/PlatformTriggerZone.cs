using System;
using Code.Player;
using UnityEngine;

namespace Code.Platforms.Helpers
{
    public class PlatformTriggerZone : MonoBehaviour
    {
        public Action OnPlayerEntered;
        
        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerEntity>();

            if (player != null)
            {
                OnPlayerEntered?.Invoke();
            }
        }
    }
}