using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;

namespace SpecFlowTraining.GeekPizza.Specs.Pages
{
    public class OrderDetailsPage
    {
        private readonly BrowserContext _browserContext;

        private IWebElement DeliveryDate => _browserContext.WebDriver.FindElement(By.Id("DeliveryDate"));
        private IWebElement DeliveryTime => _browserContext.WebDriver.FindElement(By.Id("DeliveryTime"));
        private IWebElement SaveButton => _browserContext.WebDriver.FindElement(By.Id("SaveButton"));

        public OrderDetailsPage(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo("/OrderDetails");
            StringAssert.Contains(_browserContext.WebDriver.Title, "Order Details");
        }

        public void SetDeliveryDate(DateTime deliveryDate)
        {
            DeliveryDate.Clear();
            DeliveryDate.SendKeys(deliveryDate.ToString("yyyy-MM-dd"));
        }

        public void SetDeliveryTime(TimeSpan? deliveryTime)
        {
            DeliveryTime.Clear();
            if (deliveryTime != null)
                DeliveryTime.SendKeys(deliveryTime.Value.ToString("h\\:mm"));
        }

        public void Submit()
        {
            SaveButton.Click();
        }
    }
}
