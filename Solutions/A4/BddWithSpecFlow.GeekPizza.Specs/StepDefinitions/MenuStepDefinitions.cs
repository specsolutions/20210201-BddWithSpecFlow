using System;
using System.Collections.Generic;
using BddWithSpecFlow.GeekPizza.Web.Controllers;
using BddWithSpecFlow.GeekPizza.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddWithSpecFlow.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class MenuStepDefinitions
    {
        private List<PizzaMenuItem> _menuItems;

        [When(@"the client checks the menu page")]
        public void WhenTheClientChecksTheMenuPage()
        {
            var controller = new MenuController();
            _menuItems = controller.GetMenuItems();
        }

        [Then(@"there should be (.*) pizzas listed")]
        public void ThenThereShouldBePizzasListed(int expectedCount)
        {
            Assert.AreEqual(expectedCount, _menuItems.Count);
        }

        [Then(@"the following pizzas should be listed in this order")]
        public void ThenTheFollowingPizzasShouldBeListedInThisOrder(Table expectedMenuItemsTable)
        {
            // The CompereToSet helper method (from TechTalk.SpecFlow.Assist namespace)
            // compares the menu item list with the data table by matching the column headers
            // with the property names. The manual for-loop implementation is replaced by that.
            expectedMenuItemsTable.CompareToSet(_menuItems, sequentialEquality: true);

            // Assert.AreEqual(expectedMenuItemsTable.RowCount, _menuItems.Count);
            // for (int i = 0; i < expectedMenuItemsTable.RowCount; i++)
            // {
            //     if (expectedMenuItemsTable.ContainsColumn("name"))
            //         Assert.AreEqual(expectedMenuItemsTable.Rows[i]["name"], _menuItems[i].Name);
            //     if (expectedMenuItemsTable.ContainsColumn("ingredients"))
            //         Assert.AreEqual(expectedMenuItemsTable.Rows[i]["ingredients"], _menuItems[i].Ingredients);
            // }
        }
    }
}
