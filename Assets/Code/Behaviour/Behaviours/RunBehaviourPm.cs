using DG.Tweening;
using Shared;
using UnityEngine;

namespace Behaviour.Behaviours
{
    public class RunBehaviourPm : CharacterBehaviourPm
    {
        private Sequence _sideMovingSequence;

        private static readonly int _idle = Animator.StringToHash("Idle");
        private static readonly int _running = Animator.StringToHash("Running");
        private static readonly int _jumping = Animator.StringToHash("Jumping");
        private static readonly int _falling = Animator.StringToHash("Falling");
        private static readonly int _damage = Animator.StringToHash("Damage");
        private float _sideMoveDurationSec = .5f;

        public RunBehaviourPm(Ctx ctx) : base(ctx)
        {
            _isMoving = true;
        }

        protected override void DoBehave()
        {
            if (_isMoving)
            {
                OnMove();
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

        protected override void OnSwipe(SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.Left:
                case SwipeDirection.Right:
                    SideMove(swipeDirection);

                    break;

                case SwipeDirection.Up:
                    TryJump(_ctx.state.jumpForce);

                    break;

                case SwipeDirection.Down:
                    break;
            }
        }

        private void SideMove(SwipeDirection swipeDirection)
        {
            var transform = _ctx.characterTransform;
            Vector3 roadPosition;

            if (!CanMoveToDirection(swipeDirection)) return;

            // _sideMovingSequence = DOTween
            //     .Sequence(_ctx.characterTransform.DOMoveX( roadPosition.x, _sideMoveDurationSec));
        }

        private bool CanMoveToDirection(SwipeDirection swipeDirection)
        {
            var currentLine = _ctx.state.currentRoadline;

            if (swipeDirection == SwipeDirection.Left && currentLine.Previous != null)
            {
                return true;
            }

            if (swipeDirection == SwipeDirection.Right && currentLine.Next != null)
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

            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            _ctx.animator.SetTrigger(_jumping);

            _currentAction = CharacterAction.Jumping;
        }

        private bool IsFalling()
        {
            return _ctx.rigidbody.velocity.y < 0;
        }

        private void OnMove()
        {
            var speed = _ctx.state.velocity;

            if (speed <= 0) return;

            _ctx.rigidbody.MovePosition(_ctx.characterTransform.position + _ctx.characterTransform.forward * speed * Time.fixedDeltaTime);
            _ctx.animator.SetBool(_running, true);
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