﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu
{
    public interface Driver : IDisposable
    {
        ElementFound FindButton(string locator, Scope scope);
        ElementFound FindLink(string linkText, Scope scope);
        ElementFound FindField(string locator, Scope scope);
        void Click(Element element);
        void Visit(string url);
        void Set(Element element, string value, bool forceAllEvents);
        void Select(Element element, string option);
        object Native { get; }
        bool HasContent(string text, Scope scope);
        bool HasContentMatch(Regex pattern, Scope scope);
        bool HasCss(string cssSelector, Scope scope);
        bool HasXPath(string xpath, Scope scope);
        bool HasDialog(string withText, Scope scope);
        ElementFound FindCss(string cssSelector, Scope scope);
        ElementFound FindXPath(string xpath, Scope scope);
        IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope);
        IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope);
        void Check(Element field);
        void Uncheck(Element field);
        void Choose(Element field);
        bool Disposed { get; }
        Uri Location(Scope scope);
        String Title(Scope scope);
        ElementFound Window { get; }
        void AcceptModalDialog(Scope scope);
        void CancelModalDialog(Scope scope);
        string ExecuteScript(string javascript, Scope scope);
        ElementFound FindFieldset(string locator, Scope scoper);
        ElementFound FindSection(string locator, Scope scope);
        ElementFound FindId(string id, Scope scope);
        void Hover(Element element);
        IEnumerable<Cookie> GetBrowserCookies();
        ElementFound FindWindow(string locator, Scope scope);
        ElementFound FindFrame(string locator, Scope scope);
        void SendKeys(Element element, string keys);
    }
}