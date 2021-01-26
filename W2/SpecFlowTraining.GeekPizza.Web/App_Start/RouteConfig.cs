using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SpecFlowTraining.GeekPizza.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Pages",
                url: "{action}/{id}",
                defaults: new { controller = "Pages", action = "Home", id = UrlParameter.Optional, role = "Default", agent = "Default" },
                constraints: new { action = @"^(?!agent)\w+$", agent = @"^Default$", role = @"^Default$" }
            );
            routes.MapRoute(
                name: "Agent",
                url: "agent-{agent}/{action}/{id}",
                defaults: new { controller = "Pages", action = "Home", id = UrlParameter.Optional, role = "Default" },
                constraints: new { action = @"^(?!agent)\w+$", agent = @"^\w+$", role = @"^Default$" }
            );
            routes.MapRoute(
                name: "AgentRole",
                url: "agent-{agent}-{role}/{action}/{id}", defaults: new { controller = "Pages", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}
