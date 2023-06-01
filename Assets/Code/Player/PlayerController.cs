using System;
using System.Collections;
using System.Collections.Generic;
using Code.Boosters.Essences;
using Code.Configs;
using Code.Platforms.Abstract;
using Code.Platforms.Concrete;
using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AnimationSystem _animationSystem;
        [SerializeField] private GameObject _immuneObject;

        private bool _canRun;
        private Rigidbody _rigidbody;
        private Ctx _ctx;
        private int _currentJumpingCount;

        private State _currentState;
        private PlayerStats _stats;

        private Action<float> _onLivesChangedCallback;
        private Action<float, float> _onSpeedChangedCallback;
        public Platform currentPlatform;
        private Coroutine _resumeSpeedRoutine;
        private CapsuleCollider _collider;

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
            public Action<float, float> onSpeedChangedCallback;
            public Action deathCallback;
            public Platform playerSpawnPoint;
        }

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            currentPlatform = _ctx.playerSpawnPoint;

            var playerStatsCtx = new PlayerStats.Ctx
            {
                playersConfigs = _ctx.playersConfigs,
                playerSpawnPoint = ctx.playerSpawnPoint
            };

            _stats = new PlayerStats(playerStatsCtx);

            _onLivesChangedCallback = ctx.onLivesChangedCallback;
            _onSpeedChangedCallback = ctx.onSpeedChangedCallback;
            _immuneObject.SetActive(false);
        }

        public void Respawn(LinkedList<Platform> platforms)
        {
            AddLives(1);
            _immuneObject.SetActive(false);
            _currentJumpingCount = 0;

            var getNext = false;

            Platform platformToSpawn = currentPlatform;
            LinkedListNode<Platform> curNode = platforms.Find(currentPlatform).Next;

            while (curNode != null && (curNode.Value is AbyssPlatfrom || curNode.Value is AbyssLargePlatform))
            {
                curNode = curNode.Next;
            }

            if (curNode != null)
            {
                platformToSpawn = curNode.Value;
            }

            transform.position = platformToSpawn.respawnPoint.position;
            _rigidbody.isKinematic = false;

            var currentSpeed = _stats.currentSpeed;
            var slowSpeed = currentSpeed / 2;
            _stats.SetSpeed(slowSpeed);

            if (_resumeSpeedRoutine != null)
            {
                StopCoroutine(_resumeSpeedRoutine);
            }

            _resumeSpeedRoutine = StartCoroutine(StartResumeSpeed(currentSpeed));
            _collider.enabled = true;

            StartRun();
        }

        private const float _timeSecSpeedRemaining = 2.5f;

        private IEnumerator StartResumeSpeed(float previousSpeed)
        {
            var speedToAdd = previousSpeed - _stats.currentSpeed;
            var time = _timeSecSpeedRemaining;

            while (time >= 0)
            {
                time -= Time.deltaTime;
                _stats.ChangeSpeed(Time.deltaTime * speedToAdd / _timeSecSpeedRemaining);

                yield return new WaitForEndOfFrame();
            }

            ;
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
            _currentState = State.Running;
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
            if (_resumeSpeedRoutine != null)
            {
                StopCoroutine(_resumeSpeedRoutine);
            }

            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _currentState = State.None;
            _canRun = false;

            if (withAnimation)
            {
                _animationSystem.PlayIdle();
            }
        }

        public void TryHit()
        {
            if (_stats.immuneValue > 0) return;
            if (_stats.currentLives == 0) return;

            _stats.RemoveLifes(1);
            _animationSystem.PlayDamage();
            _onLivesChangedCallback?.Invoke(-1);

            if (_stats.currentLives == 0)
            {
                Death();
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            CheckState();
            UpdateImmune();
        }

        private void UpdateImmune()
        {
            if (_stats.immuneValue > 0)
            {
                _stats.immuneValue -= Time.deltaTime;

                if (_stats.immuneValue <= 0)
                {
                    _stats.immuneValue = 0;
                    RemoveImmune();
                }
            }
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
                    Death();
                    _currentState = State.None;

                    break;
            }
        }

        private void Death()
        {
            if (_stats.currentLives > 0)
            {
                _onLivesChangedCallback?.Invoke(-_stats.currentLives);
                _stats.RemoveLifes(_stats.currentLives);
            }

            _ctx.deathCallback?.Invoke();
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
                    AddLives(value);

                    break;

                case BoosterType.Immune:
                    AddImmune(value);

                    break;

                case BoosterType.Speed:
                    ChangeSpeed(value);

                    break;
            }
        }

        private void ChangeSpeed(float value)
        {
            _onSpeedChangedCallback?.Invoke(_stats.currentSpeed, value);
            _stats.ChangeSpeed(value);
        }

        private void AddLives(float value)
        {
            _onLivesChangedCallback?.Invoke(value);
            _stats.AddLives((int)value);
        }

        private void AddImmune(float value)
        {
            _immuneObject.SetActive(true);
            _stats.AddImmune(value);
        }

        private void RemoveImmune()
        {
            _immuneObject.SetActive(false);
        }
    }
}