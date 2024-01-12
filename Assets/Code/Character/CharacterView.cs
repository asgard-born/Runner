using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Character
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _mask;
        [SerializeField, Range(.25f, .5f)] private float _overlapRadius = .3f;
        [SerializeField] private int _updateFrequencyMillisec = 50;
        [SerializeField] private Transform _interractingPointFirst;
        [SerializeField] private Transform _interractingPointSecond;
        [SerializeField] private Animator _animator;

        public Animator animator => _animator;
        public Rigidbody rigidbody => _rigidbody;

        private ReactiveCommand<Collider> _onInterraction;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
        }

        private void Awake()
        {
            CheckForInteractionProcessAsync();
        }

        public void SetCtx(Ctx ctx)
        {
            _onInterraction = ctx.onInterraction;
        }

        private async void CheckForInteractionProcessAsync()
        {
            while (true)
            {
                var colliders = new Collider[10];
                var count = Physics.OverlapCapsuleNonAlloc(_interractingPointFirst.position, _interractingPointSecond.position, _overlapRadius, colliders, _mask, QueryTriggerInteraction.Collide);

                if (count > 0)
                {
                    var actualColliders = colliders.Where(x => x != null);

                    foreach (var actualCollider in actualColliders)
                    {
                        _onInterraction?.Execute(actualCollider);
                    }
                }

                await UniTask.Delay(TimeSpan.FromMilliseconds(_updateFrequencyMillisec));
            }
        }
    }
}