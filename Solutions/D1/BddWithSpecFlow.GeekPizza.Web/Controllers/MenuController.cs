using System;
using System.Collections.Generic;
using System.Linq;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace BddWithSpecFlow.GeekPizza.Web.Controllers
{
    /// <summary>
    /// Processes requests related to the Menu page
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private DataContext db = new DataContext();

        // GET: api/menu -- returns menu items visible/available to users
        [HttpGet]
        public List<PizzaMenuItem> GetMenuItems()
        {
            var sortedMenuItems = db.MenuItems
                .Where(mi => !mi.Inactive)
                .OrderBy(mi => mi.Name)
                .ToList();
            return sortedMenuItems;
        }

        // GET: api/menu/[guid] -- returns the details of a menu item
        [HttpGet("{id}")]
        public IActionResult GetMenuItem(Guid id)
        {
            var menuItem = db.MenuItems.FirstOrDefault(mi => mi.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return Ok(menuItem);
        }
    }
}
