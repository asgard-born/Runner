using UnityEngine;

namespace Code.CameraLogic
{
    public class CameraFollowSystem : MonoBehaviour
    {
        private readonly Vector3 positionOffset = new Vector3(0, 4, -6);
        private readonly Vector3 rotationOffset = Vector3.right * 15;
        
        private Ctx _ctx;

        public struct Ctx
        {
            public Transform player;
            public float cameraSmooth;
        }

        public void Initialize(Ctx ctx)
        {
            _ctx = ctx;
        }

        private void LateUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            var smoothSpeed = _ctx.cameraSmooth * Time.deltaTime;

            Vector3 desiredPosition = _ctx.player.position + _ctx.player.rotation * positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            Quaternion desiredrotation = _ctx.player.rotation * Quaternion.Euler(rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
            transform.rotation = smoothedrotation;
        }
    }
}