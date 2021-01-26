using System;
using System.Net;
using System.Net.Http;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using BddWithSpecFlow.GeekPizza.Web.Models;
using BddWithSpecFlow.GeekPizza.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BddWithSpecFlow.GeekPizza.Web.Controllers
{
    /// <summary>
    /// Processes requests related to authentication and authorization
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: api/auth
        public IActionResult GetCurrentUser(string token = null)
        {
            var currentUser = AuthenticationServices.GetCurrentUserName(HttpContext, token);
            if (currentUser == null)
                return NotFound();
            return Content(currentUser);
        }

        // POST: api/auth
        [HttpPost]
        public IActionResult Login([FromBody] LoginInputModel args)
        {
            //for the sake of the course, we ensure that the default user always exists
            DefaultDataServices.EnsureDefaultUser();

            var db = new DataContext();
            var user = db.FindUserByName(args.Name);
            if (user == null || !user.Password.Equals(args.Password))
                return StatusCode(StatusCodes.Status403Forbidden);

            var token = AuthenticationServices.SetCurrentUser(user.Name);
            if (token == null)
                return StatusCode(StatusCodes.Status403Forbidden);

            AuthenticationServices.AddAuthCookie(this.Response, token);
            return Content(token);
        }

        // DELETE: api/auth
        [HttpDelete]
        public void Logout(string token = null)
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext, token);
        }
    }
}