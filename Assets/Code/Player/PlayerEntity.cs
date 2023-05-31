using Code.Session;
using UnityEngine;

namespace Code.Player
{
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimationSystem _animationSystem;

        private enum State
        {
            None,
            Running,
            Jumping,
            Falling,
            FallingOut,
        }

        private Collider _collider;
        private bool _canRun;
        private Rigidbody _rigidbody;
        private Ctx _ctx;
        private float distToGround;
        private int _currentJumpingCount;

        private State _currentState;

        public struct Ctx
        {
            public SessionListener sessionListener;
        }

        public void Init(Ctx ctx)
        {
            _collider = GetComponent<Collider>();
            _ctx = ctx;
            distToGround = _collider.bounds.extents.y;
        }

        public void TryJump()
        {
            if ((_currentJumpingCount == 0 && !IsGrounded()) || _currentJumpingCount >= _ctx.sessionListener.maxJumpingTimes)
            {
                return;
            }

            _currentJumpingCount += 1;
            _rigidbody.AddForce(Vector3.up * _ctx.sessionListener.jumpForce, ForceMode.VelocityChange);
            _animationSystem.PlayJump();

            _currentState = State.Jumping;
        }

        public void StartRun()
        {
            _canRun = true;
            _animationSystem.PlayRun();
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
            CheckState();
        }

        private void CheckState()
        {
            switch (_currentState)
            {
                case State.Running:
                    _animationSystem.PlayRun();

                    if (IsFallingOut())
                    {
                        _currentState = State.FallingOut;
                    }

                    break;

                case State.Jumping:
                    if (IsFalling())
                    {
                        _currentState = State.Falling;
                    }

                    break;

                case State.Falling:
                    if (IsGrounded())
                    {
                        _currentJumpingCount = 0;
                        _currentState = State.Running;
                    }
                    else if (IsFallingOut())
                    {
                        _currentState = State.FallingOut;
                    }

                    break;

                case State.FallingOut:
                    _animationSystem.PlayFalling();

                    break;
            }
        }

        private void FixedUpdate()
        {
            if (_canRun)
            {
                Run();
            }
        }

        private void Run()
        {
            var speed = _ctx.sessionListener.currentSpeed;
            _rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }

        private bool IsGrounded()
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, .2f))
            {
                if (_rigidbody.velocity.y <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsFalling()
        {
            return _rigidbody.velocity.y < 0;
        }

        private bool IsFallingOut()
        {
            return IsFalling() && _rigidbody.position.y < _ctx.sessionListener.valueYToFallOut;
        }

        public void Stop(bool withAnimation = false)
        {
            _canRun = false;

            if (withAnimation)
            {
                _animationSystem.PlayIdle();
            }
        }
    }
}