using System.Collections.Generic;
using Code.Platforms.Abstract;
using UnityEngine;

namespace Code.Platforms
{
    public class PlatformsSpawningSystem : MonoBehaviour
    {
        public List<Platform> platforms;

        private void Update()
        {
            SpawnPlatforms();
        }

        private void SpawnPlatforms()
        {
            foreach (var platform in platforms)
            {
                
            }
        }
    }
}