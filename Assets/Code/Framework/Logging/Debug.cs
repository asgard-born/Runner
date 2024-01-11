using System;
using System.Diagnostics;
using Object = UnityEngine.Object;
using UDebug = UnityEngine.Debug;

namespace Framework.Logging
{
    public class Debug
    {
        private readonly Debug _parent;
        private readonly string _name;
        private readonly string _fullName;

        public Debug(string name, Debug parent = null)
        {
            _name = name;
            _parent = parent;
            _fullName = $"{FullName}: ";
        }

        private string FullName => (_parent == null ? string.Empty : _parent.FullName + '.') + _name;

        [Conditional("DEBUG_ENABLE_LOG")]
        public static void Log(object message, Object obj = null)
        {
            UDebug.Log(message, obj);
        }

        [Conditional("DEBUG_ENABLE_LOG")]
        public static void LogWarning(object message, Object obj = null)
        {
            UDebug.LogWarning(message, obj);
        }

        [Conditional("DEBUG_ENABLE_LOG")]
        public static void LogError(object message, Object obj = null)
        {
            UDebug.LogError(message, obj);
        }

        [Conditional("DEBUG_ENABLE_LOG")]
        public static void LogException(Exception exception, Object obj = null)
        {
            UDebug.LogException(exception, obj);
        }

        [Conditional("DEBUG_ENABLE_LOG")]
        public void Ex(Exception ex, Object obj = null)
        {
            UDebug.LogError(_fullName + ex, obj);
        }
    }
}