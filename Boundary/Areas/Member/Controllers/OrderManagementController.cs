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
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Member.Controllers
{
    /// <summary>
    /// use for both api and mvc
    /// </summary>
    public class OrderManagementController : BaseController
    {
        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult GetAll()
        {
            try
            {
                CheckSessionDataModel checkSessionForMember = CheckMembererSession();
                CheckSessionDataModel checkSessionForSeller = CheckSallerSession();
                if (!checkSessionForMember.IsSuccess && !checkSessionForSeller.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
                long memberCode = (checkSessionForMember.IsSuccess) ? 
                    checkSessionForMember.MainSession.Member.Id : checkSessionForSeller.MainSession.Store.MemberCode;
                return Json(JsonResultHelper.SuccessResult(new OrderBL().GetMemberOrders(memberCode)), JsonRequestBehavior.AllowGet);
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

        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult UpdateStatus(long orderCode, byte newStatusCode)
        {
            try
            {
                CheckSessionDataModel checkSessionForMember = CheckMembererSession();
                CheckSessionDataModel checkSessionForSeller = CheckSallerSession();
                if (!checkSessionForMember.IsSuccess && !checkSessionForSeller.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
                long memberCode = (checkSessionForMember.IsSuccess) ? 
                    checkSessionForMember.MainSession.Member.Id : checkSessionForSeller.MainSession.Store.MemberCode;

                //چک کنیم اصلا سفارش متعلق به خودش هست یا نه
                Order order = new OrderBL().SelectOne(orderCode);
                if (order.MemberCode != memberCode)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                List<OrderHistory> lstOrderHistories = new OrderHistoryBL().GetAllForOrder(orderCode);
                OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();
                List<DropDownItemsModel> editableStatus = new OrderBL().CheckMembersEditableStatus((EOrderStatus)orderHistoryOfLastStatus.OrderStatusCode);
                if (editableStatus.Exists(s => s.Value == newStatusCode) == false)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                var result = new OrderHistoryBL().UpdateOrderStatus(order, newStatusCode,
                    orderHistoryOfLastStatus.OrderStatusCode, User.Identity.GetUserId() ?? Request.UserHostAddress);
                if (result.IsSuccess)
                {
                    if (result.MemberProfit > 0)
                    {
                        return Json(JsonResultHelper.SuccessResult(string.Format("سود شما از این خرید مبلغ: {0} تومان میباشد که به حساب شما در هوجی بوجی اضافه شد",result.MemberProfit)), JsonRequestBehavior.AllowGet);
                    }
                    return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
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
                            Name = HelperFunctionInBL.GetVariableName(() => orderCode),
                            Value = orderCode.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => newStatusCode),
                            Value = newStatusCode.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => newStatusCode),
                            Value = newStatusCode.ToString()
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

        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
