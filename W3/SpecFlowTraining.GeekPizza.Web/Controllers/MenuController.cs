using System.Linq;
using System.Web;
using SpecFlowTraining.GeekPizza.Web.DataAccess;
using SpecFlowTraining.GeekPizza.Web.Models;
using SpecFlowTraining.GeekPizza.Web.Services;

namespace SpecFlowTraining.GeekPizza.Web.Controllers
{
    public class MenuController
    {
        public MenuPageModel GetMenuPageModel()
        {
            var db = new DataContext();

            var model = new MenuPageModel();
            model.IsLoggedIn = AuthenticationServices.IsLoggedIn();
            model.MenuItems = db.MenuItems.OrderBy(mi => mi.Name).ToList();
            return model;
        }
    }
}