﻿using System;
using System.Collections.Generic;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;

namespace Coypu
{
    /// <summary>
    /// Global configuration settings
    /// </summary>
    public class SessionConfiguration : Options
    {
        const string DEFAULT_APP_HOST = "localhost";
        const int DEFAULT_PORT = 80;
        
        private string appHost;

        /// <summary>
        /// New default configuration
        /// </summary>
        public SessionConfiguration()
        {
            AppHost = DEFAULT_APP_HOST;
            Port = DEFAULT_PORT;
            SSL = false;
            Browser = Drivers.Browser.Firefox;
            Driver = typeof (SeleniumWebDriver);
            BrowserOptions = new Dictionary<Browser, object>();
        }

        /// <summary>
        /// <para>Specifies the browser you would like to control</para>
        /// <para>Default: Firefox</para>
        /// </summary>
        public Drivers.Browser Browser { get; set; }

        /// <summary>
        /// Specifies the native options for supported browsers (native options objects keyed by <see cref="Browser"/>).
        /// </summary>
        /// <remarks>This mechanism allows you to supply options for native browser types that are specific to the
        /// driver implementation to be used.  For example, Selenium provides "options" classes for some of the supported
        /// browser types (e.g. ChromeOptions, InternetExplorerOptions, PhantomJSOptions).  Adding them to this 
        /// dictionary will make them available to the SeleniumWebDriver implementations.</remarks>
        public IDictionary<Browser, object> BrowserOptions { get; set; }

        /// <summary>
        /// <para>Specifies the driver you would like to use to control the browser</para> 
        /// <para>Default: SeleniumWebDriver</para>
        /// </summary>
        public Type Driver { get; set; }


        /// <summary>
        /// <para>The host of the website you are testing, e.g. 'github.com'</para>
        /// <para>Default: localhost</para>
        /// </summary>
        public string AppHost
        {
            get { return appHost;}
            set { appHost = value == null ? null : value.TrimEnd('/'); }
        }

        /// <summary>
        /// <para>The port of the website you are testing</para>
        /// <para>Default: 80</para>
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// <para>Whether to use the HTTPS protocol to connect to website you are testing</para>
        /// <para>Default: false</para>
        /// </summary>
        public bool SSL { get; set; }
    }
}