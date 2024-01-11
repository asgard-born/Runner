using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework.Async
{
    public static class AsyncExtensions
    {
        public static async void DoAsync(this Task task)
        {
            await task;
        }

        public static int TimeScaled(this int time)
        {
            return (int)(time / Time.timeScale);
        }

        public static async Task WaitForCondition(Func<bool> predicate, int timeoutSeconds, StackTrace trace = null)
        {
            if (timeoutSeconds <= 0)
            {
                timeoutSeconds = int.MaxValue;
            }
            
            DateTime startTime = DateTime.Now;

            bool isErrorTriggered = false;
            while (!predicate.Invoke())
            {
                if ((DateTime.Now - startTime).Seconds >= timeoutSeconds && !isErrorTriggered)
                {
                    UnityEngine.Debug.LogError(
                        "AsyncExtensions, wait for result timeout, something went wrong, trace:\n" + trace
                        );
                    isErrorTriggered = true;
                }

                await Task.Yield();
            }
        }

        public static async Task WaitAllAndReportProgress(List<Task> tasks, Func<int, Task> progressPercentageFnc)
        {
            if (tasks.Count == 0)
            {
                await WaitForUserTask(100);
                return;
            }

            int totalCount = tasks.Count;

            List<Task> tasksQuery = tasks.ToList();

            int completedCount = 0;
            while (tasksQuery.Any())
            {
                Task finishedTask = await Task.WhenAny(tasksQuery);
                tasksQuery.Remove(finishedTask);
                completedCount++;

                int percentage = 100 * completedCount / totalCount;
                await WaitForUserTask(percentage);
                await Task.Yield();
            }
            
            async Task WaitForUserTask(int percentage)
            {
                Task fncTask = progressPercentageFnc.Invoke(percentage);
                if (fncTask != null)
                {
                    await fncTask;
                }
            }
        }
    }
}