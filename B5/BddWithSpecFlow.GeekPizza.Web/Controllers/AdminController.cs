using System;
using System.Collections.Generic;
using System.Linq;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using BddWithSpecFlow.GeekPizza.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BddWithSpecFlow.GeekPizza.Web.Controllers
{
    /// <summary>
    /// Processes restaurant admin requests (exposed on Admin page)
    /// </summary>
    [Route("api/admin/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private DataContext db = new DataContext();

        // GET: api/admin/GetMenuItems -- returns all menu items
        [HttpGet]
        public List<PizzaMenuItem> GetMenuItems(string token = null)
        {
            AuthenticationServices.EnsureAdminAuthenticated(HttpContext, token);

            var menuItems = db.MenuItems.ToList();
            return menuItems;
        }

        private bool IsValidMenuItem(PizzaMenuItem menuItem)
        {
            return !string.IsNullOrEmpty(menuItem.Name) &&
                   !string.IsNullOrEmpty(menuItem.Ingredients) &&
                   menuItem.Calories > 0;
        }

        // POST /api/admin/UpdateMenu -- replaces the menu
        [HttpPost]
        public IActionResult UpdateMenu([FromBody] PizzaMenuItem[] menuItems, string token = null)
        {
            AuthenticationServices.EnsureAdminAuthenticated(HttpContext, token);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (menuItems == null || menuItems.Length == 0)
                return BadRequest("The menu cannot be empty!");
            if (menuItems.Any(mi => !IsValidMenuItem(mi)))
                return BadRequest("Invalid menu item");

            db.MenuItems.Clear();

            foreach (var menuItem in menuItems)
            {
                if (menuItem.Id == Guid.Empty)
                    menuItem.Id = Guid.NewGuid();

                db.MenuItems.Add(menuItem);
            }

            db.SaveChanges();
            return NoContent();
        }
    }
}
