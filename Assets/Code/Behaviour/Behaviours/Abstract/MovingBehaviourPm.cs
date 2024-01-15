﻿using Shared;
using UnityEngine;
using UniRx;

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
        protected const int SPEED_MULTIPLIER = 100;

        protected static readonly int _idleHash = Animator.StringToHash("Idle");
        protected static readonly int _damageHash = Animator.StringToHash("Damage");

        protected MovingBehaviourPm(Ctx ctx) : base(ctx)
        {
            AddUnsafe(_ctx.onFinishReached.Subscribe(OnFinish));
            AddUnsafe(_ctx.onCrash.Subscribe(OnCrash));
        }

        /// <summary>
        /// Базовая логика перемещения персонажа
        /// </summary>
        protected virtual void Move()
        {
            var speedZ = _ctx.state.speed.z * SPEED_MULTIPLIER * Time.fixedDeltaTime;
            var forwardVelocity = _ctx.transform.forward * speedZ;

            var sideVelocity = CalculateSideVelocity();

            var verticalVelocity = Vector3.up * _ctx.rigidbody.velocity.y;

            _ctx.rigidbody.velocity = forwardVelocity + sideVelocity + verticalVelocity;
        }
        
        protected virtual void OnCrash(GameObject obstacle)
        {
            _ctx.rigidbody.velocity = Vector3.zero;
            _currentAction = CharacterAction.Idle;
            _ctx.state.lives.Value -= 1;
            
            if (_ctx.state.lives.Value > 0)
            {
                Respawn();
            }
        }

        protected Vector3 CalculateSideVelocity()
        {
            var speedX = _ctx.state.speed.x * SPEED_MULTIPLIER * Time.fixedDeltaTime;

            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var sideVelocity = Vector3.zero;
            var localDistance = _ctx.transform.InverseTransformPoint(roadlinePosition);
            var direction = localDistance.x > 0 ? _ctx.transform.right : -_ctx.transform.right;

            if (Mathf.Abs(localDistance.x) > _ctx.toleranceDistance.x)
            {
                sideVelocity = direction * speedX;
            }

            return sideVelocity;
        }

        protected void OnChangeSide(Direction direction)
        {
            if (!CanMoveToDirection(direction)) return;

            var currentLine = _ctx.state.currentRoadline;
            var nextRoadline = direction == Direction.Left ? currentLine.Previous : currentLine.Next;

            _ctx.state.currentRoadline = nextRoadline;
        }

        protected bool CanMoveToDirection(Direction direction)
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

        protected virtual void OnFinish()
        {
            _currentAction = CharacterAction.Idle;
            _ctx.onFinishReached?.Notify();
        }

        protected abstract void Respawn();
    }
}