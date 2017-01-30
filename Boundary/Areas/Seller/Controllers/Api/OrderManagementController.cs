using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToOrder;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Seller.Controllers.Api
{
    [Authorize(Roles = StaticString.Role_Seller)]
    [RoutePrefix("api/Seller/OrderManagement")]
    public class OrderManagementController : ApiController
    {
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                return Json(JsonResultHelper.SuccessResult(new OrderBL().GetStoreOrders(store.StoreCode)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [Route("UpdateStatus")]
        public IHttpActionResult UpdateStatus(long orderCode, byte newStatusCode)
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                //چک کنیم اصلا سفارش متعلق به خودش هست یا نه
                bool isOrderForThisStore = new StoreBL().CheckHaveOrder(store.StoreCode, orderCode);
                if (!isOrderForThisStore)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                List<OrderHistory> lstOrderHistories = new OrderHistoryBL().GetAllForOrder(orderCode);
                OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();
                List<DropDownItemsModel> editableStatus = new OrderBL().CheckSellersEditableStatus((EOrderStatus)orderHistoryOfLastStatus.OrderStatusCode);
                if (editableStatus.Exists(s => s.Value == newStatusCode) == false)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                Order order = new OrderBL().SelectOne(orderCode);
                var result = new OrderHistoryBL().UpdateOrderStatus(order, newStatusCode,orderHistoryOfLastStatus.OrderStatusCode,
                    RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress);

                if (result.IsSuccess)
                    return Json(JsonResultHelper.SuccessResult());
                return Json(JsonResultHelper.FailedResultWithMessage());
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
                        }, new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => newStatusCode),
                            Value = newStatusCode.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
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
                        }, new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => newStatusCode),
                            Value = newStatusCode.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [Route("GetSendingTypes")]
        public IHttpActionResult GetSendingTypes()
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new OrderSendingTypeBL().SelectAll()));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [Route("UpdateSendingDetailes")]
        public IHttpActionResult UpdateSendingDetailes(long orderCode, byte type,string trackingCode)
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                //چک کنیم اصلا سفارش متعلق به خودش هست یا نه
                bool isOrderForThisStore = new StoreBL().CheckHaveOrder(store.StoreCode, orderCode);
                if (!isOrderForThisStore)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                Order order = new OrderBL().SelectOne(orderCode);
                order.OrderSendingTypeCode = type;
                order.TrackingCode = trackingCode;

                if (new OrderBL().Update(order))
                    return Json(JsonResultHelper.SuccessResult());
                return Json(JsonResultHelper.FailedResultWithMessage());
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
                        }, new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => type),
                            Value = type.ToString()
                        },
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => trackingCode),
                            Value = trackingCode
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
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
                            Name = HelperFunctionInBL.GetVariableName(() => type),
                            Value = type.ToString()
                        },
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => trackingCode),
                            Value = trackingCode
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }
    }
}
