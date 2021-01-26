using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;

namespace SpecFlowTraining.GeekPizza.Specs.Pages
{
    public class LoginPage
    {
        private readonly BrowserContext _browserContext;

        private IWebElement Name => _browserContext.WebDriver.FindElement(By.Id("Name"));
        private IWebElement Password => _browserContext.WebDriver.FindElement(By.Id("Password"));
        private IWebElement LoginButton => _browserContext.WebDriver.FindElement(By.Id("LoginButton"));

        public LoginPage(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo("/Login");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Login");
        }

        public void FillOut(string userName, string password)
        {
            Name.SendKeys(userName);
            Password.SendKeys(password);
        }

        public bool Submit()
        {
            LoginButton.Click();
            return !_browserContext.WebDriver.Title.Contains("Login");
        }
    }
}
