﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;
using Cookie = System.Net.Cookie;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Data.Common;
using OpenQA.Selenium.DevTools.V85.Network;

#pragma warning disable 1591

namespace Coypu.Drivers.Playwright
{
    public class PlaywrightDriver : IDriver
    {
        private readonly Browser _browser;
        private readonly IPlaywright _playwright;
        private readonly IBrowser _playwrightBrowser;
        private readonly IPage _page;

        public PlaywrightDriver(Browser browser)
        {
            _browser = browser;
            _playwright = Async.WaitForResult(Microsoft.Playwright.Playwright.CreateAsync());

            var browserType = _playwright.Chromium; // TODO: map browser to playwright browser type

            _playwrightBrowser = Async.WaitForResult(browserType.LaunchAsync(
              new BrowserTypeLaunchOptions{Headless = false}
            ));
            _page = Async.WaitForResult(_playwrightBrowser.NewPageAsync());
        }
        protected bool NoJavascript => !_browser.Javascript;

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
          throw new Exception("Not implemented");
          // _elementFinder.SeleniumScope(scope);
          // return new Uri(_page.Url);
        }

        public string Title(Scope scope)
        {
          throw new Exception("Not implemented");
          // _elementFinder.SeleniumScope(scope);
          // return _page.Title;
        }

        public Cookies Cookies { get; set; }
        public object Native => _playwright;

      public Element Window => new PlaywrightWindow(_playwrightBrowser, _page);

      public IEnumerable<Element> FindFrames(string locator,
                                             Scope scope,
                                             Options options)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            return Async.WaitForResult(Element(scope).QuerySelectorAllAsync($"css={cssSelector}"))
                        .Select(BuildElement);
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            return Async.WaitForResult(Element(scope).QuerySelectorAllAsync($"xpath={xpath}"))
                        .Select(BuildElement);
        }

        private Element BuildElement(IElementHandle element)
        {
            var tagName = Async.WaitForResult(element.EvaluateAsync("e => e.tagName"))?.GetString();

            return new[] {"iframe", "frame"}.Contains(tagName.ToLower())
                     ? null  // TODO:  .new SeleniumFrame(element, _playwright, _seleniumWindowManager)
                     : new PlaywrightElement(element, _playwright);
        }

        public bool HasDialog(string withText,
                              Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(string url,
                          Scope scope)
        {
            IResponse response = Async.WaitForResult(_page.GotoAsync(url));
            if (response.Status != 200)
            {
              throw new Exception("Failed to load page");
            }
        }

        public void ClearBrowserCookies()
        {
            Cookies.DeleteAll();
        }

        public void Click(Element element)
        {
            Async.WaitForResult(PlaywrightElement(element).ClickAsync());
        }

        public void Hover(Element element)
        {
            throw new NotImplementedException();
        }

        public void SendKeys(Element element,
                             string keys)
        {
            throw new NotImplementedException();
        }

        public void MaximiseWindow(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Refresh(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void ResizeTo(Size size,
                             Scope scope)
        {
            throw new NotImplementedException();
        }

        public void SaveScreenshot(string fileName,
                                   Scope scope)
        {
            throw new NotImplementedException();
        }

        public void GoBack(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void GoForward(Scope scope)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Element> FindWindows(string titleOrName,
                                                Scope scope,
                                                Options options)
        {
            throw new NotImplementedException();
        }

        public void Set(Element element,
                        string value)
        {
            throw new NotImplementedException();
        }

        public void AcceptModalDialog(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void CancelModalDialog(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Check(Element field)
        {
            if (!field.Selected)
                Async.WaitForResult(PlaywrightElement(field).CheckAsync());
        }

        public void Uncheck(Element field)
        {
            if (field.Selected)
                Async.WaitForResult(PlaywrightElement(field).UncheckAsync());
        }

        public void Choose(Element field)
        {
            Async.WaitForResult(PlaywrightElement(field).CheckAsync());
        }

        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            throw new NotImplementedException();
        }
        private IElementHandle PlaywrightElement(Element element)
        {
            return (IElementHandle) element.Native;
        }

        private IElementHandle Element(Scope scope)
        {
            var scopeElement = scope.Now();
            return scopeElement.Native is IPage
              ? Async.WaitForResult(((IPage) scopeElement.Native).QuerySelectorAsync("html"))
              : (IElementHandle) scopeElement.Native;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            if (disposing)
            {
                // AcceptAnyAlert(); ??
            }

            Disposed = true;
        }
    }
}