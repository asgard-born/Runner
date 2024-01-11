using UnityEngine;

namespace Character
{
    public class CharacterAnimatorView : MonoBehaviour
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
        }

        public void PlayRun()
        {
            _animator.SetBool(_running, true);
        }

        public void PlayDamage()
        {
            _animator.SetTrigger(_damage);
        }

        public void PlayJump()
        {
            _animator.SetTrigger(_jumping);
        }

        public void Falling()
        {
            _animator.SetTrigger(_falling);
        }
    }
}