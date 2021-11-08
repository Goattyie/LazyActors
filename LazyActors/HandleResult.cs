using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActor
{
    public class HandleResult
    {
        public static HandleResult OK() => OkHandleResult.Instance;
        public static Task<HandleResult> OkTask() => OkHandleResult.Task;
        public static HandleResult Retry() => NeedRetry.Instance;
        public static Task<HandleResult> RetryTask() => NeedRetry.Task;
        public static HandleResult Retry(TimeSpan delay) => new NeedRetryWithDelay(delay);
        public static Task<HandleResult> RetryTaskWithDilay(TimeSpan delay) => Task.FromResult(Retry(delay));
    }
    internal class OkHandleResult : HandleResult
    {
        public static readonly OkHandleResult Instance = new OkHandleResult();
        public static readonly Task<HandleResult> Task = System.Threading.Tasks.Task.FromResult((HandleResult)Instance);
    }

    internal class NeedRetry : HandleResult
    {
        public static readonly NeedRetry Instance = new NeedRetry();
        public static readonly Task<HandleResult> Task = System.Threading.Tasks.Task.FromResult((HandleResult)Instance);
    }
    internal class NeedRetryWithDelay : HandleResult
    {
        public TimeSpan Delay { get; }
        public NeedRetryWithDelay(TimeSpan delay)
        {
            Delay = delay;
        }
    }

}
