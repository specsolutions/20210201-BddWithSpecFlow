using System.Collections.Generic;
using System.Linq;
using BddWithSpecFlow.GeekPizza.Web.Controllers;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;

namespace BddWithSpecFlow.GeekPizza.Specs.Drivers
{
    public class ControllerMenuDriver : IMenuDriver
    {
        public List<PizzaMenuItem> GetMenuItems()
        {
            var controller = new MenuController();
            return controller.GetMenuItems();
        }
    }
}