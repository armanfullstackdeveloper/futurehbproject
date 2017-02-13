using System.Web.Mvc;
using System.Web.Routing;

namespace Boundary
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Product Without action",
                url: "Product/{id}",
                defaults: new { controller = "Search", action = "GetProduct" }
            );

            routes.MapRoute(
                name: "Sore Without action",
                url: "Shop/{shopname}/{id}",
                defaults: new { controller = "Store", action = "ShopPage", shopname = UrlParameter.Optional, id = UrlParameter.Optional } 
            );

            routes.MapRoute(
               name: "Sore Without action2",
               url: "Shop/code/{id}",
               defaults: new { controller = "Store", action = "ShopPage"}
            );

            routes.MapRoute(
                name: "Search Without action",
                url: "Search",
                defaults: new { controller = "Search", action = "Search" }
            );

            //routes.MapRoute(
            //    name: "Panel Without action",
            //    url: "Panel",
            //    defaults: new { controller = "StoreManagment", action = "SellerPanel" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );





        }
    }
}


