﻿using System;
using Coypu.Finders;
using NUnit.Framework;
using System.Threading;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_location : DriverSpecs
    {
        [Test]
        public void Go_back_and_forward_in_history()
        {
            using (Driver)
            {
                Driver.Visit(TestSiteUrl("/"), Root);
                Driver.Visit(TestSiteUrl("/auto_login"), Root);

                Driver.GoBack(Root);
                Assert.That(Driver.Location(Root), Is.EqualTo(new Uri(TestSiteUrl("/"))));

                Driver.GoForward(Root);
                Assert.That(Driver.Location(Root), Is.EqualTo(new Uri(TestSiteUrl("/auto_login"))));

            }
        }

        [Test]
        public void Go_back_and_forward_in_correct_window_scope()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                    Driver, null, null, null, DisambiguationStrategy);

                Driver.Visit(TestSiteUrl("/auto_login"), Root);
                Driver.Visit(TestSiteUrl("/"), popUp);

                Driver.GoBack(popUp);
                // Linux Chromedriver is too fast it barfs no url on location query
                Thread.Sleep(100);

                Assert.That(Driver.Location(popUp).AbsoluteUri,
                            Is.StringEnding("src/Coypu.Drivers.Tests/html/popup.htm"));
                Assert.That(Driver.Location(Root).AbsoluteUri, Is.EqualTo(TestSiteUrl("/auto_login")));

                Driver.GoForward(popUp);
                // Linux Chromedriver is too fast it barfs no url on location query
                Thread.Sleep(100);

                Assert.That(Driver.Location(popUp).AbsoluteUri, Is.EqualTo(TestSiteUrl("/")));

                Driver.GoBack(Root);
                // Linux Chromedriver is too fast it barfs no url on location query
                Thread.Sleep(100);

                Assert.That(Driver.Location(Root).AbsoluteUri, Is.StringEnding("/html/InteractionTestsPage.htm"));
                Assert.That(Driver.Location(popUp).AbsoluteUri, Is.EqualTo(TestSiteUrl("/")));
            }
        }
    }
}