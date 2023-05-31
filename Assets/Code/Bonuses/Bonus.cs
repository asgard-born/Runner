using Code.Bonuses.Essences;
using Code.ObjectsPool;
using Code.Platforms.Helpers;
using Code.Player;
using UnityEngine;

namespace Code.Bonuses
{
    public class Bonus : PoolObject
    {
        public int value { get; private set; }
        public BonusType bonusType;
        public TriggerZone triggerZone;

        private void Awake()
        {
            triggerZone.OnPlayerEnteredWithFeedback += OnTaken;
        }

        private void OnTaken(PlayerController player)
        {
            player.TakeBonus(bonusType, value);
            ReturnToPool();
        }

        public void Update()
        {
            transform.Rotate(Vector3.up * 10 * Time.deltaTime);
        }
    }
}