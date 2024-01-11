using System;
using UniRx;

namespace Framework.Async
{
  internal struct NextFrameAwaiter : IAwaiter
  {
    public void OnCompleted(Action continuation)
    {
      if (continuation == null)
      {
        return;
      }
      IsCompleted = true;
      MainThreadDispatcher.Post(_ => continuation(), null);
    }

    public bool IsCompleted { get; private set; }

    public void GetResult()
    {
      
    }

    public IAwaiter GetAwaiter()
      => this;
  }
}