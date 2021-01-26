using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using SpecFlowTraining.GeekPizza.Web.Services;

namespace SpecFlowTraining.GeekPizza.Web.Controllers.Api
{
    public class TestApiController : ApiController
    {
        public HttpContextBase HttpContext = AuthenticationServices.GetCurrentHttpContext();

        // POST /api/test/Reset -- clears up the database
        [HttpPost]
        public IHttpActionResult Reset()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);
            var dataContext = new DataContext();
            dataContext.TruncateTables();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST /api/test/Seed -- clears up the database and adds default data
        [HttpPost]
        public IHttpActionResult Seed()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);
            var dataContext = new DataContext();
            dataContext.TruncateTables();
            DefaultDataServices.SeedWithDefaultData(dataContext);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST /api/test/ClearMenu -- clears the menu items
        [HttpPost]
        public IHttpActionResult ClearMenu()
        {
            var dataContext = new DataContext();
            dataContext.MenuItems.Clear();
            dataContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool IsValidMenuItem(PizzaMenuItem menuItem)
        {
            return !string.IsNullOrEmpty(menuItem.Name) &&
                   !string.IsNullOrEmpty(menuItem.Ingredients) &&
                   menuItem.Calories > 0;
        }

        // POST /api/test/AddMenuItem -- adds an item to the menu
        [HttpPost]
        [ResponseType(typeof(PizzaMenuItem))]
        public IHttpActionResult AddMenuItem(PizzaMenuItem menuItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!IsValidMenuItem(menuItem))
                return BadRequest("Invalid menu item");

            if (menuItem.Id == Guid.Empty)
                menuItem.Id = Guid.NewGuid();

            var dataContext = new DataContext();
            dataContext.MenuItems.Add(menuItem);
            dataContext.SaveChanges();

            return Created($"menu/{menuItem.Id}", menuItem);
        }

        // POST /api/test/SetMenu -- replaces the menu
        [HttpPost]
        public IHttpActionResult SetMenu(PizzaMenuItem[] menuItems)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (menuItems.Any(mi => !IsValidMenuItem(mi)))
                return BadRequest("Invalid menu item");

            var dataContext = new DataContext();
            dataContext.MenuItems.Clear();

            foreach (var menuItem in menuItems)
            {
                if (menuItem.Id == Guid.Empty)
                    menuItem.Id = Guid.NewGuid();

                dataContext.MenuItems.Add(menuItem);
            }

            dataContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST /api/test/AddUser -- registers a user
        [HttpPost]
        [ResponseType(typeof(User))]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
                return BadRequest("Name and Password must be provided");

            var dataContext = new DataContext();

            var existingUser = dataContext.Users.FirstOrDefault(u => u.Name == user.Name);
            if (existingUser != null)
                dataContext.Users.Remove(existingUser);

            dataContext.Users.Add(user);
            dataContext.SaveChanges();

            return Created($"users/{user.Id}", user);
        }


        // POST /api/test/DefaultLogin -- logs in with a default user
        [HttpPost]
        public IHttpActionResult DefaultLogin()
        {
            DefaultDataServices.EnsureDefaultUser();
            var token = AuthenticationServices.SetCurrentUser(DefaultDataServices.DefaultUserName);
            if (token == null)
                return StatusCode(HttpStatusCode.Forbidden);

            AuthenticationServices.AddAuthCookie(HttpContext.Response, token);
            return Content(HttpStatusCode.OK, token);
        }
    }
}
