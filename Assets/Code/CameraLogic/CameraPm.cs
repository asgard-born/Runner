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
            public float smoothFactor;
            public Vector3 positionOffset;
            public Vector3 rotationOffset;
        }

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
            
            Observable.EveryLateUpdate().Subscribe(Follow);
        }

        private void Follow(long _)
        {
            if (_ctx.characterTransform == null) return;

            float smoothSpeed = _ctx.smoothFactor * Time.deltaTime;

            Vector3 desiredPosition = _ctx.characterTransform.position + _ctx.characterTransform.rotation * _ctx.positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(_ctx.cameraTransform.position, desiredPosition, smoothSpeed);
            
            _ctx.cameraTransform.position = smoothedPosition;

            Quaternion desiredrotation = _ctx.characterTransform.rotation * Quaternion.Euler(_ctx.rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(_ctx.cameraTransform.rotation, desiredrotation, smoothSpeed);
            
            _ctx.cameraTransform.rotation = smoothedrotation;
        }
    }
}