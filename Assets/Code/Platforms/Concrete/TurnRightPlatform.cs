﻿using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class TurnRightPlatform : TurnPlatform
    {
        private void Start()
        {
            behaviourType = typeof(TurnRightInteractingBehaviour);
        }
    }
}