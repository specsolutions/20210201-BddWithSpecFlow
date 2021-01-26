using System;
using System.Collections.Generic;
using BddWithSpecFlow.GeekPizza.Web.Controllers;
using BddWithSpecFlow.GeekPizza.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using BddWithSpecFlow.GeekPizza.Specs.Support;
using BddWithSpecFlow.GeekPizza.Web.Services;
using Microsoft.AspNetCore.Mvc;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddWithSpecFlow.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class AuthStepDefinitions
    {
        private readonly AuthContext _authContext;

        public AuthStepDefinitions(AuthContext authContext)
        {
            _authContext = authContext;
        }

        [Given(@"the client is logged in")]
        public void GivenTheClientIsLoggedIn()
        {
            // The login process generates an authentication token that has to be passed in
            // for all subsequent controller method calls in the same user session.
            // You can see this in the method below, where we call the HomeController.GetHomePageModel
            // method.
            var defaultUserName = "Marvin";
            var controller = new AuthController();
            var result = controller.Login(new LoginInputModel { Name = defaultUserName, Password = "1234" });
            Assert.IsTrue(result.IsSuccessful());

            _authContext.AuthToken = ((ContentResult)result).Content;
            _authContext.LoggedInUserName = defaultUserName;
        }
    }
}
