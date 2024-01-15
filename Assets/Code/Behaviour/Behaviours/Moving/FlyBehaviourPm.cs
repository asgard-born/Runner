﻿using Behaviour.Behaviours.Abstract;
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
            _ctx.rigidbody.useGravity = false;
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _ctx.state.currentAction = CharacterAction.Lifting;

            _ctx.animator.SetBool(_flyingHash, true);
        }

        protected override void MovingProcess()
        {
            Debug.Log($"<color='red'>Current action/behaviour {_ctx.state.currentAction.ToString()}, {_ctx.configs.name}</color>");

            switch (_ctx.state.currentAction)
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

                case CharacterAction.Respawn:
                    Respawn();

                    break;
            }
        }

        protected override void OnTimeOver()
        {
            _ctx.state.currentAction = CharacterAction.Landing;
        }

        protected override void Respawn()
        {
            _ctx.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _ctx.rigidbody.useGravity = false;
            _ctx.collider.enabled = false;

            MoveToSavePoint();
        }

        protected override void OnCrash(GameObject obstacle)
        {
            if (_ctx.state.currentAction == CharacterAction.Respawn) return;

            _ctx.animator.SetBool(_idleHash, false);
            _ctx.animator.SetBool(_flyingHash, false);

            base.OnCrash(obstacle);
        }

        protected override void Reset()
        {
            _ctx.animator.SetBool(_flyingHash, false);
            _ctx.animator.SetBool(_idleHash, false);
        }

        private void OnBehaviourFinished()
        {
            Reset();
            
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
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
            }
        }

        private void MoveToSavePoint()
        {
            _ctx.state.currentRoadline = _ctx.state.currentRoadline.List.First;

            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var targetPosition = new Vector3(roadlinePosition.x, _ctx.transform.position.y, _ctx.state.currentSavePoint.position.z);

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
            _ctx.state.currentAction = CharacterAction.Moving;

            _ctx.animator.SetBool(_flyingHash, true);
            _ctx.animator.SetBool(_idleHash, false);
        }

        private void Lifting()
        {
            var roadlinePosition = _ctx.state.currentRoadline.Value.transform.position;
            var localDistance = (roadlinePosition.y + _ctx.configs.height) - _ctx.transform.position.y;

            if (localDistance >= _ctx.toleranceDistance.y)
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
            _ctx.state.currentAction = CharacterAction.Moving;
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
            _ctx.state.currentAction = CharacterAction.Finish;

            _ctx.animator.SetBool(_flyingHash, false);

            OnBehaviourFinished();
        }

        private void MoveVertical(Vector3 direction)
        {
            var verticalVelocity = _ctx.transform.up + direction * _ctx.configs.speed.y * VELOCITY_MULTIPLIER * Time.fixedDeltaTime;
            var sideVelocity = CalculateSideVelocity();

            _ctx.rigidbody.velocity = verticalVelocity + sideVelocity;
        }
    }
}