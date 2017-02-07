using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using Newtonsoft.Json.Converters;

namespace GSLogistics.Website.Admin.App_Start
{
    public static class WebApiConfig
    {
        public static string UrlPrefix { get { return "api"; } }
        public static string UrlPrefixRelative { get { return "~/api"; } }

        public static void Register(HttpConfiguration config)
        {


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: WebApiConfig.UrlPrefix + "/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           // config.Services.Replace(typeof(IHttpControllerSelector),
             //   new ApiControllerSelector(GlobalConfiguration.Configuration));

            // To send dates from the client to the Web API as ISO dates.
           // config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
        }

        public class ApiControllerSelector : DefaultHttpControllerSelector
        {
            public ApiControllerSelector(HttpConfiguration configuration)
                        : base(configuration)
            {
            }

            public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
            {
                return base.SelectController(request);
            }

            public override string GetControllerName(HttpRequestMessage request)
            {
                if (request == null)
                    throw new ArgumentNullException("request");
                IHttpRouteData routeData = request.GetRouteData();

                if (routeData == null)
                    return null;

                // Look up controller in route data
                object controllerName;
                routeData.Values.TryGetValue("controller", out controllerName);

                if (controllerName != null)
                    controllerName += "Api";

                return (string)controllerName;

            }
        }
    }
}