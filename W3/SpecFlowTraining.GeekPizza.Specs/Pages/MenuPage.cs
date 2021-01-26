using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlowTraining.GeekPizza.Specs.Support;
using SpecFlowTraining.GeekPizza.Web.DataAccess;

namespace SpecFlowTraining.GeekPizza.Specs.Pages
{
    public class MenuPage
    {
        public class MenuItemDisplayData
        {
            public string Name { get; set; }
            public string Ingredients { get; set; }
        }

        private readonly BrowserContext _browserContext;

        private IList<IWebElement> MenuItemTrs 
            => _browserContext.WebDriver.FindElements(By.CssSelector("#MenuTable tbody tr"));

        public MenuPage(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo("/Menu");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Menu");
        }

        public List<MenuItemDisplayData> GetActualMenuItems()
        {
            var result = new List<MenuItemDisplayData>();
            foreach (var actualMenuItemTr in MenuItemTrs)
            {
                var displayData = new MenuItemDisplayData();
                displayData.Name = actualMenuItemTr.FindElement(By.ClassName("menu-item-name")).Text;
                displayData.Ingredients = actualMenuItemTr.FindElements(By.TagName("td"))[1].Text;
                result.Add(displayData);
            }
            return result;
        }

        public void AddItemToOrder(string name, OrderItemSize size)
        {
            var menuItemTr = _browserContext.WebDriver.FindElements(By.CssSelector($"#MenuTable tr[data-item-name=\"{name}\"]")).FirstOrDefault();
            Assert.IsNotNull(menuItemTr, $"Could not find pizza: {name}");

            var addItemForm = menuItemTr.FindElements(By.TagName("form")).FirstOrDefault();
            Assert.IsNotNull(menuItemTr, $"Could not find add item form for pizza: {name}");

            var sizeDropDownList = addItemForm.FindElement(By.Name("size"));
            new SelectElement(sizeDropDownList).SelectByValue(size.ToString());

            addItemForm.Submit();
            _browserContext.AssertOnPath("/MyOrder");
        }
    }
}
