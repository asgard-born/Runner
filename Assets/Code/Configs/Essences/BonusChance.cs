using System;
using Code.Bonuses.Essences;

namespace Code.Configs.Essences
{
    [Serializable]
    public struct BonusChance
    {
        public BonusType bonusType;
        public float chance;
    }
}