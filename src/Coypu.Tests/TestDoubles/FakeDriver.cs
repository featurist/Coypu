﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu.Tests.TestDoubles
{
    public class FakeDriver : Driver
    {
        public readonly IList<Element> ClickedElements = new List<Element>();
        public readonly IList<Element> HoveredElements = new List<Element>();
        public readonly IList<Element> CheckedElements = new List<Element>();
        public readonly IList<Element> UncheckedElements = new List<Element>();
        public readonly IList<Element> ChosenElements = new List<Element>();
        public readonly IList<string> HasContentQueries = new List<string>();
        public readonly IList<Regex> HasContentMatchQueries = new List<Regex>();
        public readonly IList<string> HasCssQueries = new List<string>();
        public readonly IList<string> HasXPathQueries = new List<string>();
        public readonly IList<string> Visits = new List<string>();
        public readonly IDictionary<Element, SetFieldParams> SetFields = new Dictionary<Element, SetFieldParams>();
        public readonly IDictionary<Element, string> SentKeys = new Dictionary<Element, string>();
        public readonly IDictionary<Element, string> SelectedOptions = new Dictionary<Element, string>();
        private readonly IList<ScopedStubResult> stubbedButtons = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedLinks = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedTextFields = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedAllCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedAllXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedExecuteScriptResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedFieldsets = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedSections = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedFrames = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedIDs = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasContentResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasContentMatchResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasDialogResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedWindows = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedLocations = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedTitles = new List<ScopedStubResult>();
        public readonly IList<string> FindButtonRequests = new List<string>();
        public readonly IList<string> FindLinkRequests = new List<string>();
        public readonly IList<string> FindCssRequests = new List<string>();
        private IList<Cookie> stubbedCookies;

        public List<Scope> ModalDialogsAccepted = new List<Scope>();
        public List<Scope> ModalDialogsCancelled = new List<Scope>();


        public FakeDriver() {}
        public FakeDriver(Drivers.Browser browser)
        {
            Browser = browser;
        }

        
        class ScopedStubResult
        {
            public object Locator;
            public object Result;
            public Scope Scope;
        }


        public Drivers.Browser Browser { get; private set; }

        public ElementFound FindButton(string locator, Scope scope)
        {
            FindButtonRequests.Add(locator);
            return Find<ElementFound>(stubbedButtons,locator,scope);
        }

        private T Find<T>(IEnumerable<ScopedStubResult> stubbed, object locator, Scope scope)
        {
            var scopedStubResult = stubbed.FirstOrDefault(r => r.Locator == locator && r.Scope == scope);
            if (scopedStubResult == null)
            {
                throw new MissingHtmlException("Element not found: " + locator);
            }
            return (T) scopedStubResult.Result;
        }

        public ElementFound FindLink(string linkText, Scope scope)
        {
            FindLinkRequests.Add(linkText);

            return Find<ElementFound>(stubbedLinks, linkText, scope);
        }

        public ElementFound FindField(string locator, Scope scope)
        {
            return Find<ElementFound>(stubbedTextFields, locator, scope);
        }

        public void Click(Element element)
        {
            ClickedElements.Add(element);
        }

        public void Hover(Element element)
        {
            HoveredElements.Add(element);
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return stubbedCookies;
        }

        public void SetBrowserCookies(Cookie cookie)
        {
        }

        public void Visit(string url)
        {
            Visits.Add(url);
        }

        public void StubButton(string locator, ElementFound element, Scope scope)
        {
            stubbedButtons.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = element});
        }

        public void StubLink(string locator, ElementFound element, Scope scope)
        {
            stubbedLinks.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = element});
        }

        public void StubField(string locator, ElementFound element, Scope scope)
        {
            stubbedTextFields.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = element});
        }

        public void StubHasContent(string text, bool result, Scope scope)
        {
            stubbedHasContentResults.Add(new ScopedStubResult { Locator = text, Scope = scope, Result = result });
        }

        public void StubHasContentMatch(Regex pattern, bool result, Scope scope)
        {
            stubbedHasContentMatchResults.Add(new ScopedStubResult { Locator = pattern, Scope = scope, Result = result });
        }

        public void StubHasCss(string cssSelector, bool result, Scope scope)
        {
            stubbedHasCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubHasXPath(string xpath, bool result, Scope scope)
        {
            stubbedHasXPathResults.Add(new ScopedStubResult { Locator = xpath, Scope = scope, Result = result });
        }

        public void StubDialog(string text, bool result, Scope scope)
        {
            stubbedHasDialogResults.Add(new ScopedStubResult { Locator = text, Scope = scope, Result = result });
        }

        public void StubCss(string cssSelector, ElementFound result, Scope scope)
        {
            stubbedCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubXPath(string cssSelector, ElementFound result, Scope scope)
        {
            stubbedXPathResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubAllCss(string cssSelector, IEnumerable<ElementFound> result, Scope scope)
        {
            stubbedAllCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubAllXPath(string xpath, IEnumerable<ElementFound> result, Scope scope)
        {
            stubbedAllXPathResults.Add(new ScopedStubResult { Locator = xpath, Scope = scope, Result = result });
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
            return Find<Uri>(stubbedLocations, null, scope);
        }

        public string Title(Scope scope)
        {
            return Find<String>(stubbedTitles, null, scope);
        }

        public ElementFound Window
        {
            get { throw new NotImplementedException(); }
        }

        public void AcceptModalDialog(Scope scope)
        {
            ModalDialogsAccepted.Add(scope);
        }

        public void CancelModalDialog(Scope scope)
        {
            ModalDialogsCancelled.Add(scope);
        }

        public string ExecuteScript(string javascript, Scope scope)
        {
            return Find<string>(stubbedExecuteScriptResults, javascript, scope);
        }

        public ElementFound FindFieldset(string locator, Scope scope)
        {
            return Find<ElementFound>(stubbedFieldsets, locator, scope);
        }

        public ElementFound FindSection(string locator, Scope scope)
        {
            return Find<ElementFound>(stubbedSections, locator, scope);
        }

        public ElementFound FindId(string id, Scope scope)
        {
            return Find<ElementFound>(stubbedIDs, id, scope);
        }

        public ElementFound FindFrame(string locator, Scope scope)
        {
            return Find<ElementFound>(stubbedFrames, locator, scope);
        }

        public void SendKeys(Element element, string keys)
        {
            SentKeys.Add(element, keys);
        }

        public void Set(Element element, string value, bool forceAllEvents)
        {
            SetFields.Add(element, new SetFieldParams{Value = value, ForceAllEvents = forceAllEvents});
        }

        public void Select(Element element, string option)
        {
            SelectedOptions.Add(element, option);
        }

        public object Native
        {
            get { return "Native driver on fake driver"; }
        }

        public bool HasContent(string text, Scope scope)
        {
            HasContentQueries.Add(text);
            return Find<bool>(stubbedHasContentResults, text, scope);
        }

        public bool HasContentMatch(Regex pattern, Scope scope)
        {
            HasContentMatchQueries.Add(pattern);
            return Find<bool>(stubbedHasContentMatchResults, pattern, scope);
        }

        public bool HasCss(string cssSelector, Scope scope)
        {
            return Find<bool>(stubbedHasCssResults, cssSelector, scope);
        }

        public bool HasXPath(string xpath, Scope scope)
        {
            return Find<bool>(stubbedHasXPathResults, xpath, scope);
        }

        public bool HasDialog(string withText, Scope scope)
        {
            return Find<bool>(stubbedHasDialogResults, withText, scope);
        }

        public ElementFound FindCss(string cssSelector, Scope scope)
        {
            FindCssRequests.Add(cssSelector);
            return Find<ElementFound>(stubbedCssResults, cssSelector, scope);
        }

        public ElementFound FindXPath(string xpath, Scope scope)
        {
            return Find<ElementFound>(stubbedXPathResults, xpath, scope);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope)
        {
            return Find<IEnumerable<ElementFound>>(stubbedAllCssResults, cssSelector, scope);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope)
        {
            return Find<IEnumerable<ElementFound>>(stubbedAllXPathResults, xpath, scope);
        }

        public void Check(Element field)
        {
            CheckedElements.Add(field);
        }

        public void Uncheck(Element field)
        {
            UncheckedElements.Add(field);
        }

        public void Choose(Element field)
        {
            ChosenElements.Add(field);
        }

        public void StubExecuteScript(string script, string scriptReturnValue, Scope scope)
        {
            stubbedExecuteScriptResults.Add(new ScopedStubResult{Locator = script, Scope =  scope, Result = scriptReturnValue});
        }

        public void StubFieldset(string locator, ElementFound fieldset, Scope scope)
        {
            stubbedFieldsets.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = fieldset});
        }

        public void StubSection(string locator, ElementFound section, Scope scope)
        {
            stubbedSections.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = section});
        }

        public void StubFrame(string locator, ElementFound frame, Scope scope)
        {
            stubbedFrames.Add(new ScopedStubResult { Locator = locator, Scope = scope, Result = frame });
        }

        public void StubId(string id, ElementFound element, Scope scope)
        {
            stubbedIDs.Add(new ScopedStubResult { Locator = id, Scope = scope, Result = element });
        }

        public void StubCookies(List<Cookie> cookies)
        {
            stubbedCookies = cookies;
        }

        public void StubLocation(Uri location, Scope scope)
        {
            stubbedLocations.Add(new ScopedStubResult {Result = location, Scope = scope});
        }

        public void StubTitle(String title, Scope scope)
        {
            stubbedTitles.Add(new ScopedStubResult { Result = title, Scope = scope });
        }

        public void StubWindow(string locator, ElementFound window, Scope scope)
        {
            stubbedWindows.Add(new ScopedStubResult {Locator = locator, Scope = scope, Result = window});
        }

        public ElementFound FindWindow(string locator, Scope scope)
        {
            return Find<ElementFound>(stubbedWindows, locator, scope);
        }
    }

    public class SetFieldParams
    {
        public bool ForceAllEvents { get; set; }

        public string Value { get; set; }
    }
}