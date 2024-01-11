using System.Runtime.CompilerServices;

namespace Framework.Async
{
  public interface IAwaiter : INotifyCompletion
  {
    bool IsCompleted { get; }
    void GetResult();
    IAwaiter GetAwaiter();
  }

  public interface IAwaiter<out T> : IAwaiter
  {
    new T GetResult();
    new IAwaiter<T> GetAwaiter();
  }
}