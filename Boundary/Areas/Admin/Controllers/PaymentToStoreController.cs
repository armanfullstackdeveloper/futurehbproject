using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Boundary.Areas.Admin.Models;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToPayments;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToPayments;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticString.Role_Admin + "," + StaticString.Role_SuperAdmin)]
    public class PaymentToStoreController : BaseController
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
                List<HBPaymentToStoreViewModel> lst =
                    (from x in new HBPaymentToStoreBL().GetAll_OrderByLast(pageNumber)
                        select new HBPaymentToStoreViewModel()
                        {
                            Date = x.Date,
                            AdminName = new HBAdminBL().SelectOne(x.AdminCode).Name,
                            Money = x.Money,
                            OrderCode = x.OrderCode,
                            TrackingCode = x.TrackingCode,
                        }).ToList();
                 
                return View(lst);
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

        public ActionResult SimpleCreate(long orderCode,int overallCost)
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

                return View(new HBPaymentToStore()
                {
                    Money = (overallCost-HelperFunctionInBL.GetProfit(overallCost,StaticNembericInBL.OrderProfitPercentForHoojiBooji)),
                    OrderCode = orderCode
                });
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => orderCode),
                            Value = orderCode.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => overallCost),
                            Value = overallCost.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => orderCode),
                            Value = orderCode.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => overallCost),
                            Value = overallCost.ToString()
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
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

        public ActionResult PostCreate(HBPaymentToStore hbPaymentToStore)
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

                long result = new HBPaymentToStoreBL().Insert(new HBPaymentToStore()
                {
                    Date = PersianDateTime.Now.Date.ToInt(),
                    AdminCode = checkSession.MainSession.Admin.Id,
                    OrderCode = hbPaymentToStore.OrderCode,
                    TrackingCode = hbPaymentToStore.TrackingCode,
                    Money = hbPaymentToStore.Money,
                });
                if (result > 0)
                {
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
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToStore),
                            Value = JObject.FromObject(hbPaymentToStore).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToStore),
                            Value = JObject.FromObject(hbPaymentToStore).ToString()
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

        public ActionResult Edit(long id) 
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

                HBPaymentToStore hbPaymentToStore = new HBPaymentToStoreBL().SelectOne(id);
                return View(hbPaymentToStore);
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

        public ActionResult PostEdit(HBPaymentToStore hbPaymentToStore)
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

                HBPaymentToStore item = new HBPaymentToStoreBL().SelectOne(hbPaymentToStore.OrderCode);

                item.Date = PersianDateTime.Now.Date.ToInt();
                item.AdminCode = checkSession.MainSession.Admin.Id;
                item.OrderCode = hbPaymentToStore.OrderCode;
                item.TrackingCode = hbPaymentToStore.TrackingCode;
                item.Money = hbPaymentToStore.Money;
              
                bool result = new HBPaymentToStoreBL().Update(item);
                if (result)
                {
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
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToStore),
                            Value = JObject.FromObject(hbPaymentToStore).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToStore),
                            Value = JObject.FromObject(hbPaymentToStore).ToString()
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

                HBPaymentToStore hbPaymentToStore = new HBPaymentToStoreBL().SelectOne(id);
                HBPaymentToStoreViewModel model = new HBPaymentToStoreViewModel()
                     {
                         Date = hbPaymentToStore.Date,
                         AdminName = new HBAdminBL().SelectOne(hbPaymentToStore.AdminCode).Name,
                         Money = hbPaymentToStore.Money,
                         OrderCode = hbPaymentToStore.OrderCode,
                         TrackingCode = hbPaymentToStore.TrackingCode,
                     };
                return View(model); 
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

                bool result = new HBPaymentToStoreBL().Delete(id);
                if (result)
                {
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
                            Value = JObject.FromObject(id).ToString()
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
                            Value = JObject.FromObject(id).ToString()
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