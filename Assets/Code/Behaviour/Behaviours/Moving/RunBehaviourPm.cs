using Behaviour.Behaviours.Abstract;
using DG.Tweening;
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
        protected static readonly int _runningHash = Animator.StringToHash("Running");
        protected static readonly int _jumpingHash = Animator.StringToHash("Jump");
        protected static readonly int _fallingHash = Animator.StringToHash("Falling");

        public RunBehaviourPm(Ctx ctx) : base(ctx)
        {
            AddUnsafe(_ctx.onSwipeDirection.Subscribe(OnSwipeDirection));
        }

        protected override void Initialize()
        {
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

        private void MoveToSavePoint()
        {
            _ctx.state.currentRoadline = _ctx.state.currentRoadline.List.First;
            
            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var targetPosition = new Vector3(roadlinePosition.x, roadlinePosition.y, _ctx.state.currentSavePoint.position.z);

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

        protected override void OnTimeOver()
        {
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        private void OnRespawned()
        {
            SetDefaultCondition();
            _ctx.onRespawned?.Notify();
        }

        private void TryJump(float jumpForce)
        {
            if (_ctx.state.currentAction != CharacterAction.Moving) return;

            _ctx.rigidbody.useGravity = true;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _ctx.animator.SetTrigger(_jumpingHash);

            _ctx.state.currentAction = CharacterAction.Jumping;
        }

        private void OnSwipeDirection(Direction direction)
        {
            if (_ctx.state.currentAction == CharacterAction.Respawn || _ctx.state.currentAction == CharacterAction.Idle) return;

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

        protected override async void OnCrash(GameObject obstacle)
        {
            if (_ctx.state.currentAction == CharacterAction.Respawn) return;
            
            _ctx.animator.SetBool(_fallingHash, false);

            base.OnCrash(obstacle);
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

            _ctx.animator.SetBool(_fallingHash, !isGrounded);

            if (isGrounded)
            {
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

            _ctx.animator.SetBool(_fallingHash, false);
            _ctx.animator.SetTrigger(_runningHash);
        }
    }
}