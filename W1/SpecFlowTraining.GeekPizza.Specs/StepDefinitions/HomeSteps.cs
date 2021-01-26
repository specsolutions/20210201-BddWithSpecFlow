using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class HomeSteps
    {
        private readonly BrowserContext _browserContext;

        public HomeSteps(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        [When(@"I check the home page")]
        public void WhenICheckTheHomePage()
        {
            _browserContext.NavigateTo("/");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Home");
        }

        [Then(@"a message should be displayed: ""(.*)""")]
        public void ThenAMessageShouldBeDisplayed(string expectedMessage)
        {
            var messageDiv = _browserContext.WebDriver.FindElement(By.ClassName("message"));
            var actualMessage = messageDiv.Text;
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
