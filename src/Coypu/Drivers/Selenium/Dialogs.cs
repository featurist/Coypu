﻿using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class Dialogs
    {
        private readonly IWebDriver _selenium;

        public Dialogs(IWebDriver selenium)
        {
            _selenium = selenium;
        }

        public bool HasDialog(string withText)
        {
            if (!HasAnyDialog() ) return false;

            var alert = _selenium.SwitchTo().Alert();

            return alert.Text == withText;
        }

        public bool HasAnyDialog()
        {
            try
            {
                return _selenium.SwitchTo() != null &&
                       _selenium.SwitchTo()
                                .Alert() != null;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        public void AcceptModalDialog(string prompt = null)
        {
            try
            {
                var alert = _selenium.SwitchTo()
                         .Alert();
                if (prompt != null) {
                    alert.SendKeys(prompt);
                }
                alert.Accept();
            }
            catch (NoAlertPresentException ex)
            {
                throw new MissingDialogException("No dialog was present to accept", ex);
            }
        }

        public void CancelModalDialog()
        {
            try
            {
                _selenium.SwitchTo()
                         .Alert()
                         .Dismiss();
            }
            catch (NoAlertPresentException ex)
            {
                throw new MissingDialogException("No dialog was present to accept", ex);
            }
        }
    }
}
