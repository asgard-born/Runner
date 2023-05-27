using System;
using Code.Platforms.Abstract;

namespace Code.Configs.Essences
{
    [Serializable]
    public struct PlatformChance
    {
        public Platform platform;
        public float chance;
    }
}