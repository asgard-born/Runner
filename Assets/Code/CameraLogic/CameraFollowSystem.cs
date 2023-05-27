using UnityEngine;

namespace Code.CameraLogic
{
    public class CameraFollowSystem
    {
        private readonly Ctx _ctx;

        private readonly Vector3 positionOffset = new Vector3(0, 2, -6);
        private readonly Vector3 rotationOffset = Vector3.right * 11.236f;

        public struct Ctx
        {
            public Transform cameraTransform;
            public Transform player;
            public float cameraSmooth;
        }

        public CameraFollowSystem(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Follow()
        {
            var smoothSpeed = _ctx.cameraSmooth * Time.deltaTime;

            Vector3 desiredPosition = _ctx.player.position + _ctx.player.rotation * positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(_ctx.cameraTransform.position, desiredPosition, smoothSpeed);
            _ctx.cameraTransform.position = smoothedPosition;

            Quaternion desiredrotation = _ctx.player.rotation * Quaternion.Euler(rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(_ctx.cameraTransform.rotation, desiredrotation, smoothSpeed);
            _ctx.cameraTransform.rotation = smoothedrotation;
        }
    }
}