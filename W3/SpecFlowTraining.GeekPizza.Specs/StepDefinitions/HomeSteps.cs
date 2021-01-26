using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Pages;
using SpecFlowTraining.GeekPizza.Specs.Support;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class HomeSteps
    {
        private readonly HomePage _homePage;

        public HomeSteps(HomePage homePage)
        {
            _homePage = homePage;
        }

        [When(@"I check the home page")]
        public void WhenICheckTheHomePage()
        {
            _homePage.GoTo();
        }

        [Then(@"a message should be displayed: ""(.*)""")]
        public void ThenAMessageShouldBeDisplayed(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _homePage.ActualMessage);
        }

        [Then(@"the home page should be opened")]
        public void ThenTheHomePageShouldBeOpened()
        {
            Assert.IsTrue(_homePage.IsOnPage);
        }
    }
}
