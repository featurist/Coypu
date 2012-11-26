﻿using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_interacting_with_dialogs : DriverSpecs
    {
        [Test]
        public void Accepts_alerts()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger an alert", Root));
                Driver.HasDialog("You have triggered an alert and this is the text.", Root).should_be_true();
                Driver.AcceptModalDialog(Root);
                Driver.HasDialog("You have triggered an alert and this is the text.", Root).should_be_false();
            }
        }


        [Test]
        public void Clears_dialog()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger a confirm", Root));
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).should_be_true();
                Driver.AcceptModalDialog(Root);
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).should_be_false();
            }
        }

        [Test]
        public void Returns_true()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger a confirm", Root));
                Driver.AcceptModalDialog(Root);
                Driver.FindLink("Trigger a confirm - accepted", Root).should_not_be_null();
            }
        }


        [Test]
        public void Cancel_Clears_dialog()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger a confirm", Root));
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).should_be_true();
                Driver.CancelModalDialog(Root);
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).should_be_false();
            }
        }

        [Test]
        public void Cancel_Returns_false()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger a confirm", Root));
                Driver.CancelModalDialog(Root);
                Driver.FindLink("Trigger a confirm - cancelled", Root).should_not_be_null();
            }
        }


        [Test]
        public void Finds_scope_first_for_alerts()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root), Driver, null, null, null);
                Assert.That(Driver.HasContent("I am a pop up window", popUp), Is.True);

                Driver.ExecuteScript("window.setTimeout(function() {document.getElementById('alertTriggerLink').click();},500);", Root);
                Assert.That(Driver.HasContent("I am a pop up window", popUp), Is.True);
                Driver.ExecuteScript("self.close();", popUp);

                System.Threading.Thread.Sleep(500);
                Driver.AcceptModalDialog(Root);
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).should_be_false();
            }
        }

        [Test]
        public void Finds_scope_first_for_confirms()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root), Driver, null, null, null);
                Assert.That(Driver.HasContent("I am a pop up window", popUp), Is.True);

                Driver.ExecuteScript("window.setTimeout(function() {document.getElementById('confirmTriggerLink').click();},500);", Root);
                Assert.That(Driver.HasContent("I am a pop up window", popUp), Is.True);
                Driver.ExecuteScript("self.close();", popUp);

                System.Threading.Thread.Sleep(500);
                Driver.CancelModalDialog(Root);
                Driver.HasDialog("You have triggered a alert and this is the text.", Root).should_be_false();
            }
        }
    }
}