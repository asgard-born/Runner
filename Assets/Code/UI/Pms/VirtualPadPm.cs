using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace UI.Pms
{
    public class VirtualPadPm : BaseDisposable
    {
        private Vector2 _playerInputData;
        private ReactiveCommand<SwipeDirection> _onInputUpdated;

        public struct Ctx
        {
            public ReactiveCommand<SwipeDirection> onMovementUpdated;
            public ReactiveCommand<(Vector2, Vector2)> onSwipeRaw;
        }

        public VirtualPadPm(Ctx ctx)
        {
            AddUnsafe(ctx.onSwipeRaw.Subscribe(UpdateInput));
            
            _onInputUpdated = ctx.onMovementUpdated;
        }

        private void UpdateInput((Vector2 previous, Vector2 current) positions)
        {
            _playerInputData = positions.current - positions.previous;

            SwipeDirection direction;

            if (_playerInputData.x > _playerInputData.y)
            {
                direction = _playerInputData.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {
                direction = _playerInputData.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            }

            _onInputUpdated?.Execute(direction);
        }
    }
}