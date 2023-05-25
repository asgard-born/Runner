using Code.Configs;
using Code.Platforms.Abstract;
using UnityEngine;

namespace Code.Platforms
{
    public class PlatformsFactory : MonoBehaviour
    {
        private Ctx _ctx;

        public struct Ctx
        {
            public GameConfigs gameConfigs;
            public Platform currentPlatform;
        }

        private PlatformsFactory(Ctx ctx)
        {
            _ctx = ctx;
        }

        private void CreatePlatform()
        {
            // calculate according settings
            
            // var platformType = Pla
            //
            // var newPlatform = Object.Instantiate()
        }
    }
}