using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace Boundary
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            //For Client Session Enable(For ApiController)
            //RouteTable.Routes.MapHttpRoute(
            // name: "DefaultApi",
            // routeTemplate: "api/{controller}/{id}",
            // defaults: new { id = RouteParameter.Optional }
            // ).RouteHandler = new SessionRouteHandler();
        }

        //#region For Client Session Enable(For ApiController)

        //public class SessionRouteHandler : IRouteHandler
        //{
        //    IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        //    {
        //        return new SessionControllerHandler(requestContext.RouteData);
        //    }
        //}
        //public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
        //{
        //    public SessionControllerHandler(RouteData routeData)
        //        : base(routeData)
        //    { }
        //}

        //#endregion

    }
}
