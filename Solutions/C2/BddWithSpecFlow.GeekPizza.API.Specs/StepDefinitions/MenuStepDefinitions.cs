using System;
using System.Collections.Generic;
using BddWithSpecFlow.GeekPizza.Specs.Support;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using BddWithSpecFlow.GeekPizza.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddWithSpecFlow.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class MenuStepDefinitions
    {
        private readonly WebApiContext _webApiContext;
        private List<PizzaMenuItem> _menuItems;

        public MenuStepDefinitions(WebApiContext webApiContext)
        {
            _webApiContext = webApiContext;
        }

        [When(@"the client checks the menu page")]
        public void WhenTheClientChecksTheMenuPage()
        {
            _menuItems = _webApiContext.ExecuteGet<List<PizzaMenuItem>>("/api/menu");
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
