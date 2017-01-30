using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticString.Role_Admin + "," + StaticString.Role_SuperAdmin)] 
    public class PanelController : BaseController
    {
        public ActionResult Index()
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(new { success = false, responseText = checkSession.Message }, JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// اگه ورودی نداشته باشه یعنی خودشه
        /// </summary>
        /// <param name="adminCode"></param>
        /// <returns></returns>
        public ActionResult EditProfile(long? adminCode) 
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                long adminId = adminCode ?? checkSession.MainSession.Admin.Id;
                return View(new HBAdminBL().SelectOne(adminId));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult PostEditProfile(HBAdmin admin, HttpPostedFileBase file)  
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                string rootPath = string.Empty;
                string filePath = null;
                string newName = null;
                string extension = null;

                if (file != null)
                {
                    string lowerFilename = file.FileName.ToLower();
                    //age jozve anvae mojaz nabod hichi
                    if (!lowerFilename.EndsWith(".jpeg") &&
                        !lowerFilename.EndsWith(".jpg") &&
                        !lowerFilename.EndsWith(".png") &&
                        !lowerFilename.EndsWith(".gif"))
                    {
                        return Json(new { success = false, responseText = "فایل نامعتبر است" },
                            JsonRequestBehavior.AllowGet);
                    }

                    filePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Admin/AdminsProfileImage");

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    newName = Guid.NewGuid().ToString();
                    extension = Path.GetExtension(file.FileName);

                    rootPath = filePath.Substring(filePath.IndexOf("Content", System.StringComparison.Ordinal));
                    rootPath = System.IO.Path.Combine(rootPath, newName + extension);
                }

                HBAdmin lastAdmin = new HBAdminBL().SelectOne(admin.Id);
                string lastBoxImgAddress = lastAdmin.ImgAddress;

                lastAdmin.ImgAddress = (string.IsNullOrEmpty(rootPath)) ? lastBoxImgAddress : rootPath;
                lastAdmin.Name = admin.Name;

                bool result = new HBAdminBL().Update(lastAdmin);
                if (!result) return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                if (file == null || string.IsNullOrEmpty(rootPath)) return RedirectToAction("Index");
                file.SaveAs(filePath + "/" + newName + extension);

                //ghabli ro ham hazf mikonim
                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress)))
                {
                    System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress));
                }
                return RedirectToAction("Index");
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => admin),
                            Value = JObject.FromObject(admin).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
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
                            Name = HelperFunctionInBL.GetVariableName(() => admin),
                            Value = JObject.FromObject(admin).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }
	}
}