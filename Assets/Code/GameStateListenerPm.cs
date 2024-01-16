using System;
using Cysharp.Threading.Tasks;
using Framework;
using Framework.Reactive;
using UniRx;

/// <summary>
/// Слушает игровое состояние и оповещает о ключевых событиях игрового процесса
/// Управляет началом игры, выигрышем и проигрышем
/// </summary>
public class GameStateListenerPm : BaseDisposable
{
    private readonly Ctx _ctx;

    public struct Ctx
    {
        public float startDelaySec;

        public ReactiveProperty<int> lives;

        public ReactiveTrigger onGameInitialized;
        public ReactiveTrigger onGameRun;
        public ReactiveTrigger onGameOver;
        public ReactiveTrigger onGameWin;
        public ReactiveTrigger onFinishZoneReached;
    }

    public GameStateListenerPm(Ctx ctx)
    {
        _ctx = ctx;

        AddUnsafe(_ctx.onFinishZoneReached.Subscribe(SetWinState));
        AddUnsafe(_ctx.onGameInitialized.Subscribe(OnInitialized));
        AddUnsafe(_ctx.lives.Subscribe(OnLivesChanged));
    }

    private void SetWinState()
    {
        _ctx.onGameWin?.Notify();
    }

    private void OnLivesChanged(int livesLeft)
    {
        if (livesLeft <= 0)
        {
            _ctx.onGameOver?.Notify();
        }
    }

    private async void OnInitialized()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_ctx.startDelaySec));

        _ctx.onGameRun?.Notify();
    }
}