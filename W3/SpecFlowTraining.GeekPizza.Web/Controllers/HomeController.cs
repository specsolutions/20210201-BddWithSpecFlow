using SpecFlowTraining.GeekPizza.Web.Models;

namespace SpecFlowTraining.GeekPizza.Web.Controllers
{
    public class HomeController
    {
        public HomePageModel GetHomePageModel()
        {
            var model = new HomePageModel();
            model.WelcomeMessage = "Welcome to Geek Pizza!";
            return model;
        }
    }
}