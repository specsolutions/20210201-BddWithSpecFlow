using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowTraining.GeekPizza.Specs.Pages;
using SpecFlowTraining.GeekPizza.Specs.Support;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class AuthenticationSteps
    {
        private readonly LoginPage _loginPage;
        private readonly AuthorizationContext _authorizationContext;

        public AuthenticationSteps(LoginPage loginPage, AuthorizationContext authorizationContext)
        {
            _loginPage = loginPage;
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
            _loginPage.GoTo();
        }

        [Given(@"I am logged in with user '(.*)' and password '(.*)'")]
        public void GivenIAmLoggedInWithUserAndPassword(string userName, string password)
        {
            _loginPage.GoTo();
            _loginPage.FillOut(userName, password);
            bool success = _loginPage.Submit();
            Assert.IsTrue(success, "Login with user {0} as unsuccessful", userName);
            _authorizationContext.CurrentUser = userName;
        }

        [Given(@"I am logged in")]
        [BeforeScenario("login", Order = 200)]
        public void GivenIAmLoggedIn()
        {
            GivenIAmLoggedInWithUserAndPassword(DomainDefaults.UserName, DomainDefaults.UserPassword);
        }

        [When(@"I log in with user '(.*)' and password '(.*)'")]
        public void WhenILogInWithUserAndPassword(string userName, string password)
        {
            _loginPage.FillOut(userName, password);
            _loginPage.Submit();
        }
    }
}
