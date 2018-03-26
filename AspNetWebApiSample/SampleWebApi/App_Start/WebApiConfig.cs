using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SampleWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API の設定およびサービス
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Returns JSON on Google Chrome.
            // Retains XML support:
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new RequestHeaderMapping("Accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, "application/json"));
            // Quits XML support:
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            // Quits XML support:
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API ルート
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
