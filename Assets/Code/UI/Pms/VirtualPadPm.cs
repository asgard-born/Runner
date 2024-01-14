using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace UI.Pms
{
    public class VirtualPadPm : BaseDisposable
    {
        private Vector2 _playerInputData;
        private ReactiveCommand<Direction> _onSwipeDirection;

        public struct Ctx
        {
            public ReactiveCommand<Direction> onSwipeDirection;
            public ReactiveCommand<(Vector2, Vector2)> onSwipeRaw;
        }

        public VirtualPadPm(Ctx ctx)
        {
            AddUnsafe(ctx.onSwipeRaw.Subscribe(UpdateInput));
            
            _onSwipeDirection = ctx.onSwipeDirection;
        }

        private void UpdateInput((Vector2 previous, Vector2 current) positions)
        {
            _playerInputData = positions.current - positions.previous;

            Direction direction;

            if (Mathf.Abs(_playerInputData.x) > Mathf.Abs(_playerInputData.y))
            {
                direction = _playerInputData.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                direction = _playerInputData.y > 0 ? Direction.Up : Direction.Down;
            }

            _onSwipeDirection?.Execute(direction);
        }
    }
}