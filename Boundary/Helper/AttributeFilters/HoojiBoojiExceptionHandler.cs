using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using Boundary.Model;
using BusinessLogic.BussinesLogics;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Helper.AttributeFilters
{
    /// <summary>
    /// ExceptionHandler for api
    /// </summary>
    public class HoojiBoojiApiExceptionHandler : ExceptionFilterAttribute
    {
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
            long trackingCode = new ErrorLogBL().LogException(exception, !string.IsNullOrEmpty(userId) ? userId : HttpContext.Current.Request.UserHostAddress, JObject.FromObject(errorLog).ToString(), context.Request.Headers.UserAgent.ToString());

            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new ExceptionResultModel() { TrackingCode = trackingCode.ToString(), Text = "عملیات با خطا مواجه شد" }, new JsonMediaTypeFormatter(), "application/json");
        }
    }
}