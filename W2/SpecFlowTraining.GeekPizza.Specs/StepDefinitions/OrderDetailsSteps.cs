using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowTraining.GeekPizza.Specs.Pages;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class OrderDetailsSteps
    {
        private readonly OrderDetailsPage _orderDetailsPage;
        private readonly MyOrderPage _myOrderPage;

        public OrderDetailsSteps(OrderDetailsPage orderDetailsPage, MyOrderPage myOrderPage)
        {
            _orderDetailsPage = orderDetailsPage;
            _myOrderPage = myOrderPage;
        }

        [When(@"I specify (.*) at (.*) as delivery time")]
        public void WhenISpecifyTomorrowAsDeliveryTime(DateTime deliveryDate, TimeSpan? deliveryTime)
        {
            _orderDetailsPage.GoTo();
            _orderDetailsPage.SetDeliveryDate(deliveryDate);
            _orderDetailsPage.SetDeliveryTime(deliveryTime);
            _orderDetailsPage.Submit();
        }

        [Then(@"my order should indicate that the delivery date is (.*)")]
        public void ThenMyOrderShouldIndicateThatTheDeliveryDateIsTomorrow(DateTime expectedDate)
        {
            _myOrderPage.GoTo();
            Assert.AreEqual(expectedDate, _myOrderPage.DeliveryDate);
        }

        [Then(@"the delivery time should be (.*)")]
        public void ThenTheDeliveryTimeShouldBe(TimeSpan expectedTime)
        {
            _myOrderPage.GoTo();
            Assert.AreEqual(expectedTime, _myOrderPage.DeliveryTime);
        }
    }
}
