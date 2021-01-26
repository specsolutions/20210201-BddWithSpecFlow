using System;
using BddWithSpecFlow.GeekPizza.Specs.Drivers;
using BddWithSpecFlow.GeekPizza.Specs.Support;
using BddWithSpecFlow.GeekPizza.Web.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddWithSpecFlow.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class WebApiStepDefinitions
    {
        private readonly CurrentObjectContext _currentObjectContext;
        private readonly MenuApiDriver _menuApiDriver;

        private PizzaMenuItem _retrievedMenuItem;

        public WebApiStepDefinitions(CurrentObjectContext currentObjectContext, MenuApiDriver menuApiDriver)
        {
            _currentObjectContext = currentObjectContext;
            _menuApiDriver = menuApiDriver;
        }

        [When(@"the menu item \#(.*) is retrieved from the menu API resource by ID")]
        public void WhenTheMenuItemIsRetrievedFromTheMenuApiResourceById(int testId)
        {
            var menuItem = _currentObjectContext.MenuItems[testId];
            Assert.IsNotNull(menuItem);
            _retrievedMenuItem = _menuApiDriver.GetMenuItem(menuItem.Id);
        }

        [Then(@"the retrieved menu item should contain")]
        public void ThenTheRetrievedMenuItemShouldContain(Table expectedMenuItemTable)
        {
            Assert.IsNotNull(_retrievedMenuItem);
            expectedMenuItemTable.CompareToInstance(_retrievedMenuItem);
        }
    }
}
