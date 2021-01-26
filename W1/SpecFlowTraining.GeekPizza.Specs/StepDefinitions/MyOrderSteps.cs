using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlowTraining.GeekPizza.Specs.Support;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using SpecFlowTraining.GeekPizza.Web.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class MyOrderSteps
    {
        private readonly BrowserContext _browserContext;
        private readonly AuthorizationContext _authorizationContext;
        private Table _orderedItems;

        public MyOrderSteps(BrowserContext browserContext, AuthorizationContext authorizationContext)
        {
            _browserContext = browserContext;
            _authorizationContext = authorizationContext;
        }

        [Given(@"my order contains the following items")]
        public void GivenMyOrderContainsTheFollowingItems(Table orderItemsTable)
        {
            var db = new DataContext();
            var myOrder = db.GetMyOrder(_authorizationContext.AssertLoggedInUser());

            var orderItems = orderItemsTable.CreateSet(() => new OrderItem { Size = DomainDefaults.OrderItemSize }).ToArray();
            foreach (var orderItem in orderItems)
            {
                var menuItem = db.FindMenuItemByName(orderItem.Name);
                Assert.IsNotNull(menuItem, "Unable to find menu item: {0}", orderItem.Name);
            }

            myOrder.OrderItems.AddRange(orderItems);
            UpdatePrice(myOrder);
            db.SaveChanges();

            _orderedItems = orderItemsTable;
        }

        [Given(@"my order is empty")]
        public void GivenMyOrderIsEmpty()
        {
            var db = new DataContext();
            db.DeleteMyOrder(_authorizationContext.AssertLoggedInUser());
            db.SaveChanges();
        }

        [Given(@"I have items in my order")]
        public void GivenIHaveItemsInMyOrder()
        {
            var db = new DataContext();
            var orderItem = new OrderItem();
            orderItem.Name = db.MenuItems.First().Name;
            orderItem.Size = OrderItemSize.Medium;
            var myOrder = db.GetMyOrder(_authorizationContext.AssertLoggedInUser());
            myOrder.OrderItems.Add(orderItem);
            UpdatePrice(myOrder);
            db.SaveChanges();
        }

        private void UpdatePrice(Order myOrder)
        {
            // using db-level order manipulation is dangerous, because we might forget
            // setting the prices and other calculated fields. For orders, it would be 
            // better to use the OrderController or an Order API to ensure the preconditions.
            // Try to refactor the code to use one of these!

            var priceCalculatorService = new PriceCalculatorService();
            priceCalculatorService.UpdatePrice(myOrder);
        }

        [When(@"I check the my order page")]
        public void WhenICheckTheMyOrderPage()
        {
            _browserContext.NavigateTo("/MyOrder");
            StringAssert.Contains(_browserContext.WebDriver.Title, "My Order");
        }

        [When(@"I add a (.*) ""(.*)"" pizza to my order")]
        public void WhenIAddALargePizzaToMyOrder(OrderItemSize size, string name)
        {
            _browserContext.NavigateTo("/Menu");

            var menuItemTr = _browserContext.WebDriver.FindElements(By.CssSelector($"#MenuTable tr[data-item-name=\"{name}\"]")).FirstOrDefault();
            Assert.IsNotNull(menuItemTr, $"Could not find pizza: {name}");

            var addItemForm = menuItemTr.FindElements(By.TagName("form")).FirstOrDefault();
            Assert.IsNotNull(menuItemTr, $"Could not find add item form for pizza: {name}");

            var sizeDropDownList = addItemForm.FindElement(By.Name("size"));
            new SelectElement(sizeDropDownList).SelectByValue(size.ToString());

            addItemForm.Submit();
        }

        [Then(@"my order should contain the following items")]
        public void ThenMyOrderShouldContainTheFollowingItems(Table expectedOrderItemsTable)
        {
            _browserContext.AssertOnPath("/MyOrder");
            StringAssert.Contains(_browserContext.WebDriver.Title, "My Order");

            var actualOrderItemNames = _browserContext.WebDriver
                .FindElements(By.CssSelector("#OrderItems .order-item-name"))
                .Select(span => span.Text)
                .ToList();

            var expectedOrderItemNames = expectedOrderItemsTable.Rows
                .Select(r => r["name"])
                .ToList();

            CollectionAssert.AreEquivalent(expectedOrderItemNames, actualOrderItemNames);
        }

        [Then(@"my order should contain the ordered items")]
        public void ThenMyOrderShouldContainTheOrderedItems()
        {
            ThenMyOrderShouldContainTheFollowingItems(_orderedItems);
        }
    }
}
