using System;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.Support
{
    [Binding]
    public class BrowserHooks : Steps
    {
        private readonly BrowserContext _browserContext;

        public BrowserHooks(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        [BeforeScenario("web", Order = 100)]
        public void OpenBrowser()
        {
            _browserContext.Open();
        }

        [AfterScenario("web")]
        public void CloseBrowser()
        {
            _browserContext.Close();
        }

        [AfterTestRun]
        public static void CloseChachedBrowsers()
        {
            BrowserContext.CloseChachedBrowsers();
        }

        [AfterScenario("web", Order = 1)]
        public void HandleWebErrors()
        {
            if (ScenarioContext.TestError != null && _browserContext.IsOpen)
            {
                var fileNameBase = string.Format("error_{0}_{1}_{2}",
                    TestFolders.ToPath(FeatureContext.FeatureInfo.Title),
                    TestFolders.ToPath(ScenarioContext.ScenarioInfo.Title),
                    TestFolders.GetRawTimestamp());

                _browserContext.TakeScreenshot(TestFolders.OutputFolder, fileNameBase);
                _browserContext.Close(); // restart browser in case of an error
            }
        }
    }
}
