using System.Collections.Generic;
using SpecFlowTraining.GeekPizza.Web.DataAccess;

namespace SpecFlowTraining.GeekPizza.Web.Models
{
    public class MenuPageModel
    {
        public bool IsLoggedIn { get; set; }
        public List<PizzaMenuItem> MenuItems { get; set; }
    }
}