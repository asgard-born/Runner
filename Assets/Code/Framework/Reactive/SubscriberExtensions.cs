using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Reactive
{
  internal static class SubscriberExtensions
  {
    public static int SumCount(this Dictionary<Type, int> counter)
    {
      int sum = counter.Sum(_ => _.Value);
      return sum;
    }
  }
}