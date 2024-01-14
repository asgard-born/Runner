using Behaviour.Behaviours.Abstract;
using Shared;
using UnityEngine;

namespace Behaviour.Behaviours.Moving
{
    public class FlyBehaviourPm : MovingBehaviourPm
    {
        public FlyBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        protected override void InitializeState()
        {
            _ctx.state.speed = _ctx.configs.speed;
        }

        protected override void OnTimesOver()
        {
            _currentAction = CharacterAction.Landing;
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

                case CharacterAction.Lifting:
                    Move();
                    Lifting();

                    break;

                case CharacterAction.Landing:
                    Move();
                    Landing();

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
            }
        }

        private void Lifting()
        {
            var transform = _ctx.characterTransform;
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = (roalinePosition.y + _ctx.configs.height) - transform.position.y;

            if (Mathf.Abs(localDistance) > _ctx.toleranceDistance.y)
            {
                MovingVertical(transform.up);
            }
            else
            {
                _currentAction = CharacterAction.Moving;
            }
        }

        private void Landing()
        {
            var transform = _ctx.characterTransform;
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = _ctx.characterTransform.InverseTransformPoint(roalinePosition);

            if (Mathf.Abs(localDistance.y) > _ctx.toleranceDistance.y)
            {
                MovingVertical(-transform.up);
            }
            else
            {
                Finish();
            }
        }

        private void Finish()
        {
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        private void MovingVertical(Vector3 direction)
        {
            var newVelocity = direction * _ctx.configs.speed.y * Time.fixedDeltaTime;
            newVelocity = new Vector3(_ctx.rigidbody.velocity.x, newVelocity.y, _ctx.rigidbody.velocity.z);
            _ctx.rigidbody.velocity = newVelocity;
        }
    }
}