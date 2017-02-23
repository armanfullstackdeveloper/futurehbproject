using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticString.Role_Admin + "," + StaticString.Role_SuperAdmin)]
    public class UserManagementController : BaseController
    {
        public ActionResult Index(bool? active, int pageNumber = 1)
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

                pageNumber = (pageNumber > 0) ? pageNumber : 1;
                ViewBag.PageNumber = pageNumber;
                List<User> lstUsers = new UserBL().GetAll(null, active, pageNumber);
                lstUsers.RemoveAll(u => u.RoleCode == ERole.SuperAdmin || u.RoleCode == ERole.Admin);
                if (lstUsers.Count > 0)
                {
                    foreach (User user in lstUsers)
                    {
                        user.Role = StaticRole.GetRole((ERole)user.RoleCode);
                    }
                }
                return View(lstUsers);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => active),
                            Value = active.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => active),
                            Value = active.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
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

        public PartialViewResult SearchUserByAjax(string usernameOrId)
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return null;
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return null;
                    return null;
                }

                List<User> lstUsers = new UserBL().GetAll(usernameOrId);
                lstUsers.RemoveAll(u => u.RoleCode == ERole.SuperAdmin || u.RoleCode == ERole.Admin);
                foreach (User user in lstUsers)
                {
                    user.Role = StaticRole.GetRole((ERole)user.RoleCode);
                }
                return PartialView("_UserViewModel", lstUsers);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => usernameOrId),
                            Value = usernameOrId
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
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
                            Name = HelperFunctionInBL.GetVariableName(() => usernameOrId),
                            Value = usernameOrId
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public ActionResult Edit(string id)
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

                ViewBag.Roles = new RoleBL().SelectAll();
                User user = new UserBL().GetById(id);
                user.Role = StaticRole.GetRole(user.RoleCode);
                if (user.RoleCode == ERole.SuperAdmin || user.RoleCode == ERole.Admin)
                    return Json(JsonResultHelper.FailedResultWithMessage("fail access"), JsonRequestBehavior.AllowGet);

                return View(user);
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
                            Value = id
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
                            Value = id
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

        public ActionResult PostEdit(User user, string currentPassword)
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

                if (new UserBL().GetById(user.Id).Password != user.Password)
                    user.Password = HelperFunction.GetMd5Hash(user.Password);
                if (user.RoleCode == ERole.SuperAdmin || user.RoleCode == ERole.Admin)
                    return Json(JsonResultHelper.FailedResultWithMessage("fail access"), JsonRequestBehavior.AllowGet);
                if (new UserBL().Update(user).DbMessage.MessageType == MessageType.Success)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Edit", new { id = user.Id });
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => user),
                            Value = JObject.FromObject(user).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => user),
                            Value = JObject.FromObject(user).ToString()
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

        public ActionResult Delete(string id)
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

                User user = new UserBL().GetById(id);
                user.Role = StaticRole.GetRole(user.RoleCode);
                if (user.RoleCode == ERole.SuperAdmin || user.RoleCode == ERole.Admin)
                    return Json(JsonResultHelper.FailedResultWithMessage("fail access"), JsonRequestBehavior.AllowGet);
                return View(user);
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
                            Value = id
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
                            Value = id
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

        public ActionResult PostDelete(string id)
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

                User user = new UserBL().GetById(id);
                if (user.RoleCode == ERole.SuperAdmin || user.RoleCode == ERole.Admin)
                    return Json(JsonResultHelper.FailedResultWithMessage("fail access"), JsonRequestBehavior.AllowGet);
                if (new UserBL().DeleteById(id))
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { id });
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
                            Value = id
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
                            Value = id
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