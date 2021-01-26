using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowTraining.GeekPizza.Specs.Pages;
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
        private readonly MenuPage _menuPage;
        private readonly MyOrderPage _myOrderPage;
        private readonly AuthorizationContext _authorizationContext;
        private Table _orderedItems;

        public MyOrderSteps(MyOrderPage myOrderPage, MenuPage menuPage, AuthorizationContext authorizationContext)
        {
            _myOrderPage = myOrderPage;
            _menuPage = menuPage;
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
            _myOrderPage.GoTo();
        }

        [When(@"I add a (.*) ""(.*)"" pizza to my order")]
        public void WhenIAddALargePizzaToMyOrder(OrderItemSize size, string name)
        {
            _menuPage.GoTo();
            _menuPage.AddItemToOrder(name, size);
        }

        [Then(@"my order should contain the following items")]
        public void ThenMyOrderShouldContainTheFollowingItems(Table expectedOrderItemsTable)
        {
            var actualOrderItemNames = _myOrderPage.GetActualOrderItemNames();
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
