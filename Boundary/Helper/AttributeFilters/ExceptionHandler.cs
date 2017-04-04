using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Boundary.Model;
using BusinessLogic.BussinesLogics;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using NLog;

namespace Boundary.Helper.AttributeFilters
{
    /// <summary>
    /// ExceptionHandler for api
    /// </summary>
    public class HoojiBoojiApiExceptionHandler : ExceptionFilterAttribute
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception == null) return;

            ErrorLogModel errorLog = new ErrorLogModel()
            {
                Action = context.ActionContext.ActionDescriptor.ActionName,
                Controller = context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType.ToString(),
                Inputs = JArray.FromObject(context.ActionContext.ActionArguments.ToList()).ToString()
            };

            //save in db
            string userId = context.Request.GetRequestContext().Principal.Identity.GetUserId();
            long? trackingCode=null;
            try
            {
                trackingCode = new ErrorLogBL().LogException(exception, !string.IsNullOrEmpty(userId) ? userId : HttpContext.Current.Request.UserHostAddress, JObject.FromObject(errorLog).ToString(), context.Request.Headers.UserAgent.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new ExceptionResultModel() { TrackingCode = trackingCode.ToString(), Text = "عملیات با خطا مواجه شد" }, new JsonMediaTypeFormatter(), "application/json");
        }
    }

    public class HoojiBoojiExceptionHandler : HandleErrorAttribute
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            //filterContext.ExceptionHandled = true;
            //var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error1",
            //    ViewData = new ViewDataDictionary(model)
            //};

            ErrorLogModel errorLog = new ErrorLogModel()
            {
                Action = filterContext.RouteData.Values["Action"].ToString(),
                Controller = filterContext.Controller.ToString(),
                Inputs = filterContext.HttpContext.Request.QueryString.ToString()
            };

            string userId = filterContext.RequestContext.HttpContext.User.Identity.GetUserId();
            long? trackingCode = null;
            try
            {
                trackingCode = new ErrorLogBL().LogException(exception, !string.IsNullOrEmpty(userId) ? userId : HttpContext.Current.Request.UserHostAddress, JObject.FromObject(errorLog).ToString(), filterContext.RequestContext.HttpContext.Request.UserAgent);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}