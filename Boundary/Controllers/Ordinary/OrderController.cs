﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Boundary.com.arianpal.merchant;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToPayments;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Entities.RelatedToOrder;
using DataModel.Entities.RelatedToPayments;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    public class OrderController : BaseController
    {
        #region Shoping Bag

        /// <summary>
        /// در این صفحه لیست محصولات موجود در سبد خرید نشان داده خواهد شد
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<ShoppingBagViewModel> cuurentList =
                (List<ShoppingBagViewModel>)Session[StaticString.Session_ShoppingBag];
            return View(cuurentList ?? new List<ShoppingBagViewModel>());
        }

        public ActionResult MemberLogin()
        {
            return View();
        }


        [System.Web.Mvc.Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult MemberInfo()
        {
            try
            {
                long memberCode;
                CheckSessionDataModel checkSession = CheckMembererSession();
                if (!checkSession.IsSuccess)
                {
                    checkSession = CheckSallerSession();
                    if (!checkSession.IsSuccess)
                    {
                        if (checkSession.Message == StaticString.Message_WrongAccess)
                            return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin);
                        if (checkSession.Message == StaticString.Message_UnSuccessFull)
                            return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError);
                        return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                    }
                    memberCode = checkSession.MainSession.Store.MemberCode;
                }
                else
                {
                    memberCode = checkSession.MainSession.Member.Id;
                }

                ViewBag.States = new StateBL().GetAllStatesCities();

                Member m = new MemberBL().SelectOne(memberCode);
                if (m != null)
                {
                    return View(new MemberViewModel()
                    {
                        Id = m.Id,
                        Balance = m.Balance,
                        CityCode = m.CityCode,
                        City = (m.CityCode != null) ? new CityBL().SelectOne((long)m.CityCode).Name : "-",
                        //Email = m.Email,
                        Latitude = m.Latitude,
                        Longitude = m.Longitude,
                        MobileNumber = m.MobileNumber,
                        Name = m.Name,
                        PhoneNumber = m.PhoneNumber,
                        PicAddress = m.PicAddress,
                        Place = m.Place,
                        PostalCode = m.PostalCode
                    });
                }
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

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

        [System.Web.Mvc.Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult BagReview()
        {
            try
            {
                long memberCode;
                CheckSessionDataModel checkSession = CheckMembererSession();
                if (!checkSession.IsSuccess)
                {
                    checkSession = CheckSallerSession();
                    if (!checkSession.IsSuccess)
                    {
                        if (checkSession.Message == StaticString.Message_WrongAccess)
                            return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin);
                        if (checkSession.Message == StaticString.Message_UnSuccessFull)
                            return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError);
                        return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                    }
                    memberCode = checkSession.MainSession.Store.MemberCode;
                }
                else
                {
                    memberCode = checkSession.MainSession.Member.Id;
                }

                Member m = new MemberBL().SelectOne(memberCode);
                if (m != null)
                {
                    ViewBag.MemberInfo = new MemberViewModel()
                    {
                        Id = m.Id,
                        Balance = m.Balance,
                        CityCode = m.CityCode,
                        City = (m.CityCode != null) ? new CityBL().SelectOne((long)m.CityCode).Name : "-",
                        //Email = m.Email,
                        Latitude = m.Latitude,
                        Longitude = m.Longitude,
                        MobileNumber = m.MobileNumber,
                        Name = m.Name,
                        PhoneNumber = m.PhoneNumber,
                        PicAddress = m.PicAddress,
                        Place = m.Place,
                        PostalCode = m.PostalCode
                    };

                    List<ShoppingBagViewModel> cuurentList = (List<ShoppingBagViewModel>)Session[StaticString.Session_ShoppingBag];
                    return View(cuurentList ?? new List<ShoppingBagViewModel>());
                }
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

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
        /// افزودن کالا به سبد خرید
        /// </summary>
        /// <param name="item"></param>
        public void AddToBag(ProductToShoppingBagDataModel item)
        {
            if (Session[StaticString.Session_ShoppingBag] == null)
            {
                Session[StaticString.Session_ShoppingBag] = new List<ShoppingBagViewModel>()
                    {
                        new ShoppingBagViewModel()
                        {
                            ProductCode = item.ProductCode,
                            ProductName = item.ProductName,
                            StoreName = item.StoreName,
                            StoreCode = item.StoreCode,
                            StoreCityCode = item.StoreCityCode,
                            RealPrice = item.RealPrice,
                            ImgAddress = item.ImgAddress,
                            PostalCostInCountry = item.PostalCostInCountry,
                            PostalCostInTown = item.PostalCostInTown,
                            Count = 1
                        }
                    };
            }
            else
            {
                List<ShoppingBagViewModel> cuurentList =
                    (List<ShoppingBagViewModel>)Session[StaticString.Session_ShoppingBag];
                if (cuurentList.Exists(s => s.ProductCode == item.ProductCode))
                {
                    cuurentList.Single(s => s.ProductCode == item.ProductCode).Count++;
                }
                else
                {
                    cuurentList.Add(new ShoppingBagViewModel()
                    {
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        StoreName = item.StoreName,
                        StoreCode = item.StoreCode,
                        StoreCityCode = item.StoreCityCode,
                        RealPrice = item.RealPrice,
                        ImgAddress = item.ImgAddress,
                        PostalCostInCountry = item.PostalCostInCountry,
                        PostalCostInTown = item.PostalCostInTown,
                        Count = 1
                    });
                }
                Session[StaticString.Session_ShoppingBag] = cuurentList;
            }
        }


        /// <summary>
        /// حذف یکی از کالاهای موجود در سبد خرید
        /// </summary>
        /// <param name="productCode"></param>
        public void RemoveFromBag(long productCode)
        {
            List<ShoppingBagViewModel> cuurentList =
                (List<ShoppingBagViewModel>)Session[StaticString.Session_ShoppingBag];
            if (cuurentList.Exists(s => s.ProductCode == productCode))
            {
                cuurentList.RemoveAll(s => s.ProductCode == productCode);
            }
            Session[StaticString.Session_ShoppingBag] = cuurentList;
        }


        /// <summary>
        /// افزودن تعداد یکی از کالاهای موجود در سبد خرید
        /// </summary>
        /// <param name="productCode"></param>
        public void AddProductCount(long productCode)
        {
            List<ShoppingBagViewModel> cuurentList =
                (List<ShoppingBagViewModel>)Session[StaticString.Session_ShoppingBag];
            if (cuurentList.Exists(s => s.ProductCode == productCode))
            {
                cuurentList.Single(s => s.ProductCode == productCode).Count++;
            }
            Session[StaticString.Session_ShoppingBag] = cuurentList;
        }

        /// <summary>
        /// کاهش تعداد یکی از کالاهای موجود در سبد خرید
        /// </summary>
        /// <param name="productCode"></param>
        public void MinusProductCount(long productCode)
        {
            List<ShoppingBagViewModel> cuurentList =
                (List<ShoppingBagViewModel>)Session[StaticString.Session_ShoppingBag];
            if (cuurentList.Exists(s => s.ProductCode == productCode))
            {
                cuurentList.Single(s => s.ProductCode == productCode).Count--;
            }
            Session[StaticString.Session_ShoppingBag] = cuurentList;
        }

        #endregion


        //[Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult PaymentResult()
        {
            try
            {
                WebService ws = new WebService();
                string merchantid = WebConfigurationManager.AppSettings["MerchantID"];
                string password = WebConfigurationManager.AppSettings["Password"];
                string refnum = Request["refnumber"];
                string status = Request["status"]; // وضعیت پرداخت 
                string resnumber = Request["resnumber"]; // شماره فاکتور-که همان کد درخواست در دیتابیس خودمان است
                long postedPaymentRequestCode = Convert.ToInt64(resnumber);

                List<Order> lstOrdersForCurrentPaymentRequest = new OrderBL().GetOrdersByPaymentRequestCode(postedPaymentRequestCode);
                int? requestedPrice = null;
                if (lstOrdersForCurrentPaymentRequest != null && lstOrdersForCurrentPaymentRequest.Count > 0)
                    requestedPrice = lstOrdersForCurrentPaymentRequest.Sum(o => o.OverallPayment); // هزینه فاکتور

                if (lstOrdersForCurrentPaymentRequest == null || lstOrdersForCurrentPaymentRequest.Count == 0 ||
                    requestedPrice == null || requestedPrice < StaticNemberic.MinimumTarakoneshValue)
                {
                    new PaymentResponseBL().Insert(new PaymentResponse()
                    {
                        TrackingCode = Convert.ToInt64(refnum),
                        PaymentResponseStatusCode = (byte)VerifyResult.Ready,
                        PaymentRequestCode = postedPaymentRequestCode,
                        VerifyDate = PersianDateTime.Now.Date.ToInt(),
                        VerifyTime = PersianDateTime.Now.TimeOfDay.ToShort(),
                    });

                    return RedirectToAction("PaymentDetails", new PaymentResultViewModel()
                    {
                        IsSuccess = false,
                        Message = StaticString.Message_UnSuccessFull,
                        TrackingCode = string.IsNullOrEmpty(refnum) ? "-" : refnum
                    });
                }

                string resultMessage = string.Empty;
                if (status == "100") // بنظرم وقتی میاد تو این که تو صفحه بانک رو پرداخت بزنه
                {
                    VerifyPaymentResult verifyPaymentResult = ws.verifyPayment(merchantid, password, (decimal)requestedPrice, refnum); // فراخوانی تابع پرداخت وب سرویس

                    if (verifyPaymentResult.ResultStatus == VerifyResult.NotMatchMoney)
                        resultMessage = "مبلغ واریزی  " + verifyPaymentResult.PayementedPrice.ToString("0,0") + " با مبلغ درخواستی یکسان نمی باشد ";
                    else if (verifyPaymentResult.ResultStatus == VerifyResult.Verifyed)
                        resultMessage = "قبلا پرداخت شده است";
                    else if (verifyPaymentResult.ResultStatus == VerifyResult.InvalidRef)
                        resultMessage = "شماره رسید قابل قبول نیست";
                    else if (verifyPaymentResult.ResultStatus == VerifyResult.Success)
                        resultMessage = "پرداخت انجام شد";


                    new PaymentResponseBL().Insert(new PaymentResponse()
                    {
                        TrackingCode = Convert.ToInt64(refnum),
                        PaymentResponseStatusCode = (byte)verifyPaymentResult.ResultStatus,
                        PaymentRequestCode = postedPaymentRequestCode,
                        VerifyDate = PersianDateTime.Now.Date.ToInt(),
                        VerifyTime = PersianDateTime.Now.TimeOfDay.ToShort(),
                    });


                    if (verifyPaymentResult.ResultStatus == VerifyResult.Success)
                    {
                        //todo: پرداخت موفق اینجاست
                        //todo: یادم باشه یه پرداخت موفق-البته بدون پرداخت!-منظور خرید موفق تو همون ای پی ای داشتیم که مثلا موجودی قبلیش کافی بود

                        //چون تمام سفارش هایی که برای یه درخواست ثبت میشه حتما کاربر و شیوه پرداخت یکسانی دارند، میتونیم از همون اولی نمونه بگیریم
                        Member member = new MemberBL().SelectOne(lstOrdersForCurrentPaymentRequest[0].MemberCode);
                        EOrderType orderType = lstOrdersForCurrentPaymentRequest[0].OrderType;

                        int profitOfOrdersForShowing = 0;
                        foreach (Order order in lstOrdersForCurrentPaymentRequest)
                        {
                            //مجموع هزینه ای که این سفارش داشته با احتساب تخفیف
                            int currentOrderOverallCost = new OrderBL().GetOverallOrderCost(order.Id, true);

                            //اگه پرداخت آزاد بود، هم اگه از موجودی فعالش استفاده شده بود کسر میکنیم و هم سودشو میریزیم 
                            if (order.OrderType == EOrderType.FreePayment)
                            {
                                //چقدر از موجودی قبلی کاربر استفاده شده
                                int memberUsedBalance = currentOrderOverallCost - order.OverallPayment;

                                //و اگه از موجودیش استفاده شده بود، بروز رسانی می کنیم
                                if (memberUsedBalance > 0)
                                {
                                    //چون پرداخت امنه یعنی باید کم شه از حسابش
                                    member.Balance -= memberUsedBalance;
                                }

                                //سود خرید رو به حسابش بریزیم
                                profitOfOrdersForShowing += HelperFunctionInBL.GetProfit(currentOrderOverallCost, StaticNembericInBL.OrderProfitPercentForCustomer);

                                //سود خریدو تو موجودیش هم تاثیر میدیم
                                member.Balance += HelperFunctionInBL.GetProfit(currentOrderOverallCost, StaticNembericInBL.OrderProfitPercentForCustomer);

                                new MemberBL().Update(member);
                            }
                            else //اگه پرداخت امن بود، مبلغ واریزی رو میریزیم تو حساب هوجی بوجیش. البته به صورت بلوکه شده میمونه تا ته فرایند خرید
                            {
                                //سود خرید رو حساب میکنیم ولی تو موجودی فعال تاثیر نمیدیم
                                //که مثلا بگیم بعد از پایان فرایند خرید به حساب شما ریخته خواهد شد

                                profitOfOrdersForShowing += HelperFunctionInBL.GetProfit(currentOrderOverallCost, StaticNembericInBL.OrderProfitPercentForCustomer);

                                //مبلغ واریزی رو میریزیم به حساب طرف تو هوجی بوجی
                                member.Balance += order.OverallPayment;

                                new MemberBL().Update(member);
                            }

                            //وضعیت سفارش به "در انتظار تائید فروشنده" بروزرسانی می شود
                            new OrderHistoryBL().Save(new OrderHistory()
                            {
                                Date = PersianDateTime.Now.Date.ToInt(),
                                Time = PersianDateTime.Now.TimeOfDay.ToInteger(),
                                OrderCode = order.Id,
                                OrderStatusCode = (byte)EOrderStatus.PendingApprovalSeller,
                                UserCode = User.Identity.GetUserId() ?? Request.UserHostAddress
                            });


                            //اگه کد تخفیفی داشت که یکبار مصرف بود، غیر فعال بشه
                            new StoreDiscountBL().InactiveIfIsDisposable(order.StoreDiscountCode);
                        }

                        return RedirectToAction("PaymentDetails", new PaymentResultViewModel()
                        {
                            IsSuccess = true,
                            Message = resultMessage,
                            TrackingCode = string.IsNullOrEmpty(refnum) ? "-" : refnum,
                            MemberProfit = profitOfOrdersForShowing,
                            IsProfitAddedToBalance = (orderType == EOrderType.FreePayment)
                        });
                    }

                    //اگه پرداخت انجام نشد
                    return RedirectToAction("PaymentDetails", new PaymentResultViewModel()
                    {
                        IsSuccess = false,
                        Message = resultMessage,
                        TrackingCode = (string.IsNullOrEmpty(refnum) || refnum == "-1") ? "-" : refnum
                    });
                }
                else
                {
                    byte? findedStatus = null;

                    switch (int.Parse(status))
                    {
                        case -99:
                            findedStatus = (byte)EPaymentResponseResultValues.Code_99;
                            resultMessage = "انصراف از پرداخت";
                            break;
                        case -88:
                            findedStatus = (byte)EPaymentResponseResultValues.Code_88;
                            resultMessage = "پرداخت ناموفق";
                            break;
                        case -77:
                            findedStatus = (byte)EPaymentResponseResultValues.Code_77;
                            resultMessage = "منقضی شدن زمان";
                            break;
                        case -66:
                            findedStatus = (byte)EPaymentResponseResultValues.Code_66;
                            resultMessage = "قبلا پرداخت شده است .";
                            break;
                    }

                    new PaymentResponseBL().Insert(new PaymentResponse()
                    {
                        TrackingCode = Convert.ToInt64(refnum),
                        PaymentResponseStatusCode = findedStatus ?? (byte)EPaymentResponseResultValues.OtherReason,
                        PaymentRequestCode = postedPaymentRequestCode,
                        VerifyDate = PersianDateTime.Now.Date.ToInt(),
                        VerifyTime = PersianDateTime.Now.TimeOfDay.ToShort(),
                    });

                    return RedirectToAction("PaymentDetails", new PaymentResultViewModel()
                    {
                        IsSuccess = false,
                        Message = resultMessage,
                        TrackingCode = (string.IsNullOrEmpty(refnum) || refnum == "-1") ? "-" : refnum
                    });
                }
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new
                    {
                        message = StaticString.Message_UnSuccessFull
                    });
                }
                catch (Exception)
                {
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new
                    {
                        message = StaticString.Message_UnSuccessFull
                    });
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new
                    {
                        message = StaticString.Message_UnSuccessFull
                    });
                }
                catch (Exception)
                {
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new
                    {
                        message = StaticString.Message_UnSuccessFull
                    });
                }
            }
        }



        /// <summary>
        /// از اونجایی که ممکنه حالتی پیش بیاد که نیاز یه پرداخت بانکی نباشه و تماما توسط موجودی قبلی پرداخت شود
        /// این اکشنو به وجود آوردیم که هر دو حالتو پوشش بده
        /// 
        /// توجه داشته باشید این اکشنو میشه بعد از بالا اومدن دستی هم با یوآر ال تغغیر داد!
        /// پس فرضا اگه کاربری خواس اسکرین شات بگیره بگه اینه خروجی من، ملاک همواره کد رهگیریه!
        /// </summary>
        /// <returns></returns>       
        //[Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult PaymentDetails(PaymentResultViewModel model)
        {
            return View(model);
        }

    }
}