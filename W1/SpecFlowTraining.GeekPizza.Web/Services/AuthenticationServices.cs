using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SpecFlowTraining.GeekPizza.Web.Services
{
    public class AuthenticationServices
    {
        public static HttpContextWrapper GetCurrentHttpContext()
        {
            return HttpContext.Current == null ? null : new HttpContextWrapper(HttpContext.Current);
        }

        private static readonly ConcurrentDictionary<string, string> LoggedInUsersByToken = new ConcurrentDictionary<string, string>();

        public static string SetCurrentUser(string userName)
        {
            var token = Guid.NewGuid().ToString("N");
            if (LoggedInUsersByToken.TryAdd(token, userName))
                return token;
            return null;
        }

        private static string GetTokenFromCookie(HttpContextBase httpContext)
        {
            var authTokenCookie = httpContext?.Request.Cookies["auth-token"];
            return authTokenCookie?.Value;
        }

        private static string EnsureToken(HttpContextBase httpContext, string token)
        {
            return token ?? GetTokenFromCookie(httpContext); // check cookies if not provided
        }

        public static string GetCurrentUserName(HttpContextBase httpContext, string requestToken = null)
        {
            var token = EnsureToken(httpContext, requestToken);
            if (token == null || !LoggedInUsersByToken.TryGetValue(token, out var userName))
                return null;
            return userName;
        }

        public static bool IsLoggedIn()
            => GetCurrentUserName(GetCurrentHttpContext()) != null;

        public static bool IsLoggedIn(HttpContextBase httpContext, string requestToken = null)
            => GetCurrentUserName(httpContext, requestToken) != null;

        public static void ClearLoggedInUser(HttpContextBase httpContext, string requestToken = null)
        {
            var token = EnsureToken(httpContext, requestToken);
            if (token != null)
                LoggedInUsersByToken.TryRemove(token, out _);
        }

        public static string EnsureAuthenticated(HttpContextBase httpContext, string requestToken = null)
        {
            var currentUserName = GetCurrentUserName(httpContext, requestToken);
            if (currentUserName == null)
                //TODO: throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden) {ReasonPhrase = "Not logged in"});
                throw new Exception(new HttpResponseMessage(HttpStatusCode.Forbidden) { ReasonPhrase = "Not logged in" }.ToString());
            return currentUserName;
        }

        public static void AddAuthCookie(HttpResponseBase response, string token)
        {
            var httpCookie = new HttpCookie("auth-token", token)
            {
                Path = "/",
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                //IsEssential = true
            };
            response.Cookies.Add(httpCookie);
        }
    }
}