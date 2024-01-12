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
                    OnJumping();

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
                    
                    break;

                case SwipeDirection.Right:
                    break;

                case SwipeDirection.Up:
                    TryJump(_ctx.stats.jumpForce);

                    break;

                case SwipeDirection.Down:
                    break;
            }
        }

        private void MovingLeftProcess()
        {
            // _sideMovingSequence = DOTween
            //     .Sequence(_ctx.rigidbody.DOMove(_ctx.rigidbody.position));


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
            Run(_ctx.stats.speed);
        }

        private void OnJumping()
        {
            if (IsFalling())
            {
                _currentAction = CharacterAction.Falling;
            }
        }

        private void Stop()
        {
            _ctx.animator.SetBool(_idle, true);

            _ctx.rigidbody.isKinematic = true;
        }

        private void Run(float speed)
        {
            if (speed <= 0) return;

            _ctx.rigidbody.MovePosition(_ctx.characterTransform.position + _ctx.characterTransform.forward * speed * Time.fixedDeltaTime);
            _ctx.animator.SetBool(_running, true);
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