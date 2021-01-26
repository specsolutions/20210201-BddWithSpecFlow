using System;
using System.Collections.Generic;
using System.Linq;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using BddWithSpecFlow.GeekPizza.Web.Models;
using BddWithSpecFlow.GeekPizza.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BddWithSpecFlow.GeekPizza.Web.Controllers
{
    /// <summary>
    /// Processes user management requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DataContext db = new DataContext();

        // POST /api/user -- registers a user
        [HttpPost]
        public IActionResult Register(RegisterInputModel registerModel)
        {
            if (string.IsNullOrEmpty(registerModel.UserName) || string.IsNullOrEmpty(registerModel.Password) || string.IsNullOrEmpty(registerModel.PasswordReEnter))
                return BadRequest("Name and Password must be provided");
            if (registerModel.Password != registerModel.PasswordReEnter)
                return BadRequest("Re-entered password is different");

            var existingUser = db.Users.FirstOrDefault(u => u.Name == registerModel.UserName);
            if (existingUser != null)
                db.Users.Remove(existingUser);

            var user = new User
            {
                Name = registerModel.UserName,
                Password = registerModel.Password
            };
            db.Users.Add(user);
            db.SaveChanges();

            return Created($"user/{user.Id}", user);
        }

        // GET: api/user -- returns all users
        [HttpGet]
        public List<User> GetUsers(string token = null)
        {
            AuthenticationServices.EnsureAdminAuthenticated(HttpContext, token);

            var users = db.Users.OrderBy(u => u.Name).ToList();
            return users;
        }

        // GET: api/user/[guid] -- returns user details
        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id, string token = null)
        {
            var currentUserName = AuthenticationServices.EnsureAuthenticated(HttpContext, token);

            var user = db.Users.FirstOrDefault(u => u.Id == id);

            //only admins or the user with the ID should be able to call this
            if (user == null || user.Name != currentUserName)
                AuthenticationServices.EnsureAdminAuthenticated(HttpContext, token);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
