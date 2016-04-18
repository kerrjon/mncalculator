using System.Web.Mvc;
using System.Web.Routing;

namespace MnCalculator
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
                name: "ContactYourRespresentive",
                url: "contact-your-mn-representative",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );
      routes.MapRoute(
                name: "Examples",
                url: "unfair-child-support-examples-in-mn",
                defaults: new { controller = "Home", action = "Examples", id = UrlParameter.Optional }
            );

      routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
