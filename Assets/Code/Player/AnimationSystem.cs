using UnityEngine;

namespace Code.Player
{
    public class AnimationSystem : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int _idle = Animator.StringToHash("Idle");
        private static readonly int _running = Animator.StringToHash("Running");
        private static readonly int _jumping = Animator.StringToHash("Jumping");
        private static readonly int _falling = Animator.StringToHash("Falling");
        private static readonly int _damage = Animator.StringToHash("Damage");

        public void PlayIdle()
        {
            _animator.SetBool(_idle, true);
            _animator.SetBool(_running, false);
            _animator.SetBool(_falling, false);
            _animator.SetBool(_jumping, false);
        }
        
        public void PlayRun()
        {
            _animator.SetBool(_running, true);
            _animator.SetBool(_idle, false);
            _animator.SetBool(_jumping, false);
            _animator.SetBool(_falling, false);
        }

        public void PlayDamage()
        {
            _animator.SetTrigger(_damage);
        }
        
        public void PlayJump()
        {
            if (_animator.GetBool(_jumping))
            {
                _animator.Play(_jumping, -1, 0f);
            }
            
            _animator.SetBool(_jumping, true);
            _animator.SetBool(_running, false);
        }

        public void PlayFalling()
        {
            _animator.SetBool(_falling, true);
            _animator.SetBool(_running, false);
            _animator.SetBool(_jumping, false);
        }
    }
}