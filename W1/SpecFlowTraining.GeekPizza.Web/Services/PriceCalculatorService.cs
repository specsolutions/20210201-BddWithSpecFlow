using System.Linq;
using SpecFlowTraining.GeekPizza.Web.DataAccess;

namespace SpecFlowTraining.GeekPizza.Web.Services
{
    public class PriceCalculatorService
    {
        private const decimal LARGE_PRICE = 25;
        private const decimal MEDIUM_PRICE = 15;
        private const decimal SMALL_PRICE = 10;
        private const decimal DELIVERY_COST = 5;
        private const decimal DELIVERY_COST_THRESHOLD = 40;

        public decimal GetItemPrice(OrderItem orderItem)
        {
            switch (orderItem.Size)
            {
                case OrderItemSize.Large:
                    return LARGE_PRICE;
                case OrderItemSize.Small:
                    return SMALL_PRICE;
                default:
                    return MEDIUM_PRICE;
            }
        }

        public OrderPrice GetOrderPrice(Order order)
        {
            decimal subtotal = 0;
            subtotal += order.OrderItems.Select(GetItemPrice).Sum();
            decimal deliveryCosts = 0;
            if (subtotal <= DELIVERY_COST_THRESHOLD)
                deliveryCosts = DELIVERY_COST;
            return new OrderPrice
            {
                Subtotal = subtotal,
                DeliveryCosts = deliveryCosts,
                Total = subtotal + deliveryCosts
            };
        }

        public void UpdatePrice(Order order)
        {
            order.Prices = GetOrderPrice(order);
        }
    }
}