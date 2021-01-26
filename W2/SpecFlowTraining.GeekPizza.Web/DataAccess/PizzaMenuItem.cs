using System;

namespace SpecFlowTraining.GeekPizza.Web.DataAccess
{
    public class PizzaMenuItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public int Calories { get; set; }
    }
}