﻿using SpecFlowTraining.GeekPizza.Web.DataAccess;

namespace SpecFlowTraining.GeekPizza.Specs.Support
{
    public static class DomainDefaults
    {
        public const string MenuItemIngredients = "[default ingredients]";
        public const int MenuItemCalories = 1000;

        public const string UserName = "Marvin";
        public const string UserPassword = "1234";

        public const OrderItemSize OrderItemSize = Web.DataAccess.OrderItemSize.Medium;

        public static PizzaMenuItem CreateDefaultPizzaMenuItem()
        {
            return new PizzaMenuItem
            {
                Ingredients = MenuItemIngredients,
                Calories = MenuItemCalories
            };
        }

        public static void AddDefaultUsers()
        {
            var db = new DataContext();
            db.Users.Add(new User {Name = UserName, Password = UserPassword});
            db.SaveChanges();
        }

        public static void AddDefaultPizzas()
        {
            var db = new DataContext();
            db.MenuItems.Add(new PizzaMenuItem
            {
                Name = "Margherita",
                Ingredients = "tomato slices, oregano, mozzarella",
                Calories = 1920
            });
            db.MenuItems.Add(new PizzaMenuItem
            {
                Name = "Fitness",
                Ingredients = "sweetcorn, broccoli, feta cheese, mozzarella",
                Calories = 1340
            });
            db.MenuItems.Add(new PizzaMenuItem
            {
                Name = "BBQ",
                Ingredients = "BBQ sauce, bacon, chicken breast strips, onions",
                Calories = 1580
            });
            db.MenuItems.Add(new PizzaMenuItem
            {
                Name = "Mexican",
                Ingredients = "taco sauce, bacon, beans, sweetcorn, mozzarella",
                Calories = 2340
            });
            db.MenuItems.Add(new PizzaMenuItem
            {
                Name = "Quattro formaggi",
                Ingredients = "blue cheese, parmesan, smoked mozzarella, mozzarella",
                Calories = 2150
            });
            db.SaveChanges();
        }
    }
}
