using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Boundary.Helper.AttributeFilters;
using BusinessLogic.BussinesLogics;
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

        public ActionResult low()
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

        public ActionResult NotFound()
        {
            return View();
        }

        /// <summary>
        /// download android app
        /// "~/Content/AndroidAPK/1.1.apk"
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <returns></returns>
        public FileResult CheckForNewVersion(double currentVersion)  
        {
            try
            {
                double newVersion = Convert.ToDouble(ConfigurationManager.AppSettings["AndroidApkVersion"]);
                string fileAddress = ConfigurationManager.AppSettings["AndroidApkAddress"];
                if (newVersion > currentVersion)
                    return File(fileAddress, "application/vnd.android.package-archive","hoojibooji.apk");
                return null;
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => currentVersion),
                            Value = currentVersion.ToString()
                        },
                    };
                    new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => currentVersion),
                            Value = currentVersion.ToString()
                        },
                    };
                    new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}