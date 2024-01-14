using Shared;
using UnityEngine;

namespace Behaviour.Behaviours.Abstract
{
    /// <summary>
    /// Абстракция над поведениями, которые являются типом перемещения персонажа
    /// Выделяет в себе общие методы и поля, чтобы избежать дублирования кода
    /// и упорядочить типы по одному общему назначению
    /// </summary>
    public abstract class MovingBehaviourPm : CharacterBehaviourPm
    {
        protected CharacterAction _currentAction;

        protected static readonly int _idle = Animator.StringToHash("Idle");
        protected static readonly int _running = Animator.StringToHash("Running");
        protected static readonly int _jumping = Animator.StringToHash("Jump");
        protected static readonly int _falling = Animator.StringToHash("Falling");
        protected static readonly int _damage = Animator.StringToHash("Damage");

        protected MovingBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Базовая логика перемещения персонажа
        /// </summary>
        protected virtual void Move()
        {
            var speed = _ctx.state.speed.z;

            if (speed <= 0) return;
            
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;

            var newVelocity = _ctx.transform.position + _ctx.transform.forward * speed * Time.fixedDeltaTime;
            var localDistance = _ctx.transform.InverseTransformPoint(roalinePosition);

            var direction = localDistance.x > 0 ? _ctx.transform.right : -_ctx.transform.right;

            if (Mathf.Abs(localDistance.x) > _ctx.toleranceDistance.x)
            {
                newVelocity += direction * _ctx.state.speed.x * Time.fixedDeltaTime;
            }

            newVelocity = (newVelocity - _ctx.transform.position) / Time.fixedDeltaTime;
            newVelocity = new Vector3(newVelocity.x, _ctx.rigidbody.velocity.y, newVelocity.z);

            _ctx.rigidbody.velocity = newVelocity;
        }

        protected virtual void OnChangeSide(Direction direction)
        {
            if (!CanMoveToDirection(direction)) return;

            var currentLine = _ctx.state.currentRoadline;
            var nextRoadline = direction == Direction.Left ? currentLine.Previous : currentLine.Next;

            _ctx.state.currentRoadline = nextRoadline;
        }

        protected virtual bool CanMoveToDirection(Direction direction)
        {
            if (direction == Direction.Left && _ctx.state.currentRoadline.Previous != null)
            {
                return true;
            }

            if (direction == Direction.Right && _ctx.state.currentRoadline.Next != null)
            {
                return true;
            }

            return false;
        }
    }
}