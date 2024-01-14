using Behaviour.Behaviours.Abstract;
using DG.Tweening;
using Shared;
using UnityEngine;

namespace Behaviour.Behaviours.Moving
{
    public class FlyBehaviourPm : MovingBehaviourPm
    {
        protected static readonly int _flyingHash = Animator.StringToHash("Flying");

        public FlyBehaviourPm(Ctx ctx) : base(ctx)
        {
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
                    Lift();

                    break;

                case CharacterAction.Landing:
                    Land();

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

        private void Lift()
        {
            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var newPointY = roadlinePosition.y + _ctx.configs.height;
            var newPos = new Vector3(_ctx.transform.position.x, newPointY, _ctx.transform.position.z);

            _ctx.transform.DOMove(newPos, _ctx.configs.speed.y).OnComplete(() => _currentAction = CharacterAction.Moving);
        }

        private void Land()
        {
            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var newPointY = roadlinePosition.y;
            var newPos = new Vector3(_ctx.transform.position.x, newPointY, _ctx.transform.position.z);

            _ctx.transform.DOMove(newPos, _ctx.configs.speed.y).OnComplete(Finish);
        }

        private void Finish()
        {
            _currentAction = CharacterAction.None;
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }
    }
}