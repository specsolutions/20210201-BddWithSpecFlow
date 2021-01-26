using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class MenuSteps
    {
        private readonly BrowserContext _browserContext;

        public MenuSteps(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        [When(@"I check the menu page")]
        public void WhenICheckTheMenuPage()
        {
            _browserContext.NavigateTo("/Menu");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Menu");
        }

        [Then(@"there should be (.*) pizzas displayed")]
        public void ThenThereShouldBePizzasDisplayed(int expectedCount)
        {
            var actualMenuItemTrs = _browserContext.WebDriver.FindElements(By.CssSelector("#MenuTable tbody tr")).ToList();

            Assert.AreEqual(expectedCount, actualMenuItemTrs.Count);
        }

        private class MenuItemDisplayData
        {
            public string Name { get; set; }
            public string Ingredients { get; set; }
        }

        private List<MenuItemDisplayData> ParseMenu()
        {
            var actualMenuItemTrs = _browserContext.WebDriver.FindElements(By.CssSelector("#MenuTable tbody tr")).ToList();

            var result = new List<MenuItemDisplayData>();
            foreach (var actualMenuItemTr in actualMenuItemTrs)
            {
                var displayData = new MenuItemDisplayData();
                displayData.Name = actualMenuItemTr.FindElement(By.ClassName("menu-item-name")).Text;
                displayData.Ingredients = actualMenuItemTr.FindElements(By.TagName("td"))[1].Text;
                result.Add(displayData);
            }
            return result;
        }

        [Then(@"the following pizzas should be displayed in this order")]
        public void ThenTheFollowingPizzasShouldBeDisplayedInThisOrder(Table expectedMenuItemsTable)
        {
            var actualMenuItems = ParseMenu();
            expectedMenuItemsTable.CompareToSet(actualMenuItems, true);

            /* The parsing and the assertion concern of the plain implementation
             * has been split: the parsing is done by "ParseMenu()" and this way
             * we can use "CompareToSet" to perform the assertion.
             
            var actualMenuItemTrs = _browserContext.WebDriver.FindElements(By.CssSelector("#MenuTable tbody tr")).ToList();

            Assert.AreEqual(expectedMenuItemsTable.RowCount, actualMenuItemTrs.Count);
            for (int i = 0; i < expectedMenuItemsTable.RowCount; i++)
            {
                var expectedMenuItemRow = expectedMenuItemsTable.Rows[i];
                var actualMenuItemTr = actualMenuItemTrs[i];

                if (expectedMenuItemsTable.ContainsColumn("name"))
                {
                    var actualName = actualMenuItemTr.FindElement(By.ClassName("menu-item-name")).Text;
                    Assert.AreEqual(actualName, expectedMenuItemRow["name"]);
                }
                if (expectedMenuItemsTable.ContainsColumn("ingredients"))
                {
                    var actualIngredients = actualMenuItemTr.FindElements(By.TagName("td"))[1].Text;
                    Assert.AreEqual(actualIngredients, expectedMenuItemRow["ingredients"]);
                }
            }*/
        }
    }
}
