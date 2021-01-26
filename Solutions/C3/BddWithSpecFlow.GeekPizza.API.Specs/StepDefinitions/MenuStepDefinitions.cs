using System;
using System.Collections.Generic;
using BddWithSpecFlow.GeekPizza.Specs.Drivers;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddWithSpecFlow.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class MenuStepDefinitions
    {
        private readonly MenuApiDriver _menuApiDriver;
        private List<PizzaMenuItem> _menuItems;

        public MenuStepDefinitions(MenuApiDriver menuApiDriver)
        {
            _menuApiDriver = menuApiDriver;
        }

        [When(@"the client checks the menu page")]
        public void WhenTheClientChecksTheMenuPage()
        {
            _menuItems = _menuApiDriver.GetMenuItems();
        }

        [Then(@"there should be (.*) pizzas listed")]
        public void ThenThereShouldBePizzasListed(int expectedCount)
        {
            Assert.AreEqual(expectedCount, _menuItems.Count);
        }

        [Then(@"the following pizzas should be listed in this order")]
        public void ThenTheFollowingPizzasShouldBeListedInThisOrder(Table expectedMenuItemsTable)
        {
            expectedMenuItemsTable.CompareToSet(_menuItems, sequentialEquality: true);
        }
    }
}
