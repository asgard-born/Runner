using UnityEngine;

namespace Code.Player
{
    public class PlayerEntity : MonoBehaviour
    {
        private Collider _collider;
        private bool _canRun;
        private Rigidbody _rigidbody;
        private Ctx _ctx;
        private float distToGround;

        public struct Ctx
        {
            public SessionStats sessionStats;
        }

        public void Init(Ctx ctx)
        {
            _collider = GetComponent<Collider>();
            _ctx = ctx;
            distToGround = _collider.bounds.extents.y;
        }

        public void TryJump()
        {
            if (!IsGrounded()) return;
            
            _rigidbody.AddForce(Vector3.up * _ctx.sessionStats.jumpForce, ForceMode.VelocityChange);
        }

        public void StartRun()
        {
            _canRun = true;
        }

        public void RotateLeft()
        {
            transform.Rotate(Vector3.up * -90);
        }

        public void RotateRight()
        {
            transform.Rotate(Vector3.up * 90);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            IsGrounded();

            if (_canRun)
            {
                Run();
            }
        }

        private void Run()
        {
            var speed = _ctx.sessionStats.currentSpeed;
            _rigidbody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, distToGround + .1f);
        }

        private bool IsFalling()
        {
            return (!IsGrounded() && _rigidbody.velocity.y < 0);
        }
    }
}