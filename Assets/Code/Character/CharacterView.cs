using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Framework.Reactive;
using UniRx;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Представление персонажа. Имеет двустороннюю коммуникацию:
    /// отправляет события коллизий по маске, принимает обратную о состоянии персонажа
    /// </summary>
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _mask;
        [SerializeField, Range(.25f, .5f)] private float _overlapRadius = .3f;
        [SerializeField] private int _updateFrequencyMillisec = 50;
        [SerializeField] private Transform _interractingPointFirst;
        [SerializeField] private Transform _interractingPointSecond;

        private bool _isChecking = true;
        private ReactiveCommand<Collider> _onInterraction;

        public Collider collider => _collider;
        public Rigidbody rigidbody => _rigidbody;
        public Animator animator => _animator;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<GameObject> onCrash;
            public ReactiveTrigger onRespawned;
        }

        private void Awake()
        {
            CheckForInteractionProcessAsync();
        }

        public void SetContext(Ctx ctx)
        {
            _onInterraction = ctx.onInterraction;
            ctx.onCrash.Subscribe(OnCrash).AddTo(this);
            ctx.onRespawned.Subscribe(OnRespawned).AddTo(this);
        }

        private void OnRespawned()
        {
            _isChecking = true;
        }

        private void OnCrash(GameObject o)
        {
            _isChecking = false;
        }

        private async void CheckForInteractionProcessAsync()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(_updateFrequencyMillisec));

                if (!_isChecking) continue;

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
            }
        }
    }
}