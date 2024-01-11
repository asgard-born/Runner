namespace Nakusi.Reactive.Debug.Editor
{
    public class SubscriptionDebugger
    {
#if SUBSCRIPTIONS_DEBUG
        [MenuItem("Utils/Debug/Subscriptions/TakeSnapshot")]
        public static void TakeSnapshot()
        {
            ObservableExtensions.ObservatorDisposable.DebugTakeSnapshot();
            UnityEngine.Debug.Log("Subscriptions snapshot take");
        }

        [MenuItem("Utils/Debug/Subscriptions/GetDiff")]
        public static void GetDiff()
        {
            UnityEngine.Debug.Log("");
            UnityEngine.Debug.Log("");
            Dictionary<string, int> diff = ObservableExtensions.ObservatorDisposable.DebugGetSnapshotDiff();
            UnityEngine.Debug.Log($"Subscriptions diff, leak count: {diff.Count}");
            UnityEngine.Debug.Log("-------->");
            foreach (KeyValuePair<string, int> diffItem in diff)
            {
                UnityEngine.Debug.Log($"Leaks: {diffItem.Value}, {diffItem.Key}");
            }
            UnityEngine.Debug.Log("<--------");
            UnityEngine.Debug.Log("");
            UnityEngine.Debug.Log("");
        }
#endif
    }
}