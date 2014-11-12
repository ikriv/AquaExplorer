using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AquaExplorer.Util
{
    static class TaskUtil
    {
        public static Task OnSuccess<T>(this Task<T> task, Action<T> handler)
        {
            return task.ContinueWith(t=>handler(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static Task OnError(this Task task, Action<Exception> handler)
        {
            return task.ContinueWith(t=>handler(t.Exception), TaskContinuationOptions.NotOnRanToCompletion);
        }

        public static IList<T> ToList<T>(this IEnumerable<T> source, CancellationToken token, bool throwOnCancel = false)
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                if (token.IsCancellationRequested)
                {
                    if (throwOnCancel) throw new TaskCanceledException(); else return list;
                }

                list.Add(item);
            }
            return list;
        }
    }
}
