using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToStore;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace Boundary.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticString.Role_Admin + "," + StaticString.Role_SuperAdmin)]
    public class StoreManagementController : BaseController
    {
        public ActionResult Index(int pageNumber = 1)
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

                List<long> lst = new StoreBL().SearchForAdmin(null, null, null, pageNumber);
                List<StoreDetailsViewModel> storeDetails = new List<StoreDetailsViewModel>();
                foreach (long item in lst)
                {
                    storeDetails.Add(new StoreBL().GetOneStoreDetails(item));
                }
                return View(storeDetails);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
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

        public PartialViewResult SearchStoreByAjax(string codeOrName,byte? storeType=null,EStoreStatus? status=null)  
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

                if (storeType == 0)
                    storeType = null;
                List<long> lst = new StoreBL().SearchForAdmin(codeOrName, storeType, status);
                List<StoreDetailsViewModel> storeDetails = new List<StoreDetailsViewModel>();
                foreach (long item in lst)
                {
                    storeDetails.Add(new StoreBL().GetOneStoreDetails(item));
                }
                return PartialView("_StoreViewModel", storeDetails);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => codeOrName),
                            Value = codeOrName
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
                            Name = HelperFunctionInBL.GetVariableName(() => codeOrName),
                            Value = codeOrName
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
                
        public ActionResult SellerDetailes(long id)
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

                Store store=new StoreBL().SelectOne(id);
                if (store != null)
                {
                    DataModel.Entities.RelatedToStore.Seller seller = new SellerBL().SelectOne((long) store.SallerCode);
                    return View(seller);
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

        public ActionResult UpdateStatus(long storeCode, EStoreStatus status)
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

                ISession session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();
                
                Store store = new StoreBL().SelectOne(storeCode);
                store.StoreStatus = status;
                if (new StoreBL().UpdateWhitOutCommitTransaction(store, session))
                {
                    new StoreBL().AfterStatusChange(store.UserCode,storeCode, status, session);
                    session.Transaction.Commit();
                    return Json(JsonResultHelper.SuccessResult(true), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonResultHelper.SuccessResult(false), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
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

                Store store = new StoreBL().SelectOne(id);
                return View(store);
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

                StoreRemovingResultDataModel result = new StoreBL().RemoveStore(id);
                if (result.Success)
                {
                    foreach (string address in result.ImageAddressList)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + address)))
                        {
                            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + address));
                        }  
                    }
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