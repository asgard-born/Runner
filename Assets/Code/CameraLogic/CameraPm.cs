using Framework;
using UniRx;
using UnityEngine;

namespace CameraLogic
{
    /// <summary>
    /// Управляет логикой работы камеры.
    /// </summary>
    public class CameraPm : BaseDisposable
    {
        private Ctx _ctx;

        public struct Ctx
        {
            public Transform characterTransform;
            public Transform cameraTransform;
            public float speed;
            public Vector3 positionOffset;
        }

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddUnsafe(Observable.EveryFixedUpdate().Subscribe(Follow));
        }

        private void Follow(long _)
        {
            if (_ctx.characterTransform == null) return;

            float smoothSpeed = _ctx.speed * Time.deltaTime;

            Vector3 desiredPosition = _ctx.characterTransform.position + _ctx.characterTransform.rotation * _ctx.positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(_ctx.cameraTransform.position, desiredPosition, smoothSpeed);
            
            _ctx.cameraTransform.position = smoothedPosition;
        }
    }
}