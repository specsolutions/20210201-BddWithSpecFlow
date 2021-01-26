using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SpecFlowTraining.GeekPizza.Specs.Support;

namespace SpecFlowTraining.GeekPizza.Specs.Pages
{
    public class MyOrderPage
    {
        private readonly BrowserContext _browserContext;

        private IWebElement DeliveryDateSpan => _browserContext.WebDriver.FindElement(By.Id("DeliveryDate"));
        private IWebElement DeliveryTimeSpan => _browserContext.WebDriver.FindElement(By.Id("DeliveryTime"));

        public DateTime DeliveryDate =>
            DateTime.ParseExact(DeliveryDateSpan.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        public TimeSpan DeliveryTime =>
            TimeSpan.ParseExact(DeliveryTimeSpan.Text, "h\\:mm", CultureInfo.CurrentCulture);

        public MyOrderPage(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo("/MyOrder");
            StringAssert.Contains(_browserContext.WebDriver.Title, "My Order");
        }

        public List<string> GetActualOrderItemNames()
        {
            return _browserContext.WebDriver
                .FindElements(By.CssSelector("#OrderItems .order-item-name"))
                .Select(span => span.Text)
                .ToList();
        }
    }
}
