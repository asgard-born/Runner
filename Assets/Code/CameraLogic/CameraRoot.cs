using Framework;
using UnityEngine;

namespace CameraLogic
{
    public class CameraRoot : BaseDisposable
    {
        public struct Ctx
        {
            public Transform characterTransform;
            public Camera camera;
            public float cameraSmooth;
            public Vector3 positionOffset;
            public Vector3 rotationOffset;
        }
        
        public CameraRoot(Ctx ctx)
        {
            AddUnsafe(new CameraPm(new CameraPm.Ctx
            {
                characterTransform = ctx.characterTransform,
                camera = ctx.camera,
                cameraSmooth = ctx.cameraSmooth,
                positionOffset = ctx.positionOffset,
                rotationOffset = ctx.rotationOffset
            }));
        }
    }
}