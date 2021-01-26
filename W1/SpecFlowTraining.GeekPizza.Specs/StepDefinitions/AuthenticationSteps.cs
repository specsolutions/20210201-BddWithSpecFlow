using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class AuthenticationSteps
    {
        private readonly BrowserContext _browserContext;
        private readonly AuthorizationContext _authorizationContext;

        public AuthenticationSteps(BrowserContext browserContext, AuthorizationContext authorizationContext)
        {
            _browserContext = browserContext;
            _authorizationContext = authorizationContext;
        }

        [Given(@"there is a user registered with user '(.*)' and password '(.*)'")]
        public void GivenThereIsAUserRegisteredWithUserAndPassword(string userName, string password)
        {
            var db = new DataContext();
            var existingUser = db.Users.FirstOrDefault(u => u.Name == userName);
            if (existingUser != null)
            {
                db.Users.Remove(existingUser);
                db.SaveChanges();
            }
            db.Users.Add(new User { Name = userName, Password = password });
            db.SaveChanges();
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _browserContext.NavigateTo("/Login");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Login");
        }

        [Given(@"I am logged in with user '(.*)' and password '(.*)'")]
        public void GivenIAmLoggedInWithUserAndPassword(string userName, string password)
        {
            //NOTE: We will eliminate the code duplication in a later exercise

            _browserContext.NavigateTo("/Login");
            var nameTextBox = _browserContext.WebDriver.FindElement(By.Id("Name"));
            nameTextBox.SendKeys(userName);

            var passwordTextBox = _browserContext.WebDriver.FindElement(By.Id("Password"));
            passwordTextBox.SendKeys(password);

            var loginButton = _browserContext.WebDriver.FindElement(By.Id("LoginButton"));
            loginButton.Click();

            StringAssert.Contains(_browserContext.WebDriver.Title, "Home", "Login with user {0} as unsuccessful", userName);
            _authorizationContext.CurrentUser = userName;
        }

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            GivenIAmLoggedInWithUserAndPassword(DomainDefaults.UserName, DomainDefaults.UserPassword);
        }

        [BeforeScenario("login", Order = 200)]
        public void AutoLogin()
        {
            GivenIAmLoggedInWithUserAndPassword(DomainDefaults.UserName, DomainDefaults.UserPassword);
        }

        [When(@"I log in with user '(.*)' and password '(.*)'")]
        public void WhenILogInWithUserAndPassword(string userName, string password)
        {
            var nameTextBox = _browserContext.WebDriver.FindElement(By.Id("Name"));
            nameTextBox.SendKeys(userName);

            var passwordTextBox = _browserContext.WebDriver.FindElement(By.Id("Password"));
            passwordTextBox.SendKeys(password);

            var loginButton = _browserContext.WebDriver.FindElement(By.Id("LoginButton"));
            loginButton.Click();
        }

        [Then(@"the home page should be opened")]
        public void ThenTheHomePageShouldBeOpened()
        {
            _browserContext.AssertOnPath("/");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Home");
        }
    }
}
