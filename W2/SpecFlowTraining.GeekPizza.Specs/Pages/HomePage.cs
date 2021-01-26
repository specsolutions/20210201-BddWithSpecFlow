using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;

namespace SpecFlowTraining.GeekPizza.Specs.Pages
{
    public class HomePage
    {
        private readonly BrowserContext _browserContext;

        private IWebElement MessageDiv => _browserContext.WebDriver.FindElement(By.ClassName("message"));

        public bool IsOnPage => _browserContext.WebDriver.Title.Contains("Home");
        public string ActualMessage => MessageDiv.Text;

        public HomePage(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo("/");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Home");
        }
    }
}
