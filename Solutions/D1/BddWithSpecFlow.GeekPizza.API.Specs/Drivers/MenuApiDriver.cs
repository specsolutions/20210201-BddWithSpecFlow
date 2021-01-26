using System;
using System.Collections.Generic;
using BddWithSpecFlow.GeekPizza.Specs.Support;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;

namespace BddWithSpecFlow.GeekPizza.Specs.Drivers
{
    public class MenuApiDriver
    {
        private readonly WebApiContext _webApiContext;

        public MenuApiDriver(WebApiContext webApiContext)
        {
            _webApiContext = webApiContext;
        }

        public List<PizzaMenuItem> GetMenuItems()
        {
            return _webApiContext.ExecuteGet<List<PizzaMenuItem>>("/api/menu");
        }

        public PizzaMenuItem GetMenuItem(Guid id)
        {
            return _webApiContext.ExecuteGet<PizzaMenuItem>($"api/menu/{id}");
        }
    }
}
