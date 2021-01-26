using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowTraining.GeekPizza.Specs.Support;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class AdminSteps
    {
        [Given(@"there are (.*) pizzas on the menu")]
        public void GivenThereArePizzasOnTheMenu(int pizzaCount)
        {
            var db = new DataContext();
            db.MenuItems.Clear();

            for (int i = 0; i < pizzaCount; i++)
            {
                var pizzaMenuItem = DomainDefaults.CreateDefaultPizzaMenuItem();
                pizzaMenuItem.Name = "Pizza" + i;
                db.MenuItems.Add(pizzaMenuItem);
            }
            db.SaveChanges();
        }

        [Given(@"the following pizzas are on the menu")]
        public void GivenTheFollowingPizzasAreOnTheMenu(Table menuItemTable)
        {
            var db = new DataContext();
            db.MenuItems.Clear();

            var menuItems = menuItemTable.CreateSet(DomainDefaults.CreateDefaultPizzaMenuItem);
            db.MenuItems.AddRange(menuItems);
            db.SaveChanges();
        }
    }
}
