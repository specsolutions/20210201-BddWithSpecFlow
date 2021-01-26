using System;
using System.Net;
using BddWithSpecFlow.GeekPizza.Specs.Support;
using BddWithSpecFlow.GeekPizza.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddWithSpecFlow.GeekPizza.Specs.Drivers
{
    public class AuthApiDriver
    {
        private readonly WebApiContext _webApiContext;

        private WebApiResponse _lastResponse;

        public string LastError => _lastResponse?.ResponseMessage;

        public AuthApiDriver(WebApiContext webApiContext)
        {
            _webApiContext = webApiContext;
        }

        public bool AttemptLogin(string userName, string password)
        {
            var data = new LoginInputModel { Name = userName, Password = password };
            _lastResponse = _webApiContext.ExecutePost("api/auth", data);
            return _lastResponse.StatusCode == HttpStatusCode.OK;
        }
    }
}
