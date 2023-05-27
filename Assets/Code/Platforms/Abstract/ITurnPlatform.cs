using UnityEngine;

namespace Code.Platforms.Abstract
{
    public interface ITurnPlatform
    {
        Transform firstTransform { get; }
        Transform lastPartTransform { get; }
    }
}