using Framework;
using UniRx;
using UnityEngine;

namespace CameraLogic
{
    public class CameraPm : BaseDisposable
    {
        private Ctx _ctx;

        public struct Ctx
        {
            public Transform characterTransform;
            public Camera camera;
            public float cameraSmooth;
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

            var smoothSpeed = _ctx.cameraSmooth * Time.deltaTime;

            Vector3 desiredPosition = _ctx.characterTransform.position + _ctx.characterTransform.rotation * _ctx.positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(_ctx.camera.transform.position, desiredPosition, smoothSpeed);
            _ctx.camera.transform.position = smoothedPosition;

            Quaternion desiredrotation = _ctx.characterTransform.rotation * Quaternion.Euler(_ctx.rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(_ctx.camera.transform.rotation, desiredrotation, smoothSpeed);
            _ctx.camera.transform.rotation = smoothedrotation;
        }
    }
}