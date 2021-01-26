using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;
using TechTalk.SpecFlow;

namespace SpecFlowTraining.GeekPizza.Specs.StepDefinitions
{
    [Binding]
    public class OrderDetailsSteps
    {
        private readonly BrowserContext _browserContext;

        public OrderDetailsSteps(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        [When(@"I specify (.*) at (.*) as delivery time")]
        public void WhenISpecifyTomorrowAsDeliveryTime(DateTime deliveryDate, TimeSpan? deliveryTime)
        {
            _browserContext.NavigateTo("/OrderDetails");

            var deliveryDateTextBox = _browserContext.WebDriver
                .FindElement(By.Id("DeliveryDate"));
            deliveryDateTextBox.Clear();
            deliveryDateTextBox.SendKeys(deliveryDate.ToString("yyyy-MM-dd"));

            var deliveryTimeTextBox = _browserContext.WebDriver
                .FindElement(By.Id("DeliveryTime"));
            deliveryTimeTextBox.Clear();
            if (deliveryTime != null)
                deliveryTimeTextBox.SendKeys(deliveryTime.Value.ToString("h\\:mm"));

            deliveryTimeTextBox.Submit();
        }

        [Then(@"my order should indicate that the delivery date is (.*)")]
        public void ThenMyOrderShouldIndicateThatTheDeliveryDateIsTomorrow(DateTime expectedDate)
        {
            _browserContext.NavigateTo("/MyOrder");

            var deliveryDateSpan = _browserContext.WebDriver
                .FindElement(By.Id("DeliveryDate"));
            Assert.AreEqual(expectedDate.ToString("yyyy-MM-dd"), deliveryDateSpan.Text);
        }

        [Then(@"the delivery time should be (.*)")]
        public void ThenTheDeliveryTimeShouldBe(TimeSpan expectedTime)
        {
            _browserContext.NavigateTo("/MyOrder");

            var deliveryTimeSpan = _browserContext.WebDriver
                .FindElement(By.Id("DeliveryTime"));
            Assert.AreEqual(expectedTime.ToString("h\\:mm"), deliveryTimeSpan.Text);
        }
    }
}
