using System.Web.Mvc;
using System.Web.Routing;

namespace dfva_webcsharp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(name: "download", url: "download/{id}", defaults: new { controller = "Home", action = "Download" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(name: "create_sign", url: "create_sign", defaults: new { controller = "Home", action = "ShowSignBnt" });
            routes.MapRoute(name: "sign", url: "sign/{id}", defaults: new { controller = "Dfva", action = "Sign" });
            routes.MapRoute(name: "signcheck", url: "check_sign/{id}", defaults: new { controller = "Dfva", action = "SignCheck" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
