using UnityEngine;

namespace Code.Player
{
    public class PlayerEntity : MonoBehaviour
    {
        private bool _canRun;
        private Rigidbody _rigidbody;
        private Ctx _ctx;

        public struct Ctx
        {
            public SessionStats sessionStats;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_canRun)
            {
                Run();
            }
        }

        public void StartRun()
        {
            _canRun = true;
        }

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void RotateLeft()
        {
            transform.Rotate(Vector3.up * -90);
        }

        public void RotateRight()
        {
            transform.Rotate(Vector3.up * 90);
        }

        private void Run()
        {
            var speed = _ctx.sessionStats.currentSpeed;
            _rigidbody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }

        private void Jump()
        {
            
        }
    }
}