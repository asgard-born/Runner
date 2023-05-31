using Code.Boosters.Essences;
using Code.ObjectsPool;
using Code.Platforms.Helpers;
using Code.Player;
using UnityEngine;

namespace Code.Boosters.Abstract
{
    public abstract class Booster : PoolObject
    {
        public float value;
        public BoosterType boosterType;
        public TriggerZone triggerZone;

        private void Awake()
        {
            triggerZone.OnPlayerEnteredWithFeedback += OnTaken;
        }

        private void OnTaken(PlayerController player)
        {
            player.TakeBonus(boosterType, value);
            ReturnToPool();
        }

        public void Update()
        {
            transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
        }
    }
}