using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using SpecFlowTraining.GeekPizza.Web.Models;
using SpecFlowTraining.GeekPizza.Web.Services;

namespace SpecFlowTraining.GeekPizza.Web.Controllers
{
    public class AuthController
    {
        public HttpContextBase HttpContext = AuthenticationServices.GetCurrentHttpContext();

        public string Login(LoginFormModel args)
        {
            //for the sake of the course, we ensure that the default user always exists
            DefaultDataServices.EnsureDefaultUser();

            var db = new DataContext();
            var user = db.FindUserByName(args.Name);
            if (user == null || !user.Password.Equals(args.Password))
                return null;

            var token = AuthenticationServices.SetCurrentUser(user.Name);
            if (token == null)
                return null;

            AuthenticationServices.AddAuthCookie(HttpContext.Response, token);
            return token;
        }

        public void Logout(string token = null)
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext, token);
        }
    }
}