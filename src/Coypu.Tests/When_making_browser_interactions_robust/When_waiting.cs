using System;
using System.Diagnostics;
using Coypu.Timing;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_waiting
    {
        public const int AccuracyMilliseconds = 40;

        [Test]
        public void It_sleeps_for_the_expected_time_Case_1()
        {
            It_sleeps_for_the_expected_time(100);
        }

        [Test]
        public void It_sleeps_for_the_expected_time_Case_2()
        {
            It_sleeps_for_the_expected_time(200);
        }

        public void It_sleeps_for_the_expected_time(int expectedDurationMilliseconds)
        {
            var waiter = new StopwatchWaiter();
            var stopWatch = Stopwatch.StartNew();
            var expectedDuration = TimeSpan.FromMilliseconds(expectedDurationMilliseconds);

            waiter.Wait(expectedDuration);

            var actualWait = stopWatch.ElapsedMilliseconds;

            const int toleranceMilliseconds = AccuracyMilliseconds;

            Assert.That((int)actualWait, Is.InRange(expectedDurationMilliseconds - toleranceMilliseconds,
                                               expectedDurationMilliseconds + toleranceMilliseconds));
        }

    }
}
