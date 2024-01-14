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
        protected bool _isMoving;
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
            var speed = _ctx.state.speed;

            if (speed <= 0) return;

            var transform = _ctx.characterTransform;
            var roalinePosition = _ctx.state.currentRoadline.Value.transform.position;

            var newVelocity = transform.position + transform.forward * speed * Time.fixedDeltaTime;
            var localDistance = _ctx.characterTransform.InverseTransformPoint(roalinePosition);

            var direction = localDistance.x > 0 ? transform.right : -transform.right;

            if (Mathf.Abs(localDistance.x) > _ctx.toleranceSideDistance)
            {
                newVelocity += direction * _ctx.state.sideSpeed * Time.fixedDeltaTime;
            }

            newVelocity = (newVelocity - transform.position) / Time.fixedDeltaTime;
            newVelocity = new Vector3(newVelocity.x, _ctx.rigidbody.velocity.y, newVelocity.z);
            _ctx.state.velocity = newVelocity;

            _ctx.rigidbody.velocity = _ctx.state.velocity;
        }

        protected virtual void OnChangeSide(SwipeDirection swipeDirection)
        {
            if (!CanMoveToDirection(swipeDirection)) return;

            var currentLine = _ctx.state.currentRoadline;
            var nextRoadline = swipeDirection == SwipeDirection.Left ? currentLine.Previous : currentLine.Next;

            _ctx.state.currentRoadline = nextRoadline;
        }

        protected virtual bool CanMoveToDirection(SwipeDirection swipeDirection)
        {
            if (swipeDirection == SwipeDirection.Left && _ctx.state.currentRoadline.Previous != null)
            {
                return true;
            }

            if (swipeDirection == SwipeDirection.Right && _ctx.state.currentRoadline.Next != null)
            {
                return true;
            }

            return false;
        }
    }
}