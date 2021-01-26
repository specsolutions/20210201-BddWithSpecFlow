using System;
using System.Collections.Generic;
using System.Linq;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;

namespace BddWithSpecFlow.GeekPizza.Specs.Drivers
{
    public interface IMenuDriver
    {
        List<PizzaMenuItem> GetMenuItems();
    }
}
