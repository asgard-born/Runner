using UnityEngine;

namespace Code.CameraLogic
{
    public class CameraFollowSystem
    {
        private readonly Ctx _ctx;

        private Vector3 positionOffset = new Vector3(0, 2, -6);
        private Vector3 rotationOffset = Vector3.right * 11.236f;

        public struct Ctx
        {
            public Transform transform;
            public Transform player;
        }

        public CameraFollowSystem(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Follow()
        {
            var smoothSpeed = 30f * Time.deltaTime;

            Vector3 desiredPosition = _ctx.player.position + _ctx.player.rotation * positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(_ctx.transform.position, desiredPosition, smoothSpeed);
            _ctx.transform.position = smoothedPosition;

            Quaternion desiredrotation = _ctx.player.rotation * Quaternion.Euler(rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(_ctx.transform.rotation, desiredrotation, smoothSpeed);
            _ctx.transform.rotation = smoothedrotation;
        }

        public void Follow(int x = 0)
        {
            // Quaternion rotation = Quaternion.Euler(new Vector3(11.236f, 0, 0));
            // _ctx.transform.rotation = Quaternion.Lerp(_ctx.transform.rotation, _ctx.player.rotation * rotation, 300f * Time.deltaTime);
            // _ctx.transform.position = _ctx.player.position - Vector3.forward * _distance;

            // Vector3 offset = new Vector3(0, 1.8f, -_distance);
            // Quaternion rotation = Quaternion.Euler(11.236f, _ctx.player.rotation.y, -_distance);
            //
            // _ctx.transform.position = _ctx.player.position + rotation * offset;
            // _ctx.transform.LookAt(_ctx.player.position);
        }
    }
}