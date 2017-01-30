using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Helper;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.FirstPageBL;
using BusinessLogic.Helpers;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserGuid()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Low()
        {
            return View();
        }
        public ActionResult Complaints()
        {
            return View();
        }
        public ActionResult Error(string message, string trackingCode)
        {
            return View(new ErrorViewModel()
            {
                Message = message,
                TrackingCode = trackingCode
            });
        }

        //public ActionResult GetActiveAdvertise()
        //{
        //    try
        //    {
        //        return Json(JsonResultHelper.SuccessResult(new FirstPage_AdvertiseBL().GetActiveSlider()), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (MyExceptionHandler exp1)
        //    {
        //        try
        //        {
        //            List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
        //            {
        //                //new ActionInputViewModel()
        //                //{
        //                //    Name = HelpfulFunction.GetVariableName(() => X),
        //                //    Value = X
        //                //},
        //            };
        //            long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
        //            return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception)
        //        {
        //            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception exp3)
        //    {
        //        try
        //        {
        //            List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
        //            {
        //                //new ActionInputViewModel()
        //                //{
        //                //    Name = HelpfulFunction.GetVariableName(() => X),
        //                //    Value = X
        //                //},
        //            };
        //            long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
        //            return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception)
        //        {
        //            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
    }
}