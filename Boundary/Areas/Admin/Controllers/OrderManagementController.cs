using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToOrder;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticString.Role_Admin + "," + StaticString.Role_SuperAdmin)]
    public class OrderManagementController : BaseController
    {
        /// <summary>
        /// show all orders
        /// </summary> 
        /// <returns></returns>
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
                List<OrderViewModelForAdmins> lst = new OrderBL().GetAllOrdersForAdmin(pageNumber);
                return View(lst ?? new List<OrderViewModelForAdmins>());
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


        /// <summary>
        /// سفارشاتی که نیازمند پیگیری ادمین می باشند
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult GetAdminReviewPendingOrders(int pageNumber = 1) 
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
                List<OrderViewModelForAdmins> lst = new OrderBL().GetAdminReviewPendingOrders(pageNumber);
                return View(lst ?? new List<OrderViewModelForAdmins>());
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


        public ActionResult Details(long orderCode)
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

                OrderViewModelForAdmins orderViewModel = new OrderBL().GetOneOrderDetailesForAdmin(orderCode);
                return View(orderViewModel ?? new OrderViewModelForAdmins());
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

        public ActionResult Edit(long orderCode) 
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

                OrderViewModelForAdmins orderViewModel = new OrderBL().GetOneOrderDetailesForAdmin(orderCode);

                ViewBag.Statuses = new SelectList(new OrderStatusBL().SelectAll(), "Id", "Name",orderViewModel.OrderStatus.Id);

                return View(orderViewModel ?? new OrderViewModelForAdmins());
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

        [HttpPost]
        public ActionResult PostEdit(long orderCode, byte newStatus)
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

                Order order = new OrderBL().SelectOne(orderCode);
                List<OrderHistory> lstOrderHistories = new OrderHistoryBL().GetAllForOrder(orderCode);
                OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();

                var result = new OrderHistoryBL().UpdateOrderStatus(order, newStatus,
                    orderHistoryOfLastStatus.OrderStatusCode, User.Identity.GetUserId() ?? Request.UserHostAddress);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Details", "OrderManagement", new { orderCode });
                }
                return RedirectToAction("Edit", "OrderManagement", new { orderCode });
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
                        },
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => newStatus),
                            Value = newStatus.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => newStatus),
                            Value = newStatus.ToString()
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

        public ActionResult Delete(long orderCode) 
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

                OrderViewModelForAdmins orderViewModel = new OrderBL().GetOneOrderDetailesForAdmin(orderCode);
                return View(orderViewModel ?? new OrderViewModelForAdmins());
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

        [HttpPost]
        public ActionResult PostDelete(long orderCode)
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

                bool result = new OrderBL().Delete(orderCode);

                if (result)
                {
                    return RedirectToAction("Index", "OrderManagement", new { orderCode });
                }
                else
                {
                    return RedirectToAction("Delete", "OrderManagement", new { orderCode });
                }
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