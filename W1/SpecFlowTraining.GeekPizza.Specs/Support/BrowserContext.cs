using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISExpressBootstrapper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.Support
{
    public class BrowserContext
    {
        private readonly ScenarioContext _scenarioContext;

        public Uri BaseUrl { get; }
        public IWebDriver WebDriver { get; private set; }

        public bool IsOpen => WebDriver != null;

        public BrowserContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            BaseUrl = new Uri(ConfigurationManager.AppSettings["BaseUrl"]);
            WebDriver = GetCachedWebDriver();
        }

        public void Open()
        {
            if (IsOpen)
                return;
            StartIisExpress();
            WebDriver = CreateWebDriver();
            CacheWebDriver(WebDriver);
        }

        public void Close()
        {
            if (!IsOpen)
                return;
            if (GetCachedWebDriver() == WebDriver) // cached browser
            {
                // because we reuse the browser, we should clear session at least
                ClearSession();
                return;
            }
            WebDriver.Quit();
            WebDriver = null;
        }

        public void NavigateTo(string relativeUrl)
        {
            WebDriver.Navigate().GoToUrl(new Uri(BaseUrl, relativeUrl));
            StringAssert.Contains(WebDriver.Title, "Geek Pizza");
        }

        public void AssertOnPath(string relativeUrl)
        {
            Wait.For(() => // the busy waiting is needed by the new Firefox driver
            {
                var expectedUrl = new Uri(BaseUrl, relativeUrl).GetLeftPart(UriPartial.Path);
                var actualUrl = new Uri(WebDriver.Url).GetLeftPart(UriPartial.Path);
                actualUrl.Should().BeEquivalentTo(expectedUrl, "browser should be on page '{0}'", relativeUrl);
            });
        }

        private IWebDriver CreateWebDriver()
        {
            var browser = ConfigurationManager.AppSettings["Browser"] ?? "Chrome";
            switch (browser.ToLowerInvariant())
            {
                case "firefox":
                    return new FirefoxDriver();
                case "ie":
                    return new InternetExplorerDriver();
                case "chrome":
                    return new ChromeDriver();
            }
            throw new ConfigurationErrorsException("Invalid browser: " + browser);
        }

        public void ClearSession()
        {
            if (!IsOpen)
                return;
            try
            {
                WebDriver.Manage().Cookies.DeleteAllCookies();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SessionClearingError: {0}", ex);
            }
        }

        public void TakeScreenshot(string outputFolder, string fileNameBase)
        {
            try
            {
                string pageSource = WebDriver.PageSource;
                string sourceFilePath = Path.Combine(outputFolder, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));

                ITakesScreenshot takesScreenshot = WebDriver as ITakesScreenshot;

                if (takesScreenshot != null)
                {
                    var screenshot = takesScreenshot.GetScreenshot();
                    string screenshotFilePath = Path.Combine(outputFolder, fileNameBase + "_screenshot.png");
                    screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

                    Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);
            }
        }

        #region Browser Caching

        private class BrowserCacheInstance : IDisposable
        {
            public bool IsDisposed { get; private set; } = false;
            public IWebDriver WebDriver { get; set; }

            public void Dispose()
            {
                IsDisposed = true;
                WebDriver?.Dispose();
            }
        }

        private BrowserCacheInstance GetBrowserCacheInstance()
        {
            if (ConfigurationManager.AppSettings["CacheBrowser"] != "true")
                return null;

            var testThreadContext = _scenarioContext.ScenarioContainer.Resolve<TestThreadContext>();
            return testThreadContext.TestThreadContainer.Resolve<BrowserCacheInstance>();
        }

        public IWebDriver GetCachedWebDriver()
        {
            var cacheInstance = GetBrowserCacheInstance();
            return cacheInstance?.WebDriver;
        }

        public void CacheWebDriver(IWebDriver webDriver)
        {
            var cacheInstance = GetBrowserCacheInstance();
            if (cacheInstance != null)
            {
                cacheInstance.WebDriver = webDriver;
                //HACK: we need to save the instances because of a SpecFlow issue: the test thread context is not displosed
                CacheInstances.Add(cacheInstance);
            }
        }

        static private List<BrowserCacheInstance> CacheInstances = new List<BrowserCacheInstance>();

        public static void CloseChachedBrowsers()
        {
            //close all cached browsers if there are any
            foreach (var cacheInstance in CacheInstances.ToArray())
                if (!cacheInstance.IsDisposed)
                    cacheInstance.Dispose();

            StopIisExpress();
        }

        #endregion

        #region IIS Express
        private static IISExpressHost _iisExpressHost;

        public void StartIisExpress()
        {
            if (string.Equals(ConfigurationManager.AppSettings["StartIISExpress"], "true", StringComparison.InvariantCultureIgnoreCase) &&
                _iisExpressHost == null)
            {
                Console.WriteLine("Starting IIS Express...");
                _iisExpressHost = new IISExpressHost("SpecFlowTraining.GeekPizza.Web", 7880, iisExpressPath: GetDefaultIisExpressPath());
            }
        }

        public static void StopIisExpress()
        {
            if (_iisExpressHost != null)
            {
                Console.WriteLine("Shutting down IIS Express...");
                _iisExpressHost.Dispose();
                _iisExpressHost = null;
            }
        }

        private static string GetDefaultIisExpressPath()
        {
            var iisExpressX86Path = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\IIS Express\IISExpress.exe");
            var iisExpressX64Path = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\IIS Express\IISExpress.exe");

            return File.Exists(iisExpressX86Path) ? iisExpressX86Path : iisExpressX64Path;
        }
        #endregion
    }
}
