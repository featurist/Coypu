﻿using System;
using System.Diagnostics;
using System.Threading;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu.Robustness
{
    public class RetryUntilTimeoutRobustWrapper : RobustWrapper
    {
        public void TryUntil(BrowserAction tryThis, PredicateQuery until, TimeSpan overrallTimeout, TimeSpan waitBeforeRetry)
        {
            var outcome = Robustly(new ActionSatisfiesPredicateQuery(tryThis, until, overrallTimeout, until.RetryInterval, waitBeforeRetry, this));
            if (!outcome)
                throw new MissingHtmlException("Timeout from TryUntil: the page never reached the required state.");
        }

        public bool ZeroTimeout { get; set; }
        private TimeSpan? overrideTimeout;

        public void SetOverrideTimeout(TimeSpan timeout)
        {
            overrideTimeout = timeout;
        }

        public void ClearOverrideTimeout()
        {
            overrideTimeout = null;
        }

        public virtual TResult Robustly<TResult>(Query<TResult> query)
        {
            var interval = query.RetryInterval;
            var timeout = Timeout(query);
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    var result = query.Run();
                    if (ExpectedResultNotFoundWithinTimeout(query.ExpectedResult, result, stopWatch, timeout, interval))
                    {
                        WaitForInterval(interval);
                        continue;
                    }
                    return result;
                }
                catch (NotSupportedException) { throw; }
                catch (Exception)
                {
                    if (TimeoutReached(stopWatch, timeout, interval))
                    {
                        throw;
                    }
                    WaitForInterval(interval);
                }
            }
        }

        protected TimeSpan Timeout<TResult>(Query<TResult> query)
        {
            TimeSpan timeout;
            if (ZeroTimeout)
            {
                timeout = TimeSpan.Zero;
            }
            else if (overrideTimeout.HasValue)
            {
                timeout = overrideTimeout.Value;
            }
            else
            {
                timeout = query.Timeout;
            }
            return timeout;
        }

        protected void WaitForInterval(TimeSpan interval)
        {
            Thread.Sleep(interval);
        }

        protected bool ExpectedResultNotFoundWithinTimeout<TResult>(object expectedResult, TResult result, Stopwatch stopWatch, TimeSpan timeout, TimeSpan interval)
        {
            return expectedResult != null && !result.Equals(expectedResult) && !TimeoutReached(stopWatch, timeout, interval);
        }

        protected bool TimeoutReached(Stopwatch stopWatch, TimeSpan timeout, TimeSpan interval)
        {
            var elapsedTimeToNextCall = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds) + interval;
            var timeoutReached = elapsedTimeToNextCall >= timeout;

            return timeoutReached;
        }
    }
}