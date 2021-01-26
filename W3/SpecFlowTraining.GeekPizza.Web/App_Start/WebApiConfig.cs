using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SpecFlowTraining.GeekPizza.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Test API
            config.Routes.MapHttpRoute(
                name: "AgentRoleTestApi",
                routeTemplate: "agent-{agent}-{role}/api/Test/{action}",
                defaults: new { id = RouteParameter.Optional, controller = "TestApi" }
            );
            config.Routes.MapHttpRoute(
                name: "AgentTestApi",
                routeTemplate: "agent-{agent}/api/Test/{action}",
                defaults: new { id = RouteParameter.Optional, controller = "TestApi", role = "Default" }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultTestApi",
                routeTemplate: "api/Test/{action}",
                defaults: new { id = RouteParameter.Optional, controller = "TestApi", agent = "Default", role = "Default" }
            );

            // Normal API
            config.Routes.MapHttpRoute(
                name: "AgentRoleApi",
                routeTemplate: "agent-{agent}-{role}/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "AgentApi",
                routeTemplate: "agent-{agent}/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, role = "Default" }
            );

            config.Routes.MapHttpRoute(
                name: "MenuApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, agent = "Default", role = "Default" }
            );
        }
    }
}
