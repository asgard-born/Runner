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

        private bool _isChecking = true;
        private ReactiveCommand<Collider> _onInterraction;

        public Collider collider => _collider;
        public Rigidbody rigidbody => _rigidbody;
        public Animator animator => _animator;

        public struct Ctx
        {
            public ReactiveCommand<Collider> onInterraction;
            public ReactiveCommand<GameObject> onInteractedWIthObstacle;
            public ReactiveTrigger onRespawned;
        }

        public void SetContext(Ctx ctx)
        {
            _onInterraction = ctx.onInterraction;
            ctx.onInteractedWIthObstacle.Subscribe(onInteractedWIthObstacle).AddTo(this);
            ctx.onRespawned.Subscribe(OnRespawned).AddTo(this);
        }

        private void OnRespawned()
        {
            _isChecking = true;
        }

        private void onInteractedWIthObstacle(GameObject _)
        {
            _isChecking = false;
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            _onInterraction?.Execute(otherCollider);
        }
    }
}