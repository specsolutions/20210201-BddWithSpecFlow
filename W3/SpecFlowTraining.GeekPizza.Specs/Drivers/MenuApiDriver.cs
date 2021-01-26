using System;
using RestSharp;
using SpecFlowTraining.GeekPizza.Web.DataAccess;

namespace SpecFlowTraining.GeekPizza.Specs.Drivers
{
    public class MenuApiDriver : RestApiDriverBase
    {
        public PizzaMenuItem GetMenuItem(Guid id)
        {
            var request = new RestRequest();
            request.Resource = "api/menu/{id}";
            request.AddUrlSegment("id", id.ToString());
            return Execute<PizzaMenuItem>(request);
        }
    }
}
