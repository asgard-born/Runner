using System;

namespace Framework.Async
{
  internal interface IDisposableAwaiter : IAwaiter, IDisposable
  {
    
  }
  internal interface IDisposableAwaiter<out T> : IAwaiter<T>, IDisposableAwaiter
  {
    
  }
}