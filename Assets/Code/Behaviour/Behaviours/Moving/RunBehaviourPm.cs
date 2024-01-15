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
            _ctx.state.speed = _ctx.configs.speed;
            _ctx.state.jumpForce = _ctx.configs.jumpForce;

            SetDefaultCondition();

            _hasStarted = true;
        }

        protected override void Behave()
        {
            switch (_currentAction)
            {
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

                case CharacterAction.Damage:
                    _ctx.rigidbody.velocity = Vector3.zero;
                    _currentAction = CharacterAction.Idle;
                    _ctx.state.lives.Value -= 1;

                    if (_ctx.state.lives.Value > 0)
                    {
                        Respawn();
                    }

                    break;
            }
        }

        protected override void Respawn()
        {
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            var currentZonePos = _ctx.state.currentSaveZone.position;

            _ctx.rigidbody.useGravity = false;
            _ctx.collider.enabled = false;
            _ctx.state.currentRoadline = _ctx.state.currentRoadline.List.First;

            _ctx.rigidbody.DOMove(currentZonePos, _ctx.crashDelay).OnComplete(OnRespawned);
        }

        protected override void OnTimesOver()
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
            if (_currentAction != CharacterAction.Moving) return;

            _ctx.rigidbody.useGravity = true;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _ctx.animator.SetTrigger(_jumpingHash);

            _currentAction = CharacterAction.Jumping;
        }

        private void OnSwipeDirection(Direction direction)
        {
            if (_currentAction == CharacterAction.Damage || _currentAction == CharacterAction.Idle) return;

            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    OnChangeSide(direction);

                    break;

                case Direction.Up:
                    TryJump(_ctx.state.jumpForce);

                    break;
            }
        }

        protected override async void OnCrash(GameObject obstacle)
        {
            _currentAction = CharacterAction.Damage;
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

            _ctx.rigidbody.position = new Vector3(characterPosition.x, _ctx.state.currentRoadline.Value.transform.position.y, characterPosition.z);
            _ctx.collider.enabled = true;
            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

            _currentAction = CharacterAction.Moving;
            _ctx.animator.SetTrigger(_runningHash);
        }
    }
}