using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowTraining.GeekPizza.Specs.Pages;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class MenuSteps
    {
        private readonly MenuPage _menuPage;

        public MenuSteps(MenuPage menuPage)
        {
            _menuPage = menuPage;
        }

        [When(@"I check the menu page")]
        public void WhenICheckTheMenuPage()
        {
            _menuPage.GoTo();
        }

        [Then(@"there should be (.*) pizzas displayed")]
        public void ThenThereShouldBePizzasDisplayed(int expectedCount)
        {
            var actualMenuItems = _menuPage.GetActualMenuItems();

            Assert.AreEqual(expectedCount, actualMenuItems.Count);
        }

        [Then(@"the following pizzas should be displayed in this order")]
        public void ThenTheFollowingPizzasShouldBeDisplayedInThisOrder(Table expectedMenuItemsTable)
        {
            var actualMenuItems = _menuPage.GetActualMenuItems();
            expectedMenuItemsTable.CompareToSet(actualMenuItems, true);
        }
    }
}
