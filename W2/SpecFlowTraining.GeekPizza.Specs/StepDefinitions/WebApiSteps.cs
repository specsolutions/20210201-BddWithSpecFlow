﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowTraining.GeekPizza.Specs.Drivers;
using SpecFlowTraining.GeekPizza.Specs.Support;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class WebApiSteps
    {
        private readonly CurrentObjectContext _currentObjectContext;
        private readonly MenuApiDriver _menuApiDriver;

        private PizzaMenuItem _retrievedMenuItem;

        public WebApiSteps(CurrentObjectContext currentObjectContext, MenuApiDriver menuApiDriver)
        {
            _currentObjectContext = currentObjectContext;
            _menuApiDriver = menuApiDriver;
        }

        [When(@"the menu item \#(.*) is retrieved from the menu API resource by ID")]
        public void WhenTheMenuItemIsRetrievedFromTheMenuAPIResourceByID(int testId)
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
