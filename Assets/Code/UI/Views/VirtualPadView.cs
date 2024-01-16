using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Views
{
    /// <summary>
    /// Панель, которая принимает в себя пользовательский ввод, проверяет его
    /// соответствие заданным условиям и передает отфильтрованный ввод в реактивной команде
    /// </summary>
    public class VirtualPadView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField, Range(1,4)] private float _distanceThreshold = 2.2f;

        private Vector2 _prevPosition;
        private Vector2 _currentPosition;

        private ReactiveCommand<(Vector2 prevPosition, Vector2 currentPosition)> _onSwipeRaw;

        public struct Ctx
        {
            public ReactiveCommand<(Vector2, Vector2)> onSwipeRaw;
        }

        public void SetContext(Ctx ctx)
        {
            _onSwipeRaw = ctx.onSwipeRaw;
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            var distance = Vector3.Distance(_currentPosition, _prevPosition);

            var normalizedDistance = distance / Screen.width * 10;
            
            if (normalizedDistance < _distanceThreshold) return;

            _onSwipeRaw?.Execute((_prevPosition, _currentPosition));
            _prevPosition = _currentPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _currentPosition = eventData.position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _prevPosition = eventData.position;
            _currentPosition = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _prevPosition = Vector2.zero;
            _currentPosition = Vector2.zero;
        }
    }
}