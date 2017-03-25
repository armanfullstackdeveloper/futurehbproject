using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Boundary.Areas.SuperAdmin.Models;
using Boundary.Controllers;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticString.Role_SuperAdmin)]
    public class AdminManagementController : BaseController 
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
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                List<HBAdmin> lst = (List<HBAdmin>) new HBAdminBL().SelectAll();
                List<AdminViewModel> lstResult=new List<AdminViewModel>();
                foreach (HBAdmin hbAdmin in lst)
                {
                    lstResult.Add(new AdminViewModel()
                    {
                        Id = hbAdmin.Id,
                        ImgAddress = hbAdmin.ImgAddress,
                        Name = hbAdmin.Name,
                        UserCode = hbAdmin.UserCode,
                        RoleName = new RoleBL().SelectOne((int)new UserBL().GetById(hbAdmin.UserCode).RoleCode).Name
                    });
                }
                return View(lstResult);
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

        public ActionResult Create()
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

        public async Task<ActionResult> PostCreate(AdminRegisterDataModel adminRegisterDataModel, HttpPostedFileBase file)
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

                string rootPath = null, filePath = null, newName = null, extension = null;
                if (file != null)
                {

                    string lowerFilename = file.FileName.ToLower();
                    if (!lowerFilename.EndsWith(".jpeg") &&
                        !lowerFilename.EndsWith(".jpg") &&
                        !lowerFilename.EndsWith(".png") &&
                        !lowerFilename.EndsWith(".gif"))
                    {
                        return Json(JsonResultHelper.FailedResultWithMessage("فایل نامعتبر است"),
                            JsonRequestBehavior.AllowGet);
                    }

                    filePath =
                        System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Admin/AdminsProfileImage");

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    newName = Guid.NewGuid().ToString();
                    extension = Path.GetExtension(file.FileName);

                    rootPath = filePath.Substring(filePath.IndexOf("Content", System.StringComparison.Ordinal));
                    rootPath = System.IO.Path.Combine(rootPath, newName + extension);

                    adminRegisterDataModel.HbAdmin.ImgAddress = rootPath;
                }


                Role adminRole = (!adminRegisterDataModel.IsSuperAdmin ? StaticRole.Admin : StaticRole.SuperAdmin);
                if (adminRole == null)
                    return Json(JsonResultHelper.FailedResultWithMessage("Role not found"), JsonRequestBehavior.AllowGet);

                AppUser user = new AppUser { UserName = adminRegisterDataModel.RegisterMemberDataModel.UserName, Role = adminRole, RoleCode = adminRole.Id };
                var result = await new ShopFinderUserManager().CreateUser(ref user, HelperFunction.GetMd5Hash(adminRegisterDataModel.RegisterMemberDataModel.Password),"123");

                if (result == null || result.Succeeded == false)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                var saveResult = new HBAdminBL().Insert(new HBAdmin()
                {
                    Name = adminRegisterDataModel.HbAdmin.Name,
                    UserCode = user.Id,
                    ImgAddress = rootPath
                });

                if (saveResult > 0)
                {
                    if (string.IsNullOrEmpty(rootPath) == false)
                        file.SaveAs(filePath + "/" + newName + extension);
                    return RedirectToAction("Index");
                }

                new UserBL().DeleteById(user.Id);
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => adminRegisterDataModel),
                            Value = JObject.FromObject(adminRegisterDataModel).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithMessage(code+" :"+ exp1.ToString()), JsonRequestBehavior.AllowGet);
                }
                catch (Exception exp)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(exp.ToString()), JsonRequestBehavior.AllowGet);
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
                            Name = HelperFunctionInBL.GetVariableName(() => adminRegisterDataModel),
                            Value = JObject.FromObject(adminRegisterDataModel).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithMessage(code + " :" + exp3.ToString()), JsonRequestBehavior.AllowGet);
                }
                catch (Exception exp)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(exp.ToString()), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete(long id)
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

                return View(new HBAdminBL().SelectOne(id));
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

        public ActionResult PostDelete(long id)
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

                string imgAddress = new HBAdminBL().SelectOne(id).ImgAddress;
                if (new HBAdminBL().Delete(id))
                {
                    if (!string.IsNullOrEmpty(imgAddress))
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + imgAddress)))
                        {
                            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + imgAddress));
                        }
                    }

                    return RedirectToAction("Index");
                }
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = id.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = id.ToString()
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