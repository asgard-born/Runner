using Behaviour.Behaviours.Abstract;
using Shared;
using UnityEngine;
using UniRx;

namespace Behaviour.Behaviours.Moving
{
    /// <summary>
    /// Поведение игрока типа Бег. Поведению делегируется работа с компонентами представления
    /// и обработка пользовательского свайпа в разные стороны
    /// </summary>
    public class RunBehaviourPm : MovingBehaviourPm
    {
        private static readonly int _runningHash = Animator.StringToHash("Running");
        private static readonly int _jumpingHash = Animator.StringToHash("Jumping");
        private static readonly int _fallingHash = Animator.StringToHash("Falling");

        public RunBehaviourPm(Ctx ctx) : base(ctx)
        {
            AddUnsafe(_ctx.onSwipeDirection.Subscribe(OnSwipeDirection));
        }

        protected override void Initialize()
        {
            ClearAnimations();
            SetDefaultCondition();

            _hasStarted = true;
        }

        protected override void MovingProcess()
        {
            Debug.Log($"<color='red'>Current action/behaviour {_ctx.state.currentAction.ToString()}, {_ctx.configs.name}</color>");

            switch (_ctx.state.currentAction)
            {
                case CharacterAction.Moving:
                    Move();

                    break;

                case CharacterAction.Jumping:
                    Move();

                    if (!IsGrounded())
                    {
                        _ctx.state.currentAction = CharacterAction.Falling;
                    }

                    break;

                case CharacterAction.Falling:
                    Move();
                    OnFalling();

                    break;

                case CharacterAction.Respawn:
                    Respawn();

                    break;
            }
        }

        protected override void Respawn()
        {
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _ctx.rigidbody.useGravity = false;
            _ctx.collider.enabled = false;

            MoveToSavePoint();
        }

        protected override void Reset()
        {
            ClearAnimations();
        }

        protected override void OnTimeOver()
        {
            Reset();

            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        protected override void OnCrash(GameObject obstacle)
        {
            if (_ctx.state.currentAction == CharacterAction.Respawn) return;

            ClearAnimations();

            base.OnCrash(obstacle);
        }

        protected override void OnGameOver()
        {
            ClearAnimations();

            base.OnGameOver();
        }

        private void MoveToSavePoint()
        {
            _ctx.state.currentRoadline = _ctx.state.currentRoadline.List.First;

            var targetPosition = _ctx.state.currentSavePoint.position;
            var distanceVector = targetPosition - _ctx.transform.position;

            if (distanceVector.magnitude > _ctx.toleranceDistance.x)
            {
                _ctx.rigidbody.velocity = distanceVector.normalized * _ctx.state.speed.z * VELOCITY_MULTIPLIER * Time.fixedDeltaTime;
            }
            else
            {
                _ctx.rigidbody.position = targetPosition;
                _ctx.rigidbody.velocity = Vector3.zero;

                OnRespawned();
            }
        }

        private void OnRespawned()
        {
            SetDefaultCondition();
            _ctx.onRespawned?.Notify();
        }

        private void TryJump(float jumpForce)
        {
            if (_ctx.state.currentAction != CharacterAction.Moving) return;
            if (!IsGrounded()) return;
            if (_ctx.rigidbody.velocity.y < 0) return;

            _ctx.rigidbody.useGravity = true;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            _ctx.animator.ResetTrigger(_runningHash);
            _ctx.animator.SetTrigger(_jumpingHash);

            _ctx.state.currentAction = CharacterAction.Jumping;
        }

        private void OnSwipeDirection(Direction direction)
        {
            if (_ctx.state.currentAction is CharacterAction.Respawn or CharacterAction.Idle) return;

            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    OnChangeSide(direction);

                    break;

                case Direction.Up:
                    TryJump(_ctx.state.height);

                    break;
            }
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

            _ctx.animator.ResetTrigger(_jumpingHash);

            if (isGrounded)
            {
                _ctx.animator.SetTrigger(_fallingHash);
                SetDefaultCondition();
            }
        }

        private void SetDefaultCondition()
        {
            var characterPosition = _ctx.transform.position;

            _ctx.collider.enabled = true;
            _ctx.rigidbody.useGravity = false;
            _ctx.transform.position = new Vector3(characterPosition.x, _ctx.state.currentRoadline.Value.transform.position.y, characterPosition.z);
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

            _ctx.state.currentAction = CharacterAction.Moving;
            _ctx.animator.SetTrigger(_runningHash);
        }

        private void ClearAnimations()
        {
            _ctx.animator.ResetTrigger(_idleHash);
            _ctx.animator.ResetTrigger(_jumpingHash);
            _ctx.animator.ResetTrigger(_fallingHash);
            _ctx.animator.ResetTrigger(_runningHash);
        }
    }
}