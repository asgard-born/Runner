using System;
using System.Collections.Generic;
using Code.Platforms.Abstract;
using UnityEngine;

namespace Code.Platforms
{
    public class PlatformsSystem : MonoBehaviour
    {
        public List<Platform> platforms;

        private void Update()
        {
            MovePlatforms();
        }

        private void MovePlatforms()
        {
            foreach (var platform in platforms)
            {
                
            }
        }
    }
}