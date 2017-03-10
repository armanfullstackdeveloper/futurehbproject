using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;
using Boundary.com.arianpal.merchant;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToPayments;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToOrder;
using DataModel.Entities.RelatedToPayments;
using DataModel.Entities.RelatedToProduct;
using DataModel.Entities.RelatedToStore;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using NHibernate;
using DataModel.Entities;
using System.Security.Cryptography;
using System.Text;
using Boundary.Model;

namespace Boundary.Controllers.Api
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        /// <summary>
        /// ثبت اولیه سفارش 
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public IHttpActionResult Register(PaymentViewModel payment)
        {
            if (payment.Bag == null || payment.Bag.Count <= 0)
                return Json(JsonResultHelper.FailedResultWithMessage("سبد خرید خالی است"));

            ISession session = null;
            try
            {
                #region getting customer Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                Member member = new MemberBL().GetSummaryForSession(userId);
                member = new MemberBL().SelectOne(member.Id); //چون مشخصات کاملشو میخوام
                if (member == null || member.Id <= 0)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion

                if (member.CityCode == null || string.IsNullOrEmpty(member.MobileNumber) ||
                    string.IsNullOrEmpty(member.PostalCode) || string.IsNullOrEmpty(member.Place) || string.IsNullOrEmpty(member.PhoneNumber))
                    return Json(JsonResultHelper.FailedResultWithMessage("مشخصات شما برای ثبت سفارش وارد نشده است"));

                bool haveValidDiscountCode = !string.IsNullOrEmpty(payment.DiscountCode);

                StoreDiscount storeDiscount = null;
                //رکورد مورد نظر که دارای این کد تخفیف است را پیدا می کنیم
                if (haveValidDiscountCode)
                {
                    storeDiscount = new StoreDiscountBL().SelectByCode(payment.DiscountCode);
                    if (storeDiscount == null || storeDiscount.IsActive == false)
                        haveValidDiscountCode = false;
                }

                // جمع کل قابل پرداخت بدون در نظر گرفتن موجودی فعال کاربر
                int overallPaymentWithoutConsideringMemberActiveBalance = 0;

                //برای اطمینان بیشتر خودم لیست محصولاتو لود میکنم
                List<StoreShopingBagDataModel> lstStoreShopingBag = new List<StoreShopingBagDataModel>();
                foreach (ShoppingBagViewModel model in payment.Bag)
                {
                    if (payment.Bag.Count(p => p.ProductCode == model.ProductCode) > 1)
                        return Json(JsonResultHelper.FailedResultWithMessage("خطا در سبد خرید. امکان ثبت کالای تکراری در سبد خرید وجود ندارد"));

                    if (model.Count <= 0)
                        return Json(JsonResultHelper.FailedResultWithMessage("خطا در سبد خرید. تعداد کالا درست وارد نشده است"));

                    Product product = new ProductBL().SelectOne(model.ProductCode);
                    if (!product.CanSend)
                        return Json(JsonResultHelper.FailedResultWithMessage("کالای مورد نظر قابلیت ارسال ندارد"));
                    if (!product.IsExist)
                        return Json(JsonResultHelper.FailedResultWithMessage("کالای مورد نظر موجود نیست"));
                    if (lstStoreShopingBag.Exists(s => s.Store.Id == product.StoreCode) == false)
                    {
                        lstStoreShopingBag.Add(new StoreShopingBagDataModel(new StoreBL().SelectOne(product.StoreCode)));
                    }

                    //اگه قیمت با تخفیف نداشت همون قیمت واقعیش هست ولی اگه داشت،قیمت با تخفیف قیمت واقعیش محسوب میشه
                    int realPrice = (int)((product.DiscountedPrice != null && product.DiscountedPrice > 0)
                                ? product.DiscountedPrice
                                : product.Price);

                    //برای اینکه ببینیم کد تخفیف بدردش میخوره یا نه
                    int meghdareTakhfif = 0;
                    if (haveValidDiscountCode && product.StoreCode == storeDiscount.StoreCode)
                    {
                        meghdareTakhfif = realPrice;
                        meghdareTakhfif *= storeDiscount.DiscountPercent;
                        meghdareTakhfif /= 100;
                        //
                        lstStoreShopingBag.Single(s => s.Store.Id == product.StoreCode).DiscountCode = storeDiscount.Id;
                    }

                    ProductInShopingBag productInShopingBag = new ProductInShopingBag()
                    {
                        ProductCode = product.Id,
                        Count = model.Count,
                        PostalCostInCountry = product.PostalCostInCountry ?? 0,
                        PostalCostInTown = product.PostalCostInTown ?? 0,
                        RealPrice = realPrice,
                        ProductCostConsideringDiscountCodeAndCount =
                            (meghdareTakhfif > 0) ? (realPrice - meghdareTakhfif) * model.Count : realPrice * model.Count
                    };
                    lstStoreShopingBag.Single(s => s.Store.Id == product.StoreCode).Products.Add(productInShopingBag);

                    overallPaymentWithoutConsideringMemberActiveBalance +=
                        productInShopingBag.ProductCostConsideringDiscountCodeAndCount;
                }

                //خب حالا باید هزینه ی ارسال رو هم اضافه کنم
                foreach (StoreShopingBagDataModel model in lstStoreShopingBag)
                {
                    //اگه فروشگاه تو شهر خودش نبود، که باید هزینه پستی حساب بشه
                    if (member.CityCode != model.Store.CityCode)
                    {
                        overallPaymentWithoutConsideringMemberActiveBalance += model.Products.Select(p => p.PostalCostInCountry).Max();
                    }
                    else
                    {
                        overallPaymentWithoutConsideringMemberActiveBalance += model.Products.Select(p => p.PostalCostInTown).Max();
                    }
                }

                //حالا باید ببینم از موجودی فعالش استفاده میشه یا نه
                int activeBalance = new OrderBL().GetMemberActivedBalance(member.Id);

                //موجودی فعال  - (جمع کل خرید   +   هزینه ارسال)   =   جمع کل قابل پرداخت    
                int overallPaymentWithConsideringMemberActiveBalance =
                    overallPaymentWithoutConsideringMemberActiveBalance - activeBalance;


                //چون به تعداد فروشگاه باید سفارش ثبت شه از ترنزاکشن استفاده می کنیم
                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                PaymentRequest paymentRequest = null;
                int finalPriceToPay;
                
                //کد مورد نیاز برای درخواست بعدی جهت ارجاع به بانک
                long paymentRequestCode;

                //حالت اصلی: که مبلغ قابل پرداخت با در نظر گرفتن کسر شدن موجودی فعال بیشتر از حداقل مقدار تراکنش هست***
                if (overallPaymentWithConsideringMemberActiveBalance >= StaticNemberic.MinimumTarakoneshValue)
                {
                    finalPriceToPay = overallPaymentWithConsideringMemberActiveBalance;


                    //خب حالا اول درخواستشو ثبت میکنم ، چون کلید خارجی تو سفارشه بهش نیاز دارم
                    paymentRequest = new PaymentRequest()
                    {
                        Description = "",
                        PaymentRequestStatusCode = (byte)ResultValues.Ready,
                        PaymentGateway = payment.PaymentGateway
                    };
                    paymentRequestCode = new PaymentRequestBL().InsertWhitOutCommitTransaction(paymentRequest,
                        session);

                    if (paymentRequestCode <= 0)
                    {
                        session.Transaction.Rollback();
                        session = null;
                        return Json(JsonResultHelper.FailedResultWithMessage());
                    }

                    //اگه کالاهای موجود در سبد خرید متعلق به چند فروشگاه باشند،به تعداد فروشگاهها باید سفارش ثبت شود
                    foreach (StoreShopingBagDataModel model in lstStoreShopingBag)
                    {
                        int postalCostForCurrentStore;
                        if (model.Store.CityCode == member.CityCode)
                        {
                            postalCostForCurrentStore = model.Products.Select(p => p.PostalCostInTown).Max();
                        }
                        else
                        {
                            postalCostForCurrentStore = model.Products.Select(p => p.PostalCostInCountry).Max();
                        }

                        int overallCostForCurrentStore =
                            model.Products.Sum(p => p.ProductCostConsideringDiscountCodeAndCount) + postalCostForCurrentStore;
                        int memberUsedBalance = 0;

                        //برای اینکه مشخص شود موجودی فعال چه مقدار برای هر سفارش استفاده شود-هدف تقسیم آن می باشد
                        if (activeBalance > 0)
                        {
                            //اگه هزینه ی این فروشگاه از موجودی فعال بیشتره همه رو از همین کم میکنم
                            if (overallCostForCurrentStore >= activeBalance)
                            {
                                memberUsedBalance = activeBalance;
                                activeBalance = 0;
                            }
                            else
                            {
                                memberUsedBalance = activeBalance - overallCostForCurrentStore;
                                activeBalance -= overallCostForCurrentStore;
                            }
                        }

                        long orderCode = new OrderBL().InsertWhitOutCommitTransaction(new Order()
                        {
                            PaymentRequestCode = paymentRequestCode,
                            MemberCode = member.Id,
                            StoreDiscountCode = model.DiscountCode,
                            SendingCost = postalCostForCurrentStore,
                            OrderType = payment.OrderType,
                            StoreCode = model.Store.Id,
                            //مجموع پرداختی در هر سفارش برابر است با هزینه ی کل کالاهای فروشگاه مورد نظر با احتساب تخفیف احتمالی که میخورن
                            //به علاوه هزینه پستی فروشگاه مورد نظر
                            //منهای اون مقداری که از موجودی فعال مشتری برداشت میشه
                            OverallPayment = overallCostForCurrentStore - memberUsedBalance,
                        }, session);


                        if (orderCode <= 0)
                        {
                            session.Transaction.Rollback();
                            session = null;
                            return Json(JsonResultHelper.FailedResultWithMessage());
                        }

                        foreach (ProductInShopingBag product in model.Products)
                        {
                            if (new OrderProductsBL().SaveWithoutCommit(new OrderProducts()
                            {
                                OrderCode = orderCode,
                                Count = product.Count,
                                CurrentPrice = product.RealPrice,
                                ProductCode = product.ProductCode

                            }, session) == false)
                            {
                                session.Transaction.Rollback();
                                session = null;
                                return Json(JsonResultHelper.FailedResultWithMessage());
                            }
                        }

                        //وضعیت اولیه را با "در انتظار پرداخت" مقداردهی میکنم
                        if (new OrderHistoryBL().SaveWithoutCommit(new OrderHistory()
                        {
                            Date = PersianDateTime.Now.Date.ToInt(),
                            Time = PersianDateTime.Now.TimeOfDay.ToInteger(),
                            OrderCode = orderCode,
                            OrderStatusCode = (byte)EOrderStatus.AwaitingPayment,
                            UserCode = RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress
                        }, session) == false)
                        {
                            session.Transaction.Rollback();
                            session = null;
                            return Json(JsonResultHelper.FailedResultWithMessage());
                        }

                        new OrderCustomerInfoBL().InsertWhitOutCommitTransaction(new OrderCustomerInfo()
                        {
                            OrderCode = orderCode,
                            PhoneNumber = member.PhoneNumber,
                            CityCode = member.CityCode,
                            Comments = "",
                            MobileNumber = member.MobileNumber,
                            Name = member.Name,
                            Place = member.Place,
                            PostalCode = member.Place,
                        }, session);
                    }
                }
                else
                {
                    //اگه موجودی فعالش بیشتر از هزینه ی سفارشه
                    if (overallPaymentWithConsideringMemberActiveBalance <= 0)
                    {
                        //لازم نیست مبلغی پرداخت بشه، یکسره سفارش ثبت بشه
                        int profitOfOrders = 0;

                        //اگه کالاهای موجود در سبد خرید متعلق به چند فروشگاه باشند،به تعداد فروشگاهها باید سفارش ثبت شود
                        foreach (StoreShopingBagDataModel model in lstStoreShopingBag)
                        {
                            int postalCostForCurrentStore;
                            if (model.Store.CityCode == member.CityCode)
                            {
                                postalCostForCurrentStore = model.Products.Select(p => p.PostalCostInTown).Max();
                            }
                            else
                            {
                                postalCostForCurrentStore = model.Products.Select(p => p.PostalCostInCountry).Max();
                            }

                            int overallCostForCurrentStore =
                                model.Products.Sum(p => p.ProductCostConsideringDiscountCodeAndCount) + postalCostForCurrentStore;

                            //دیگه اینجا نیاز نیست موجودی فعال رو تقسیم کنیم-چون میدانیم از کل هزینه بیشتر=>خب تمام هزینه ها رو میدیم
                            //و تهش موجودی قبلی استفاده شده برابر کل هزینه هاست

                            long orderCode = new OrderBL().InsertWhitOutCommitTransaction(new Order()
                            {
                                PaymentRequestCode = null,
                                MemberCode = member.Id,
                                SendingCost = postalCostForCurrentStore,
                                OrderType = payment.OrderType,
                                StoreDiscountCode = model.DiscountCode,
                                StoreCode=model.Store.Id,
                                //چون در این حالت همه از موجودی فعال کاربر کسر میشه پس پرداختی نداریم
                                OverallPayment = 0,
                            }, session);


                            if (orderCode <= 0)
                            {
                                session.Transaction.Rollback();
                                session = null;
                                return Json(JsonResultHelper.FailedResultWithMessage());
                            }

                            foreach (ProductInShopingBag product in model.Products)
                            {
                                if (new OrderProductsBL().SaveWithoutCommit(new OrderProducts()
                                {
                                    OrderCode = orderCode,
                                    Count = product.Count,
                                    CurrentPrice = product.RealPrice,
                                    ProductCode = product.ProductCode

                                }, session) == false)
                                {
                                    session.Transaction.Rollback();
                                    session = null;
                                    return Json(JsonResultHelper.FailedResultWithMessage());
                                }
                            }

                            //اینجا دیگه چون پرداخت نداریم با "در انتظار پرداخت" مقداردهی نمیکنم
                            if (new OrderHistoryBL().SaveWithoutCommit(new OrderHistory()
                            {
                                Date = PersianDateTime.Now.Date.ToInt(),
                                Time = PersianDateTime.Now.TimeOfDay.ToInteger(),
                                OrderCode = orderCode,
                                OrderStatusCode = (byte)EOrderStatus.PendingApprovalSeller, //در انتظار تائید فروشنده
                                UserCode = RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress
                            }, session) == false)
                            {
                                session.Transaction.Rollback();
                                session = null;
                                return Json(JsonResultHelper.FailedResultWithMessage());
                            }

                            new OrderCustomerInfoBL().InsertWhitOutCommitTransaction(new OrderCustomerInfo()
                            {
                                OrderCode = orderCode,
                                PhoneNumber = member.PhoneNumber,
                                CityCode = member.CityCode,
                                Comments = "",
                                MobileNumber = member.MobileNumber,
                                Name = member.Name,
                                Place = member.Place,
                                PostalCode = member.Place,
                            }, session);

                            #region بروز رسانی موجودی قبلی

                            //DataModel.Entities.Member currentMember = new MemberBL().SelectOneWhitOutCommitTransaction(member.Id,session);


                            //اگه پرداخت آزاد بود 
                            if (payment.OrderType == EOrderType.FreePayment)
                            {
                                //چون پرداخت آزاده یعنی باید کم شه از حسابش
                                member.Balance -= overallCostForCurrentStore;

                                //سود خرید رو به حسابش بریزیم
                                int temp = overallCostForCurrentStore;
                                temp *= StaticNembericInBL.OrderProfitPercentForCustomer;
                                temp /= 100;
                                profitOfOrders += temp;

                                //تو موجودیش هم تاثیر میدیم
                                member.Balance += temp;

                                //todo: حواسم باشه اینجا آپدیتش کردم
                                new MemberBL().UpdateWhitOutCommitTransaction(member, session);
                            }
                            else
                            {
                                //سود خرید رو به حساب میکنیم ولی تو موجودی فعال نشون نمیدیم
                                //که مثلا بگیم بعد از پایان فرایند خرید به حساب شما ریخته خواهد شد
                                int temp = overallCostForCurrentStore;
                                temp *= StaticNembericInBL.OrderProfitPercentForCustomer;
                                temp /= 100;
                                profitOfOrders += temp;
                            }

                            #endregion


                            //اگه کد تخفیفی داشت که یکبار مصرف بود، غیر فعال بشه
                            new StoreDiscountBL().InactiveIfIsDisposable(model.DiscountCode);
                        }

                        //اگه پرداخت آنلاینی در کار نبوده باشه و تماما از موجودی قبلی کاربر استفاده شده باشد
                        return Json(JsonResultHelper.SuccessResult("http://" + Request.Headers.Host +
                            string.Format("/Order/PaymentDetails?IsSuccess=true&Message={0}&TrackingCode={1}&MemberProfit={2}&IsProfitAddedToBalance={3}",
                            StaticString.Message_SuccessFull,
                            "-",
                            profitOfOrders,
                            (payment.OrderType == EOrderType.FreePayment) ? "true" : "false")));

                    }
                    else //چون حداقل مقدار تراکنش MinimumTarakoneshValue تومان می باشد
                    {
                        //تو این حالت هدف اینه که حتما 1000 تومان پرداخت داشته باشه و بقیش از موجودی فعالش کسر بشه
                        finalPriceToPay = StaticNemberic.MinimumTarakoneshValue;


                        //خب حالا اول درخواستشو ثبت میکنم ، چون کلید خارجی تو سفارشه بهش نیاز دارم
                        paymentRequest = new PaymentRequest()
                        {
                            Description = "",
                            PaymentRequestStatusCode = (byte)ResultValues.Ready,
                            PaymentGateway = payment.PaymentGateway
                        };
                        paymentRequestCode = new PaymentRequestBL().InsertWhitOutCommitTransaction(paymentRequest,
                            session);

                        if (paymentRequestCode <= 0)
                        {
                            session.Transaction.Rollback();
                            session = null;
                            return Json(JsonResultHelper.FailedResultWithMessage());
                        }


                        int forceToPayPrice = StaticNemberic.MinimumTarakoneshValue;
                        //اگه کالاهای موجود در سبد خرید متعلق به چند فروشگاه باشند،به تعداد فروشگاهها باید سفارش ثبت شود
                        foreach (StoreShopingBagDataModel model in lstStoreShopingBag)
                        {
                            int postalCostForCurrentStore;
                            if (model.Store.CityCode == member.CityCode)
                            {
                                postalCostForCurrentStore = model.Products.Select(p => p.PostalCostInTown).Max();
                            }
                            else
                            {
                                postalCostForCurrentStore = model.Products.Select(p => p.PostalCostInCountry).Max();
                            }

                            int overallCostForCurrentStore =
                                model.Products.Sum(p => p.ProductCostConsideringDiscountCodeAndCount) + postalCostForCurrentStore;

                            //مبلغ پرداختی از مبلغی که باید پرداخت شود
                            int payed = 0;

                            if (forceToPayPrice > 0)
                            {
                                //اگه هزینه ی این فروشگاه از مبلغی که باید پرداخت شود بیشتره همه رو از همین کم میکنم
                                if (overallCostForCurrentStore >= forceToPayPrice)
                                {
                                    payed = forceToPayPrice;
                                    forceToPayPrice = 0;
                                }
                                else
                                {
                                    payed = forceToPayPrice - overallCostForCurrentStore;
                                    forceToPayPrice -= overallCostForCurrentStore;
                                }
                            }

                            long orderCode = new OrderBL().InsertWhitOutCommitTransaction(new Order()
                            {
                                PaymentRequestCode = paymentRequestCode,
                                MemberCode = member.Id,
                                SendingCost = postalCostForCurrentStore,
                                OrderType = payment.OrderType,
                                OverallPayment = payed,
                                StoreCode = model.Store.Id,
                                StoreDiscountCode = model.DiscountCode,
                            }, session);


                            if (orderCode <= 0)
                            {
                                session.Transaction.Rollback();
                                session = null;
                                return Json(JsonResultHelper.FailedResultWithMessage());
                            }

                            //fill OrderProducts
                            foreach (ProductInShopingBag product in model.Products)
                            {
                                if (new OrderProductsBL().SaveWithoutCommit(new OrderProducts()
                                {
                                    OrderCode = orderCode,
                                    Count = product.Count,
                                    CurrentPrice = product.RealPrice,
                                    ProductCode = product.ProductCode

                                }, session) == false)
                                {
                                    session.Transaction.Rollback();
                                    session = null;
                                    return Json(JsonResultHelper.FailedResultWithMessage());
                                }
                            }

                            //وضعیت اولیه را با "در انتظار پرداخت" مقداردهی میکنم
                            if (new OrderHistoryBL().SaveWithoutCommit(new OrderHistory()
                            {
                                Date = PersianDateTime.Now.Date.ToInt(),
                                Time = PersianDateTime.Now.TimeOfDay.ToInteger(),
                                OrderCode = orderCode,
                                OrderStatusCode = (byte)EOrderStatus.AwaitingPayment,
                                UserCode = RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress
                            }, session) == false)
                            {
                                session.Transaction.Rollback();
                                session = null;
                                return Json(JsonResultHelper.FailedResultWithMessage());
                            }

                            new OrderCustomerInfoBL().InsertWhitOutCommitTransaction(new OrderCustomerInfo()
                            {
                                OrderCode = orderCode,
                                PhoneNumber = member.PhoneNumber,
                                CityCode = member.CityCode,
                                Comments = "",
                                MobileNumber = member.MobileNumber,
                                Name = member.Name,
                                Place = member.Place,
                                PostalCode = member.Place,
                            }, session);
                        }
                    }
                }


                //خب وقتی تا اینجا اومدیم یعنی پرداخت آنلاین داریم حتما
                //این کد درخواستو به کاربر رو خروجی میدیم تا با درخواست بعدی اول مبلغ تراکنش لود شه بعد به بانک ارجاع میدیم
                //این دو مرحله ای بودن به خاطر اینه که اندروید رو هم داشته باشیم و کدها یکی باشه
                return Json(JsonResultHelper.SuccessResult(paymentRequestCode));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    session?.Transaction.Rollback();
                    session = null;
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ??
                        HttpContext.Current.Request.UserHostAddress, JObject.FromObject(payment).ToString());
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
                    session?.Transaction.Rollback();
                    session = null;
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ??
                        HttpContext.Current.Request.UserHostAddress, JObject.FromObject(payment).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            finally
            {
                session?.Transaction.Commit();
            }
        }

        /// <summary>
        /// اگه کد تخفیف معتبر باشه خروجی این اکشن مبلغ کل پرداختی جدید با توجه به تاثیر کد تخفیف خواهد بود
        /// </summary>
        /// <param name="discountCode"></param>
        /// <param name="bag"></param>
        /// <returns></returns>
        [Route("VerifyDiscountCode")]
        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public IHttpActionResult VerifyDiscountCode([FromUri]string discountCode, [FromBody]List<ShoppingBagViewModel> bag)
        {
            if (bag == null || bag.Count <= 0)
                return Json(JsonResultHelper.FailedResultWithMessage("سبد خرید خالی است"));
            if (string.IsNullOrEmpty(discountCode))
                return Json(JsonResultHelper.FailedResultWithMessage());

            try
            {
                #region getting customer Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                DataModel.Entities.Member member = new MemberBL().GetSummaryForSession(userId);
                if (member == null || member.Id <= 0)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion

                if (member.CityCode == null)
                    return Json(JsonResultHelper.FailedResultWithMessage("شهر شما انتخاب نشده است"));


                //برای اطمینان بیشتر خودم لیست محصولاتو لود میکنم
                List<Product> lstProducts = new ProductBL().GetProductsByCodes(bag.Select(b => b.ProductCode).ToList());

                //رکورد مورد نظر که دارای این کد تخفیف است را پیدا می کنیم
                StoreDiscount storeDiscount = new StoreDiscountBL().SelectByCode(discountCode);
                if (storeDiscount == null)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                //اگه کد تخفیف بدرد هیچ کدوم از کالاهای سبد خرید نمیخوره
                if (lstProducts.Exists(p => p.StoreCode == storeDiscount.StoreCode) == false)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                //مجموع قیمت کالاهای موجود در سبد خرید با در نظر گرفتن کد تخفیف
                int overallProductPriceConsideringDiscountCode = 0;

                //لیست فروشگاهها رو میخوایم برای محاسبه ی هزینه ی پستی
                List<long> lstUniqueStoreCode = new List<long>();

                foreach (Product product in lstProducts)
                {
                    if (!product.CanSend)
                        return Json(JsonResultHelper.FailedResultWithMessage("کالای مورد نظر قابلیت ارسال ندارد"));

                    if (!lstUniqueStoreCode.Contains(product.StoreCode))
                        lstUniqueStoreCode.Add(product.StoreCode);

                    //اگه قیمت با تخفیف نداشت همون قیمت واقعیش هست ولی اگه داشت،قیمت با تخفیف قیمت واقعیش محسوب میشه
                    int realPrice;

                    //اگه شامل کد تخفیف میشه
                    if (product.StoreCode == storeDiscount.StoreCode)
                    {
                        //اگه قیمت با تخفیف داره
                        if (product.DiscountedPrice != null && product.DiscountedPrice > 0)
                        {
                            realPrice = (int)product.DiscountedPrice;
                        }
                        else
                        {
                            realPrice = product.Price;
                        }

                        int meghdareTakhfif = realPrice * (storeDiscount.DiscountPercent / 100);
                        //قیمت تخفیف خورده رو در تعداد ضرب میکنم
                        overallProductPriceConsideringDiscountCode += ((realPrice - meghdareTakhfif) * bag.Single(p => p.ProductCode == product.Id).Count);
                    }
                    else //اگه شامل کد تخفیف نمیشه
                    {
                        if (product.DiscountedPrice != null && product.DiscountedPrice > 0)
                        {
                            realPrice = (int)product.DiscountedPrice;
                        }
                        else
                        {
                            realPrice = product.Price;
                        }
                        overallProductPriceConsideringDiscountCode += (realPrice * bag.Single(p => p.ProductCode == product.Id).Count);
                    }
                }

                //مبلغ کل قابل پرداختی
                int overallPayment = overallProductPriceConsideringDiscountCode;

                //خب حالا باید هزینه ی ارسال رو هم اضافه کنم
                foreach (long item in lstUniqueStoreCode)
                {
                    Store store = new StoreBL().SelectOne(item);

                    //اگه فروشگاه تو شهر خودش نبود که باید هزینه پستی حساب بشه
                    if (member.CityCode != store.CityCode)
                    {
                        overallPayment += lstProducts.Where(p => p.StoreCode == item)
                            .OrderByDescending(p => p.PostalCostInCountry).Select(p => p.PostalCostInCountry).FirstOrDefault() ?? 0;
                    }
                    else //و گرنه هزینه پیک
                    {
                        overallPayment += lstProducts.Where(p => p.StoreCode == item)
                            .OrderByDescending(p => p.PostalCostInTown).Select(p => p.PostalCostInTown).FirstOrDefault() ?? 0;
                    }
                }

                //حالا باید ببینم از موجودی فعالش استفاده میشه یا نه
                int activeBalance = new OrderBL().GetMemberActivedBalance(member.Id);

                //موجودی فعال  - (جمع کل خرید   +   هزینه ارسال)   =   جمع کل قابل پرداخت
                overallPayment -= activeBalance;

                if (overallPayment >= StaticNemberic.MinimumTarakoneshValue)
                {
                    return Json(JsonResultHelper.SuccessResult(overallPayment));
                }
                else
                {
                    if (overallPayment <= 0)
                    {
                        return Json(JsonResultHelper.SuccessResult(0));
                    }
                    else //چون حداقل مقدار تراکنش MinimumTarakoneshValue تومان می باشد
                    {
                        return Json(JsonResultHelper.SuccessResult(StaticNemberic.MinimumTarakoneshValue));
                    }
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
                            Name = HelperFunctionInBL.GetVariableName(() => discountCode),
                            Value = discountCode
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => bag),
                            Value = JArray.FromObject(bag).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => discountCode),
                            Value = discountCode
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => bag),
                            Value = JArray.FromObject(bag).ToString()
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
