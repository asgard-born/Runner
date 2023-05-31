using System;
using Code.Boosters.Abstract;

namespace Code.Configs.Essences
{
    [Serializable]
    public struct BonusChance
    {
        public Booster _booster;
        public float chance;
    }
}