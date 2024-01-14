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

        protected override void Initialize()
        {
            _ctx.state.speed = _ctx.configs.speed;

            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _currentAction = CharacterAction.Lifting;
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

        protected override void OnTimesOver()
        {
            _currentAction = CharacterAction.Landing;
        }

        private void Lifting()
        {
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = (roalinePosition.y + _ctx.configs.height) - _ctx.transform.position.y;

            if (Mathf.Abs(localDistance) > _ctx.toleranceDistance.y)
            {
                MovingVertical(_ctx.transform.up);
            }
            else
            {
                _currentAction = CharacterAction.Moving;
            }
        }

        private void Landing()
        {
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = _ctx.transform.InverseTransformPoint(roalinePosition);

            if (Mathf.Abs(localDistance.y) > _ctx.toleranceDistance.y)
            {
                MovingVertical(-_ctx.transform.up);
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
            var speed = _ctx.state.speed.y;

            var newVelocity = _ctx.transform.position + direction * speed * Time.fixedDeltaTime;

            var currentVelocity = _ctx.rigidbody.velocity;
            newVelocity = new Vector3(currentVelocity.x, newVelocity.y, currentVelocity.z);
            _ctx.rigidbody.velocity = newVelocity;
        }
    }
}