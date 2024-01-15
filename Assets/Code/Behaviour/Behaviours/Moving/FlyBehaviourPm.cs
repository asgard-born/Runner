using Behaviour.Behaviours.Abstract;
using DG.Tweening;
using Shared;
using UnityEngine;
using UniRx;

namespace Behaviour.Behaviours.Moving
{
    public class FlyBehaviourPm : MovingBehaviourPm
    {
        protected static readonly int _flyingHash = Animator.StringToHash("Flying");

        public FlyBehaviourPm(Ctx ctx) : base(ctx)
        {
            AddUnsafe(_ctx.onSwipeDirection.Subscribe(OnSwipeDirection));
        }

        protected override void Initialize()
        {
            _ctx.state.speed = _ctx.configs.speed;

            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _currentAction = CharacterAction.Lifting;

            _ctx.animator.SetTrigger(_flyingHash);
        }

        protected override void MovingProcess()
        {
            switch (_currentAction)
            {
                case CharacterAction.Moving:
                    Move();

                    break;

                case CharacterAction.Lifting:
                    Lifting();

                    break;

                case CharacterAction.Landing:
                    Landing();

                    break;
            }
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
            }
        }

        protected override void OnTimesOver()
        {
            _currentAction = CharacterAction.Landing;
        }

        protected override void Respawn()
        {
            var currentZonePos = _ctx.state.currentSaveZone.position;
            var newPosition = new Vector3(currentZonePos.x, _ctx.transform.position.y, currentZonePos.z);

            _ctx.state.currentRoadline = _ctx.state.currentRoadline.List.First;
            _ctx.rigidbody.DOMove(newPosition, _ctx.crashDelay).OnComplete(OnRespawned);
        }

        private void OnRespawned()
        {
            SetDefaultCondition();
            _ctx.onRespawned?.Notify();
        }

        private void SetDefaultCondition()
        {
            _ctx.collider.enabled = true;
            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            _currentAction = CharacterAction.Moving;
            _ctx.animator.SetTrigger(_flyingHash);
        }

        private void Lifting()
        {
            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = (roadlinePosition.y + _ctx.configs.height) - _ctx.transform.position.y;

            if (Mathf.Abs(localDistance) > _ctx.toleranceDistance.y)
            {
                MoveVertical(_ctx.transform.up);
            }
            else
            {
                OnLifted();
            }
        }

        private void OnLifted()
        {
            _hasStarted = true;
            _currentAction = CharacterAction.Moving;
        }

        private void Landing()
        {
            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = _ctx.transform.InverseTransformPoint(roadlinePosition);

            if (Mathf.Abs(localDistance.y) > _ctx.toleranceDistance.y)
            {
                MoveVertical(-_ctx.transform.up);
            }
            else
            {
                OnLanded();
            }
        }

        private void OnLanded()
        {
            var characterPosition = _ctx.transform.position;
            _ctx.transform.position = new Vector3(characterPosition.x, _ctx.state.currentRoadline.Value.transform.position.y, characterPosition.z);

            Finish();
        }

        private void MoveVertical(Vector3 direction)
        {
            var speedY = _ctx.state.speed.y;
            var verticalVelocity = Vector3.up + direction * speedY * SPEED_MULTIPLIER * Time.fixedDeltaTime;

            var sideVelocity = CalculateSideVelocity();

            var newVelocity = verticalVelocity + sideVelocity;
            _ctx.rigidbody.velocity = newVelocity;
        }

        private void Finish()
        {
            _currentAction = CharacterAction.None;
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }
    }
}