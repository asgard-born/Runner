using Behaviour.Behaviours.Abstract;
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
            AddUnsafe(_ctx.onCrash.Subscribe(OnCrash));
        }

        protected override void Initialize()
        {
            _ctx.state.speed = _ctx.configs.speed;

            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _currentAction = CharacterAction.Lifting;

            _ctx.animator.SetTrigger(_flyingHash);
        }

        protected override void Behave()
        {
            switch (_currentAction)
            {
                case CharacterAction.Idle:
                    _ctx.animator.SetBool(_idleHash, true);

                    break;

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

        private void OnCrash(GameObject obstacle)
        {
            _ctx.rigidbody.velocity = Vector3.zero;
            _currentAction = CharacterAction.Idle;
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