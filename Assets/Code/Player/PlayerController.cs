﻿using System;
using Code.Boosters.Essences;
using Code.Configs;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AnimationSystem _animationSystem;

        private bool _canRun;
        private Rigidbody _rigidbody;
        private Ctx _ctx;
        private int _currentJumpingCount;

        private State _currentState;
        private PlayerStats _stats;

        private Action<float> _onLivesChangedCallback;

        private enum State
        {
            None,
            Running,
            Jumping,
            Falling,
            FallingOut,
        }

        public struct Ctx
        {
            public PlayersConfigs playersConfigs;
            public Action<float> onLivesChangedCallback;
            public Action deathCallback;
            public Transform playerSpawnPoint;
        }

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            var playerStatsCtx = new PlayerStats.Ctx
            {
                playersConfigs = _ctx.playersConfigs,
                playerSpawnPoint = ctx.playerSpawnPoint
            };

            _stats = new PlayerStats(playerStatsCtx);

            _onLivesChangedCallback = ctx.onLivesChangedCallback;
        }

        public void TryJump()
        {
            if ((_currentJumpingCount == 0 && !IsGrounded()) || _currentJumpingCount >= _stats.maxJumpingTimes)
            {
                return;
            }

            _currentJumpingCount += 1;
            _rigidbody.AddForce(Vector3.up * _stats.jumpForce, ForceMode.VelocityChange);
            _animationSystem.PlayJump();

            _currentState = State.Jumping;
        }

        public void StartRun()
        {
            _canRun = true;
            _animationSystem.PlayRun();
        }

        public void RotateLeft()
        {
            transform.Rotate(Vector3.up * -90);
        }

        public void RotateRight()
        {
            transform.Rotate(Vector3.up * 90);
        }

        public void Stop(bool withAnimation = false)
        {
            _canRun = false;

            if (withAnimation)
            {
                _animationSystem.PlayIdle();
            }
        }

        public void Hit()
        {
            _stats.RemoveLifes(1);
            _animationSystem.PlayDamage();
            _onLivesChangedCallback?.Invoke(-1);

            if (_stats.currentLives <= 0)
            {
                _ctx.deathCallback?.Invoke();
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            CheckState();
        }

        private void CheckState()
        {
            switch (_currentState)
            {
                case State.Running:
                    _animationSystem.PlayRun();

                    if (IsFallingOut())
                    {
                        _currentState = State.FallingOut;
                    }

                    break;

                case State.Jumping:
                    if (IsFalling())
                    {
                        _currentState = State.Falling;
                    }

                    break;

                case State.Falling:
                    if (IsGrounded())
                    {
                        _currentJumpingCount = 0;
                        _currentState = State.Running;
                    }
                    else if (IsFallingOut())
                    {
                        _currentState = State.FallingOut;
                    }

                    break;

                case State.FallingOut:
                    _animationSystem.PlayFalling();

                    break;
            }
        }

        private void FixedUpdate()
        {
            if (_canRun)
            {
                Run();
            }
        }

        private void Run()
        {
            var speed = _stats.currentSpeed;
            _rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }

        private bool IsGrounded()
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, .2f))
            {
                if (_rigidbody.velocity.y <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsFalling()
        {
            return _rigidbody.velocity.y < 0;
        }

        private bool IsFallingOut()
        {
            return IsFalling() && _rigidbody.position.y < _stats.valueYToFallOut;
        }

        public void TakeBonus(BoosterType boosterType, float value)
        {
            switch (boosterType)
            {
                case BoosterType.Heart:
                    _onLivesChangedCallback?.Invoke(value);
                    _stats.AddLifes((int)value);
                    break;
                
                case BoosterType.Immune:
                    _stats.AddImmune((int)value);
                    break;
                
                case BoosterType.Speed:
                    _stats.AddSpeed(value);
                    break;
            }
        }
    }
}