using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using SpecFlowTraining.GeekPizza.Web.DataAccess;

namespace SpecFlowTraining.GeekPizza.Web.Controllers.Api
{
    public class MenuController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/menu
        public IQueryable<PizzaMenuItem> GetMenuItems()
        {
            return db.MenuItems.AsQueryable();
        }

        // GET: api/menu/[guid]
        [ResponseType(typeof(PizzaMenuItem))]
        public IHttpActionResult GetMenuItem(Guid id)
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
