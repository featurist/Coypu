﻿using System;
using System.IO;
using Coypu.Drivers.Selenium;
using Coypu.Drivers.Tests.Sites;
using Coypu.Drivers.Watin;
using Coypu.Finders;
using NUnit.Framework;

[SetUpFixture]
public class AssemblyTearDown
{
    private SinatraSite sinatraSite;

    [SetUp]
    public void StartSinatra()
    {
        sinatraSite = new SinatraSite(@"..\..\..\Coypu.AcceptanceTests\sites\site_with_secure_resources.rb");
    }

    [TearDown]
    public void TearDown()
    {
        sinatraSite.Dispose();

        var driver = Coypu.Drivers.Tests.DriverSpecs.Driver;
        if (driver != null && !driver.Disposed)
            driver.Dispose();
    }
}


namespace Coypu.Drivers.Tests
{
    public class DriverSpecs
    {
        private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
        private static DriverScope root;
        private static Driver driver;

        private static readonly Browser browser = Browser.Firefox;
        private static readonly Type driverType = typeof (SeleniumWebDriver);

        [SetUp]
        public virtual void SetUp()
        {
            Driver.Visit(GetTestHTMLPathLocation());
        }

        protected string GetTestHTMLPathLocation()
        {
            var file = new FileInfo(Path.Combine(@"..\..\", TestPage)).FullName;
            return "file:///" + file.Replace('\\', '/');
        }

        protected virtual string TestPage
        {
            get { return INTERACTION_TESTS_PAGE; }
        }

        protected static DriverScope Root
        {
            get { return root ?? (root = new DriverScope(new SessionConfiguration(), new DocumentElementFinder(Driver), null, null, null, null)); }
        }

        private static void EnsureDriver()
        {
            if (driver != null && !driver.Disposed)
            {
                if (driverType == driver.GetType())
                    return;

                driver.Dispose();
            }

            driver = (Driver)Activator.CreateInstance(driverType,browser);
            root = null;
        }

        public static Driver Driver
        {
            get
            {
                EnsureDriver();
                return driver;

            }
        }
    }
}