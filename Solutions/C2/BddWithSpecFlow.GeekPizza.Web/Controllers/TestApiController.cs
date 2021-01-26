using System;
using System.Linq;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using BddWithSpecFlow.GeekPizza.Web.Models;
using BddWithSpecFlow.GeekPizza.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BddWithSpecFlow.GeekPizza.Web.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// API to enable testing back-doors. Should not be deployed in production.
    /// </summary>
    [Route("api/test/[action]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        // POST /api/test/Reset -- clears up the database
        [HttpPost]
        public IActionResult Reset()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);
            var dataContext = new DataContext();
            dataContext.TruncateTables();
            return NoContent();
        }

        // POST /api/test/Seed -- clears up the database and adds default data
        [HttpPost]
        public IActionResult Seed()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);
            var dataContext = new DataContext();
            dataContext.TruncateTables();
            DefaultDataServices.SeedWithDefaultData(dataContext);
            return NoContent();
        }

        // POST /api/test/DefaultLogin -- logs in with a default user
        [HttpPost]
        public IActionResult DefaultLogin()
        {
            DefaultDataServices.EnsureDefaultUser();
            var token = AuthenticationServices.SetCurrentUser(DefaultDataServices.DefaultUserName);
            if (token == null)
                return StatusCode(StatusCodes.Status403Forbidden);

            AuthenticationServices.AddAuthCookie(this.Response, token);
            return Content(token);
        }

        // POST: api/test/BulkAddToOrder -- add multiple items to my order
        [HttpPost]
        public IActionResult BulkAddToOrder([FromBody] AddToOrderInputModel[] addToOrderInputs, string token = null)
        {
            var orderController = new OrderController();
            foreach (var addToOrderInput in addToOrderInputs)
            {
                var result = orderController.AddToOrder(addToOrderInput, token);
                if (!(result is OkObjectResult))
                    return result;
            }
            return NoContent();
        }
    }
}
