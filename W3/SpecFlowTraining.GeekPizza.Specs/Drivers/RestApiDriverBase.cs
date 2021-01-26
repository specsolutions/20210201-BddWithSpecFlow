using System;
using System.Configuration;
using System.Net;
using RestSharp;

namespace SpecFlowTraining.GeekPizza.Specs.Drivers
{
    // based on https://github.com/restsharp/RestSharp/wiki/Recommended-Usage
    public class RestApiDriverBase
    {
        protected readonly string baseUrl;

        public RestApiDriverBase()
        {
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            if (baseUrl == null)
                throw new ConfigurationErrorsException("The 'BaseUrl' is not specified in the <appSettings>.");
        }

        protected sealed class VoidResult { }

        protected T Execute<T>(RestRequest request, string customBaseUrl = null) where T : class, new()
        {
            Console.WriteLine("Executing... {0}:{1}", request.Method, request.Resource);
            var client = new RestClient();
            client.BaseUrl = new Uri(customBaseUrl ?? baseUrl);
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }

            Console.WriteLine("Done. Staus code: {0} ({1})", (int)response.StatusCode, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound && typeof(T) != typeof(VoidResult))
                return null;

            if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                throw new ApplicationException($"Error retrieving response.  {response.StatusDescription}: {response.Content}");

            return response.Data;
        }
    }
}
