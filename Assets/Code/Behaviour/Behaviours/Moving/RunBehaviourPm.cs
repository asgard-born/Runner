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
            StartMoving();
        }

        private void StartMoving()
        {
            _currentAction = CharacterAction.Moving;
            _ctx.animator.SetTrigger(_running);
        }

        protected override void InitializeState()
        {
            _ctx.state.speed = _ctx.configs.speed;
            _ctx.state.jumpForce = _ctx.configs.jumpForce;
        }

        protected override void Behave()
        {
            switch (_currentAction)
            {
                case CharacterAction.Idle:
                    _ctx.animator.SetBool(_idle, true);

                    break;

                case CharacterAction.Moving:
                    Move();

                    break;

                case CharacterAction.Jumping:
                    Move();

                    if (!IsGrounded())
                    {
                        _currentAction = CharacterAction.Falling;
                    }

                    break;

                case CharacterAction.Falling:
                    Move();
                    OnFalling();

                    break;
            }
        }

        protected override void OnSwipeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    OnChangeSide(direction);

                    break;

                case Direction.Up:
                    TryJump(_ctx.state.jumpForce);

                    break;

                case Direction.Down:
                    break;
            }
        }

        protected override void OnTimesOver()
        {
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        private void TryJump(float jumpForce)
        {
            if (_currentAction != CharacterAction.Moving) return;

            _ctx.rigidbody.useGravity = true;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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
            return Physics.Raycast(
                _ctx.transform.position + Vector3.up * _ctx.toleranceDistance.y,
                Vector3.down,
                _ctx.toleranceDistance.y,
                _ctx.landingMask);
        }

        private void OnFalling()
        {
            var isGrounded = IsGrounded();

            _ctx.animator.SetBool(_falling, !isGrounded);

            if (isGrounded)
            {
                OnGrounded();
            }
        }

        private void OnGrounded()
        {
            var characterPosition = _ctx.transform.position;
            _ctx.rigidbody.position = new Vector3(characterPosition.x, _ctx.state.currentRoadline.Value.transform.position.y, characterPosition.z);
            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

            _currentAction = CharacterAction.Moving;
        }
    }
}