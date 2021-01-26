using System;
using System.Web.Mvc;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using SpecFlowTraining.GeekPizza.Web.Models;
using SpecFlowTraining.GeekPizza.Web.Services;

namespace SpecFlowTraining.GeekPizza.Web.Controllers
{
    public class PagesController : Controller
    {
        // GET: /Home or / -- renders home page
        public ViewResult Home()
        {
            var controller = new HomeController();
            var model = controller.GetHomePageModel();
            return View(model);
        }

        // GET: /Menu -- renders menu page
        public ViewResult Menu()
        {
            var controller = new MenuController();
            var model = controller.GetMenuPageModel();
            return View(model);
        }

        // GET: /Login -- renders login page
        public ViewResult Login()
        {
            return View();
        }

        // GET: /MyOrder -- renders MyOrder page
        public ViewResult MyOrder()
        {
            if (!AuthenticationServices.IsLoggedIn(HttpContext)) // not logged in
                return View("MyOrderNotLoggedIn");

            var controller = new OrderController();
            var model = controller.GetMyOrderPageModel();
            return View(model);
        }

        // GET: /OrderDetails -- renders OrderDetails page
        public ViewResult OrderDetails()
        {
            var controller = new OrderController();
            var model = controller.GetOrderDetailsPageModel();
            return View(model);
        }

        #region Actions

        //IMPORTANT: The authentication logic of this application has been simplified
        //          for the sake of the BDD with SpecFlow course. Do not copy!
        // POST: /Login -- submits Login form
        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            if (!ModelState.IsValid)
                return View();

            var controller = new AuthController();
            var token = controller.Login(new LoginFormModel { Name = name, Password = password });

            if (token == null)
            {
                ViewBag.ErrorMessage = "Invalid user name or password";
                return View();
            }

            return RedirectToAction("Home");
        }

        // GET: /Logout -- logs out current user
        public ActionResult Logout()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);

            return RedirectToAction("Home");
        }

        // POST: /OrderDetails -- submits Order Details form
        [HttpPost]
        public ActionResult OrderDetails(OrderDetailsPageModel orderDetails)
        {
            if (!ModelState.IsValid)
                return View();

            var controller = new OrderController();
            controller.UpdateOrderDetails(orderDetails);

            return RedirectToAction("MyOrder");
        }

        // POST: /AddToOrder -- adds a pizza menu item to the order
        [HttpPost]
        public ActionResult AddToOrder(string menuItemName, OrderItemSize size)
        {
            var controller = new OrderController();
            controller.AddToOrder(menuItemName, size);
            return RedirectToAction("MyOrder");
        }

        // POST: /PlaceOrder -- places the order
        [HttpPost]
        public ActionResult PlaceOrder()
        {
            var controller = new OrderController();
            controller.PlaceOrder();
            return RedirectToAction("MyOrder");
        }

        #endregion

    }
}