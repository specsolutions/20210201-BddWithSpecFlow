using System;
using System.Web;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using SpecFlowTraining.GeekPizza.Web.Models;
using SpecFlowTraining.GeekPizza.Web.Services;

namespace SpecFlowTraining.GeekPizza.Web.Controllers
{
    public class OrderController
    {
        public HttpContextBase HttpContext = AuthenticationServices.GetCurrentHttpContext();

        private readonly PriceCalculatorService _priceCalculatorService = new PriceCalculatorService();

        public MyOrderPageModel GetMyOrderPageModel()
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext);

            var db = new DataContext();
            return new MyOrderPageModel
            {
                MyOrder = db.GetMyOrder(userName)
            };
        }

        public OrderDetailsPageModel GetOrderDetailsPageModel()
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext);

            var db = new DataContext();

            var myOrder = db.GetMyOrder(userName);
            return new OrderDetailsPageModel
            {
                DeliveryStreetAddress = myOrder.DeliveryAddress.StreetAddress,
                DeliveryCity = myOrder.DeliveryAddress.City,
                DeliveryZip = myOrder.DeliveryAddress.Zip,
                DeliveryDate = myOrder.DeliveryDate,
                DeliveryTime = myOrder.DeliveryTime
            };
        }

        public Order UpdateOrderDetails(OrderDetailsPageModel orderUpdates)
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext);

            var db = new DataContext();
            var myOrder = db.GetMyOrder(userName);
            if (orderUpdates.DeliveryStreetAddress != null)
                myOrder.DeliveryAddress.StreetAddress = orderUpdates.DeliveryStreetAddress;
            if (orderUpdates.DeliveryCity != null)
                myOrder.DeliveryAddress.City = orderUpdates.DeliveryCity;
            if (orderUpdates.DeliveryZip != null)
                myOrder.DeliveryAddress.Zip = orderUpdates.DeliveryZip;
            myOrder.DeliveryDate = orderUpdates.DeliveryDate;
            myOrder.DeliveryTime = orderUpdates.DeliveryTime ?? DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(40));
            db.SaveChanges();

            return myOrder;
        }

        public Order AddToOrder(string menuItemName, OrderItemSize size)
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext);

            var db = new DataContext();
            var menuItem = db.FindMenuItemByName(menuItemName);
            var myOrder = db.GetMyOrder(userName);
            if (menuItem != null)
            {
                var pizzaOrderItem = new OrderItem
                {
                    Name = menuItem.Name,
                    Size = size
                };
                myOrder.OrderItems.Add(pizzaOrderItem);
                _priceCalculatorService.UpdatePrice(myOrder);
                db.SaveChanges();
            }

            return myOrder;
        }

        public void PlaceOrder()
        {
            var userName = AuthenticationServices.EnsureAuthenticated(HttpContext);

            // we do not place an order for real, but just clear the current order
            var db = new DataContext();
            db.DeleteMyOrder(userName);
            db.SaveChanges();
        }
    }
}