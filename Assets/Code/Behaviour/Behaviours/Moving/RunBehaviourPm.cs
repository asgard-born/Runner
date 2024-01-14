using Behaviour.Behaviours.Abstract;
using Shared;
using UnityEngine;

namespace Behaviour.Behaviours.Moving
{
    /// <summary>
    /// Поведение игрока типа Бег. Поведению делегируется работа с компонентами представления
    /// и обработка пользовательского свайпа в разные стороны
    /// </summary>
    public class RunBehaviourPm : MovingBehaviourPm
    {
        public RunBehaviourPm(Ctx ctx) : base(ctx)
        {
            InitializeState();

            _isMoving = true;
            _currentAction = CharacterAction.Running;
        }

        protected override void InitializeState()
        {
            _ctx.state.speed = _ctx.configs.speed;
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
                case CharacterAction.Running:
                    _ctx.animator.SetTrigger(_running);

                    break;

                case CharacterAction.Jumping:
                    if (!IsGrounded())
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
        
        protected override void OnTimesOver()
        {
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        private void TryJump(float jumpForce)
        {
            if (_currentAction != CharacterAction.Running) return;

            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _ctx.animator.SetTrigger(_jumping);

            _currentAction = CharacterAction.Jumping;
        }

        private void Stop()
        {
            _ctx.animator.SetTrigger(_idle);

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
            var isGrounded = IsGrounded();

            _ctx.animator.SetBool(_falling, !isGrounded);

            if (isGrounded)
            {
                _currentAction = CharacterAction.Running;
            }
        }
    }
}