using Code.Configs;
using UnityEngine;

namespace Code.Player
{
    public class PlayerEntity : MonoBehaviour
    {
        public bool canRun;

        private Rigidbody _rigidbody;
        private PlayersConfigs _playersConfigs;
        private float _currentSpeed;

        public void Init(PlayersConfigs playersConfigs)
        {
            _playersConfigs = playersConfigs;
            _currentSpeed = playersConfigs.speed;
        }

        public void AddSpeed(float factor)
        {
            _currentSpeed *= factor;
        }

        public void ReduceSpeed(float factor)
        {
            _currentSpeed /= factor;
        }

        public void RotateLeft()
        {
        }

        public void RotateRight()
        {
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Run()
        {
            _rigidbody.MovePosition(transform.position + transform.forward * _currentSpeed * Time.deltaTime);
        }

        private void Update()
        {
            if (canRun)
            {
                Run();
            }
        }
    }
}