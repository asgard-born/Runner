using Shared;
using UnityEngine;

namespace Behaviour.Behaviours
{
    /// <summary>
    /// Поведение игрока типа Бег. Поведению делегируется работа с вью и обработка пользовательского свайпа в разные стороны 
    /// </summary>
    public class RunBehaviourPm : CharacterBehaviourPm
    {
        private static readonly int _idle = Animator.StringToHash("Idle");
        private static readonly int _running = Animator.StringToHash("Running");
        private static readonly int _jumping = Animator.StringToHash("Jumping");
        private static readonly int _falling = Animator.StringToHash("Falling");
        private static readonly int _damage = Animator.StringToHash("Damage");

        public RunBehaviourPm(Ctx ctx) : base(ctx)
        {
            _isMoving = true;

            InitializeState();
        }

        private void InitializeState()
        {
            _ctx.state.nextPosition = _ctx.characterTransform.position;
            _ctx.state.velocity = _ctx.configs.speed;
            _ctx.state.jumpForce = _ctx.configs.jumpForce;
            _ctx.state.sideSpeed = _ctx.configs.sideSpeed;
        }

        protected override void Behave()
        {
            if (_isMoving)
            {
                Move();
            }

            switch (_currentAction)
            {
                case CharacterAction.Jumping:
                    if (IsFalling())
                    {
                        _currentAction = CharacterAction.Falling;
                    }

                    break;

                case CharacterAction.Falling:
                    OnFalling();

                    break;
            }
        }

        protected override void OnSwipeDirection(SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.Left:
                case SwipeDirection.Right:
                    OnChangeSide(swipeDirection);

                    break;

                case SwipeDirection.Up:
                    TryJump(_ctx.state.jumpForce);

                    break;

                case SwipeDirection.Down:
                    break;
            }
        }

        private void Move()
        {
            var speed = _ctx.state.velocity;

            if (speed <= 0) return;

            var transform = _ctx.characterTransform;
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;

            var nextPosition = transform.forward * speed * Time.fixedDeltaTime;
            var localDistance = _ctx.characterTransform.InverseTransformPoint(roalinePosition);

            var direction = localDistance.x > 0 ? transform.right : -transform.right;

            if (Mathf.Abs(localDistance.x) > _ctx.toleranceSideDistance)
            {
                nextPosition += direction * _ctx.state.sideSpeed * Time.fixedDeltaTime;
            }

            _ctx.state.nextPosition += nextPosition;

            _ctx.rigidbody.MovePosition(_ctx.state.nextPosition);
            _ctx.animator.SetBool(_running, true);
        }

        private void OnChangeSide(SwipeDirection swipeDirection)
        {
            if (!CanMoveToDirection(swipeDirection)) return;

            var currentLine = _ctx.state.currentRoadline;
            var nextRoadline = swipeDirection == SwipeDirection.Left ? currentLine.Previous : currentLine.Next;

            _ctx.state.currentRoadline = nextRoadline;
        }

        private bool CanMoveToDirection(SwipeDirection swipeDirection)
        {
            if (swipeDirection == SwipeDirection.Left && _ctx.state.currentRoadline.Previous != null)
            {
                return true;
            }

            if (swipeDirection == SwipeDirection.Right && _ctx.state.currentRoadline.Next != null)
            {
                return true;
            }

            return false;
        }

        private void TryJump(float jumpForce)
        {
            if (!IsGrounded())
            {
                return;
            }

            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _ctx.animator.SetTrigger(_jumping);

            _currentAction = CharacterAction.Jumping;
        }

        private bool IsFalling()
        {
            return _ctx.rigidbody.velocity.y < 0;
        }

        private void Stop()
        {
            _ctx.animator.SetBool(_idle, true);

            _ctx.rigidbody.isKinematic = true;
        }

        private bool IsGrounded()
        {
            if (Physics.Raycast(_ctx.characterTransform.position + new Vector3(0, 0.2f, 0), Vector3.down, .2f))
            {
                if (_ctx.rigidbody.velocity.y <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnFalling()
        {
            if (IsGrounded())
            {
            }
            else
            {
                //OnFalling --> View (Rx)
            }
        }
    }
}