﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_all_matching_elements : BrowserInteractionTests
    {
        [Test, Ignore("Didn't work from original fork")]
        public void FindAllCss_should_make_direct_call_to_underlying_driver()
        {
            Should_make_direct_call_when_no_predicate_supplied(browserSession.FindAllCss, driver.StubAllCss);
        }

        [Test, Ignore("Didn't work from original fork")]
        public void FindAllCss_wrap_each_element_found_in_a_snapshot_scope_with()
        {
            Should_wrap_elements_in_snapshot_scope(browserSession.FindAllCss, driver.StubAllCss);
        }

        [Test]
        public void FindAllCss_when_predicate_supplied_retries_until_it_passes()
        {
            Should_test_predicate_against_query_results_and_retry_on_failure(browserSession.FindAllCss,
                                                                             driver.StubAllCss);
        }

        [Test, Ignore("Didn't work from original fork")]
        public void FindAllXPath_should_make_direct_call_to_underlying_driver()
        {
            Should_make_direct_call_when_no_predicate_supplied(browserSession.FindAllXPath, driver.StubAllXPath);
        }

        [Test, Ignore("Didn't work from original fork")]
        public void FindAllXPath_wrap_each_element_found_in_a_snapshot_scope_with()
        {
            Should_wrap_elements_in_snapshot_scope(browserSession.FindAllXPath, driver.StubAllXPath);
        }

        [Test]
        public void FindAllXPath_when_predicate_supplied_retries_until_it_passes()
        {
            Should_test_predicate_against_query_results_and_retry_on_failure(browserSession.FindAllXPath,
                                                                             driver.StubAllXPath);
        }

        protected void Should_make_direct_call_when_no_predicate_supplied(Func<string, Func<IEnumerable<SnapshotElementScope>, bool>, Options, IEnumerable<SnapshotElementScope>> subject, Action<string, IEnumerable<Element>, DriverScope, Options> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var stubElements = new[] { new StubElement() };

            stub(locator, stubElements, browserSession, new Options());

            subject(locator, null, sessionConfiguration);

            Assert.That(SpyTimingStrategy.NoQueriesRan, Is.True, "Expected no robust queries run");
        }

        private void Should_wrap_elements_in_snapshot_scope(Func<string, Func<IEnumerable<SnapshotElementScope>, bool>, Options, IEnumerable<SnapshotElementScope>> subject, Action<string, IEnumerable<Element>, DriverScope, Options> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var stubElements = new[] {new StubElement(), new StubElement()};

            stub(locator, stubElements, browserSession, new Options());

            var actualImmediateResult = subject(locator, null, sessionConfiguration).ToArray();
            Assert.That(actualImmediateResult.Count(), Is.EqualTo(2));
            Assert.That(actualImmediateResult[0].Now(), Is.EqualTo(stubElements[0]));
            Assert.That(actualImmediateResult[1].Now(), Is.EqualTo(stubElements[1]));
        }

        protected void Should_test_predicate_against_query_results_and_retry_on_failure(Func<string, Func<IEnumerable<SnapshotElementScope>,bool>, Options, IEnumerable<SnapshotElementScope>> subject, Action<string, IEnumerable<Element>, Scope, Options> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            var expectedImmediateResult = new[] { new StubElement(), new StubElement() };
            var expectedDeferredResult = new[] { new StubElement(), new StubElement() };

            var options = new Options{Timeout = individualTimeout};
            SpyTimingStrategy.AlwaysReturnFromSynchronise(expectedImmediateResult.Select(e => new SnapshotElementScope(e, browserSession, options)));

            stub(locator, expectedDeferredResult, browserSession, options);

            VerifyFoundRobustly(subject, 0, locator, (elements) => true, expectedDeferredResult, expectedImmediateResult, options);
            VerifyFoundRobustlyAndThrows<MissingHtmlException>(subject, 1, locator, (elements) => false, null, expectedImmediateResult, options);
            VerifyFoundRobustlyAndThrows<TestException>(subject, 2, locator, (elements) => { throw new TestException("Thrown in FindAll predicate"); }, null, expectedImmediateResult, options);
        }

        private void VerifyFoundRobustlyAndThrows<T>(Func<string, Func<IEnumerable<SnapshotElementScope>, bool>, Options, IEnumerable<SnapshotElementScope>> subject, int driverCallIndex, string locator, Func<IEnumerable<SnapshotElementScope>, bool> predicate, IEnumerable<Element> expectedDeferredResult, IEnumerable<Element> expectedImmediateResult, Options options) 
            where T: Exception
        {
            Assert.Throws<T>(
                () => VerifyFoundRobustly(subject, driverCallIndex, locator, predicate, expectedDeferredResult,
                                          expectedImmediateResult, options));
        }

        protected void VerifyFoundRobustly(Func<string, Func<IEnumerable<SnapshotElementScope>, bool>, Options, IEnumerable<SnapshotElementScope>> subject, int driverCallIndex, string locator, Func<IEnumerable<SnapshotElementScope>, bool> predicate, IEnumerable<Element> expectedDeferredResult, IEnumerable<Element> expectedImmediateResult, Options options)
        {
            var scopedResult = subject(locator, predicate, options);
            var elementScopeResult = RunQueryAndCheckTiming<IEnumerable<SnapshotElementScope>>(options.Timeout, driverCallIndex);

            Assert.That(scopedResult.Select(e => e.Now()), Is.Not.EquivalentTo(expectedDeferredResult), "Result was not found robustly");
            Assert.That(scopedResult.Select(e => e.Now()), Is.EquivalentTo(expectedImmediateResult));

            Assert.That(elementScopeResult.Select(e => e.Now()), Is.EquivalentTo(expectedDeferredResult));
        }
    }
}