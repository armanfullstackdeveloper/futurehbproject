using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.BussinesLogics.RelatedToPayments;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Entities.RelatedToOrder;
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToOrder
{

    public class OrderBL : GenericRepository<Order, long>
    {
        private IDbConnection _db;

        /// <summary>
        /// اگه کاربر بخواد خرید جدیدی بکنه
        /// و از موجودی قبلیش استفاده بکنه
        /// باید چک کنم که بر خرید های دیگه تاثیر نذاره
        /// برای همین با این تابع هزینه ی بلوکه شده ی خرید های قبلی رو درمیارم
        /// </summary>
        /// <param name="memberCode"></param>
        /// <returns></returns>
        public int GetMemberBlokedBalance(long memberCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@memberCode", memberCode);
                int price = _db.Query<int>("Order_SelectMemberBlockedBalance", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return price;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, memberCode.ToString());
            }
        }

        public int GetMemberActivedBalance(long memberCode)
        {
            Member fullMember = new MemberBL().SelectOne(memberCode);
            int blockedBalance = GetMemberBlokedBalance(memberCode);
            int activeBalance = fullMember.Balance - blockedBalance;
            return (activeBalance>0)?activeBalance:0;
        }

        public List<Order> GetOrdersByPaymentRequestCode(long postedPaymentRequestCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@paymentRequestCode", postedPaymentRequestCode);
                List<Order> lst = _db.Query<Order>("Order_GetAllByPaymentRequestCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, postedPaymentRequestCode.ToString());
            }
        }

        /// <summary>
        /// این اکشن مجموع هزینه ای که برای یک سفارش شده است را برمی گرداند
        /// همچنین میتوانید فیلتر کنید که اگه کد تخفیف داشته، تاثیر داده بشه یا نه
        /// شامل مجموع هزینه کالاها+هزینه ی پستی و موجودی فعال استفاده شده کاربر می باشد
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="considerDiscountCode"></param>
        /// <returns></returns>
        public int GetOverallOrderCost(long orderCode, bool considerDiscountCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@orderCode", orderCode);
                parameters.Add("@considerDiscountCode", considerDiscountCode);

                int cost = _db.Query<int>("Order_GetOverallOrderCost", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return cost;
            }
            catch (Exception ex)
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
                        Name = HelperFunctionInBL.GetVariableName(() => considerDiscountCode),
                        Value = considerDiscountCode.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public int SelectCountOfUsingDiscountCode(long discountCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                int result = _db.Query<int>("SELECT count(Id) FROM [Order] where [StoreDiscountCode]=@discountCode", new { discountCode }).SingleOrDefault();
                EnsureCloseConnection(_db);
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, discountCode.ToString());
            }
        }

        /// <summary>
        /// با این تابع چک میکنم که با توجه به وضعیت جاری سفارش
        /// آیا مشتری میتواند وضعیت آن را بروزرسانی کند
        /// و اگر میتواند، به چه وضعیت هایی میتواند
        /// اگر نه هم که نال برگشت داده می شود
        /// </summary>
        /// <param name="currentStatus"></param>
        /// <returns></returns>
        public List<DropDownItemsModel> CheckMembersEditableStatus(EOrderStatus currentStatus)
        {
            switch (currentStatus)
            {
                case EOrderStatus.AwaitingPayment:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.RefuseByMember,
                            Text = "انصراف از خرید"
                        }
                    };
                case EOrderStatus.PendingApprovalSeller:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.RefuseByMember,
                            Text = "انصراف از خرید"
                        }
                    };
                case EOrderStatus.RejectedBySeller:
                    return null;
                case EOrderStatus.VerifiedSeller:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.RefuseByMember,
                            Text = "انصراف از خرید"
                        }
                    };
                case EOrderStatus.Posted:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.Received,
                            Text = "دریافت شده"
                        },new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.BackShaken,
                            Text = "برگشت زدن"
                        },new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.RefuseByMember,
                            Text = "انصراف از خرید"
                        },
                    };
                case EOrderStatus.BackShaken:
                    return null;
                case EOrderStatus.Received:
                    return null;
                case EOrderStatus.Closed:
                    return null;
                case EOrderStatus.RefuseBySeller:
                    return null;
                case EOrderStatus.RefuseByMember:
                    return null;
                default:
                    return null;
            }
        }



        /// <summary>
        /// با این تابع چک میکنم که با توجه به وضعیت جاری سفارش
        /// آیا فروشنده میتواند وضعیت آن را بروزرسانی کند
        /// و اگر میتواند، به چه وضعیت هایی میتواند
        /// اگر نه هم که نال برگشت داده می شود
        /// </summary>
        /// <param name="currentStatus"></param>
        /// <returns></returns>
        public List<DropDownItemsModel> CheckSellersEditableStatus(EOrderStatus currentStatus)
        {
            switch (currentStatus)
            {
                case EOrderStatus.AwaitingPayment:
                    return null;
                case EOrderStatus.PendingApprovalSeller:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.VerifiedSeller,
                            Text = "تائید سفارش"
                        },new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.RejectedBySeller,
                            Text = "رد کردن سفارش"
                        }
                    };
                case EOrderStatus.RejectedBySeller:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.VerifiedSeller,
                            Text = "تائید سفارش"
                        }
                    };
                case EOrderStatus.VerifiedSeller:
                    return new List<DropDownItemsModel>()
                    {
                        new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.Posted,
                            Text = "ارسال شده"
                        },new DropDownItemsModel()
                        {
                            Value = (long) EOrderStatus.RefuseBySeller,
                            Text = "انصراف از سفارش"
                        }
                    };
                case EOrderStatus.Posted:
                    return null;
                case EOrderStatus.BackShaken:
                    return null;
                case EOrderStatus.Received:
                    return null;
                case EOrderStatus.Closed:
                    return null;
                case EOrderStatus.RefuseBySeller:
                    return null;
                case EOrderStatus.RefuseByMember:
                    return null;
                default:
                    return null;
            }
        }


        /// <summary>
        /// لیست سفارشات اعضا
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="pageNumer"></param>
        /// <param name="rowsPage"></param>
        /// <returns></returns>
        public List<OrderViewModelForMembers> GetMemberOrders(long memberCode, int? pageNumer = 1, int? rowsPage = 10)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@memberCode", memberCode);
                parameters.Add("@PageNumber", pageNumer);
                parameters.Add("@RowspPage", rowsPage);

                List<Order> lstOrders = _db.Query<Order>("Order_GetOrdersOfMember", parameters, commandType: CommandType.StoredProcedure).ToList();

                List<OrderViewModelForMembers> lst = new List<OrderViewModelForMembers>();
                foreach (Order order in lstOrders)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@orderCode", order.Id);
                    List<OrderProductsViewModel> lstProducts = _db.Query<OrderProductsViewModel>("OrderProducts_GetAllDetailesByOrderCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                    List<OrderHistory> lstOrderHistories = new OrderHistoryBL().GetAllForOrder(order.Id);
                    OrderHistory orderHistoryOfAfterPayment = lstOrderHistories.SingleOrDefault(o => o.OrderStatusCode == 1); //در انتظار تائید فروشنده

                    //age be in vaziyat nareside bashe yani aslan pardakht nashode! pas aslan neshonesh nemidim
                    if(orderHistoryOfAfterPayment==null)
                        continue;

                    OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();

                    lst.Add(new OrderViewModelForMembers()
                    {
                        OrderCode = order.Id,
                        OrderTypeName = HelperFunctionInBL.GetOrderTypeName(order.OrderType),
                        PostalCost = order.SendingCost,
                        OverallPayment = order.OverallPayment,
                        Date = orderHistoryOfAfterPayment.Date,
                        Time = orderHistoryOfAfterPayment.Time,
                        OverallDiscount = ((lstProducts.Sum(p => p.OverallPrice) + order.SendingCost) - order.OverallPayment),
                        ProductDetailes = lstProducts,
                        ShopName = _db.Query<string>("SELECT Name FROM [Store] where [Id]=(select StoreCode from Product where Id=@productCode)", new { productCode = lstProducts[0].ProductCode }).SingleOrDefault(),
                        StatusName = new OrderStatusBL().SelectOne(orderHistoryOfLastStatus.OrderStatusCode).Name,
                        EditableStatus = CheckMembersEditableStatus((EOrderStatus)orderHistoryOfLastStatus.OrderStatusCode),
                        TrackingCode = order.TrackingCode,
                        OrderSendingTypeName = (order.OrderSendingTypeCode!=null)?new OrderSendingTypeBL().SelectOne((byte) order.OrderSendingTypeCode).Name:"-"
                    });
                }
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                        Value = pageNumer.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => rowsPage),
                        Value = rowsPage.ToString()
                    },                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => memberCode),
                        Value = memberCode.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }


        /// <summary>
        /// لیست سفارشات فروشگاه
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="pageNumer"></param>
        /// <param name="rowsPage"></param>
        /// <returns></returns>
        public List<OrderViewModelForStores> GetStoreOrders(long storeCode, int? pageNumer = 1, int? rowsPage = 10)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@PageNumber", pageNumer);
                parameters.Add("@RowspPage", rowsPage);

                List<Order> lstOrders = _db.Query<Order>("Order_GetOrdersOfStore", parameters, commandType: CommandType.StoredProcedure).ToList();

                List<OrderViewModelForStores> lst = new List<OrderViewModelForStores>();
                foreach (Order order in lstOrders)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@orderCode", order.Id);
                    List<OrderProductsViewModel> lstProducts = _db.Query<OrderProductsViewModel>("OrderProducts_GetAllDetailesByOrderCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                    List<OrderHistory> lstOrderHistories = _db.Query<OrderHistory>("SELECT * FROM [OrderHistory] where [OrderCode]=@orderCode", new { orderCode = order.Id }).ToList();
                    OrderHistory orderHistoryOfAfterPayment = lstOrderHistories.SingleOrDefault(o => o.OrderStatusCode == (byte)EOrderStatus.PendingApprovalSeller); //در انتظار تائید فروشنده
                    
                    //age be in vaziyat nareside bashe yani aslan pardakht nashode! pas aslan neshonesh nemidim
                    if(orderHistoryOfAfterPayment==null)
                        continue;

                    OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();

                    StoreDiscount storeDiscount = null;
                    int overallProductCostConsideringDiscounts; //مجموع هزینه کالاها با احتساب کد تخفیف اگر دارد

                    if (order.StoreDiscountCode != null)
                    {
                        storeDiscount = new StoreDiscountBL().SelectOne((long)order.StoreDiscountCode);
                        overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice - HelperFunctionInBL.GetProfit(p.OverallPrice, storeDiscount.DiscountPercent));
                    }
                    else
                    {
                        overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice);
                    }

                    int overallOrderCost = overallProductCostConsideringDiscounts + order.SendingCost; //مجموع هزینه کالاها در سفارش = مجموع هزینه سفارش  + PostalCost

                    lst.Add(new OrderViewModelForStores()
                    {
                        OrderCode = order.Id,
                        OrderTypeName = HelperFunctionInBL.GetOrderTypeName(order.OrderType),
                        PostalCost = order.SendingCost,
                        Date = orderHistoryOfAfterPayment.Date,
                        Time = orderHistoryOfAfterPayment.Time,
                        OverallIncome = overallOrderCost, //سود هوجی بوجی از سفارش اینجا تاثیر نمیذاره
                        ProductDetailes = lstProducts,
                        MemberName = _db.Query<string>("SELECT Name FROM [Member] where [Id]=@memberCode", new { memberCode = order.MemberCode }).SingleOrDefault(),
                        StatusName = new OrderStatusBL().SelectOne(orderHistoryOfLastStatus.OrderStatusCode).Name,
                        EditableStatus = CheckSellersEditableStatus((EOrderStatus)orderHistoryOfLastStatus.OrderStatusCode),
                        TrackingCode = order.TrackingCode,
                        OrderSendingTypeName = (order.OrderSendingTypeCode != null) ? new OrderSendingTypeBL().SelectOne((byte)order.OrderSendingTypeCode).Name : "-"
                    });
                }
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                        Value = pageNumer.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => rowsPage),
                        Value = rowsPage.ToString()
                    },                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                        Value = storeCode.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }


        public List<OrderViewModelForAdmins> GetAllOrdersForAdmin(int? pageNumer = 1)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumer);
                parameters.Add("@RowspPage", StaticNembericInBL.CountOfItemsInAdminPages);

                List<Order> lstOrders = _db.Query<Order>("Order_GetAllOrders", parameters, commandType: CommandType.StoredProcedure).ToList();

                List<OrderViewModelForAdmins> lst = new List<OrderViewModelForAdmins>();
                foreach (Order order in lstOrders)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@orderCode", order.Id);
                    List<OrderProductsViewModel> lstProducts = _db.Query<OrderProductsViewModel>("OrderProducts_GetAllDetailesByOrderCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                    List<OrderHistory> lstOrderHistories = _db.Query<OrderHistory>("SELECT * FROM [OrderHistory] where [OrderCode]=@orderCode", new { orderCode = order.Id }).ToList();
                    OrderHistory orderHistoryOfAfterPayment = lstOrderHistories.SingleOrDefault(o => o.OrderStatusCode == (byte)EOrderStatus.PendingApprovalSeller); //در انتظار تائید فروشنده                    
                    OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();

                    //برای زمانی که پرداخت انجام نشه
                    if (orderHistoryOfAfterPayment == null) 
                        orderHistoryOfAfterPayment = orderHistoryOfLastStatus;

                    StoreDiscount storeDiscount = null;
                    int overallProductCostConsideringDiscounts; //مجموع هزینه کالاها با احتساب کد تخفیف اگر دارد

                    if (order.StoreDiscountCode != null)
                    {
                        storeDiscount = new StoreDiscountBL().SelectOne((long)order.StoreDiscountCode);
                        overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice - HelperFunctionInBL.GetProfit(p.OverallPrice, storeDiscount.DiscountPercent));
                    }
                    else
                    {
                        overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice);
                    }

                    int overallOrderCost = overallProductCostConsideringDiscounts + order.SendingCost; //مجموع هزینه کالاها در سفارش = مجموع هزینه سفارش  + PostalCost

                    lst.Add(new OrderViewModelForAdmins()
                    {
                        OrderCode = order.Id,
                        OrderType = order.OrderType,
                        Date = (orderHistoryOfAfterPayment!=null)?orderHistoryOfAfterPayment.Date:0,
                        Time = (orderHistoryOfAfterPayment!=null)?orderHistoryOfAfterPayment.Time:0,
                        ShopName = _db.Query<string>("SELECT Name FROM [Store] where [Id]=(select StoreCode from Product where Id=@productCode)", new { productCode = lstProducts[0].ProductCode }).SingleOrDefault(),
                        MemberName = _db.Query<string>("SELECT Name FROM [Member] where [Id]=@memberCode", new { memberCode = order.MemberCode }).SingleOrDefault(),
                        OrderStatus = new OrderStatusBL().SelectOne(orderHistoryOfLastStatus.OrderStatusCode),
                        OverallPayment = order.OverallPayment,
                        OverallOrderCost = overallOrderCost, //todo: farada bayad tebghe in ba foroshandeha tasviye hesab konam
                        MemberUsedBalance = overallOrderCost - order.OverallPayment,
                        IsPony = (new HBPaymentToStoreBL().SelectOne(order.Id) != null),
                        CanPony = CheckCanPony(order.OrderType, orderHistoryOfLastStatus.OrderStatusCode),
                    });
                }
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                        Value = pageNumer.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }


        /// <summary>
        /// آیا با توجه به وضعیت فعلی سفارش امکان تسویه حساب با فروشنده وجود داره یا نه
        /// </summary>
        /// <param name="orderType"></param>
        /// <param name="orderStatusCode"></param>
        /// <returns></returns>
        private bool CheckCanPony(EOrderType orderType, byte orderStatusCode)
        {
            if (orderType == EOrderType.FreePayment) //اگه پرداخت آزاد باشه
            {
                //اگه از طرف فروشنده رد نشده باشه، میشه پرداخت کرد
                if (orderStatusCode != (byte)EOrderStatus.RejectedBySeller)
                    return true;
                return false;
            }
            else
            {
                if (orderStatusCode == (byte)EOrderStatus.Received) //اگه ته فرایند خرید باشیم میشه
                    return true;
                return false;
            }
        }


        public OrderViewModelForAdmins GetOneOrderDetailesForAdmin(long orderCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                Order order = new OrderBL().SelectOne(orderCode);
                if (order == null)
                {
                    return null;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@orderCode", order.Id);
                List<OrderProductsViewModel> lstProducts = _db.Query<OrderProductsViewModel>("OrderProducts_GetAllDetailesByOrderCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                List<OrderHistory> lstOrderHistories = _db.Query<OrderHistory>("SELECT * FROM [OrderHistory] where [OrderCode]=@orderCode", new { orderCode = order.Id }).ToList();
                OrderHistory orderHistoryOfAfterPayment = lstOrderHistories.Single(o => o.OrderStatusCode == (byte)EOrderStatus.PendingApprovalSeller); //در انتظار تائید فروشنده
                OrderHistory orderHistoryOfLastStatus =
                    lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();

                StoreDiscount storeDiscount = null;
                int overallProductCostConsideringDiscounts; //مجموع هزینه کالاها با احتساب کد تخفیف اگر دارد

                if (order.StoreDiscountCode != null)
                {
                    storeDiscount = new StoreDiscountBL().SelectOne((long)order.StoreDiscountCode);
                    overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice - HelperFunctionInBL.GetProfit(p.OverallPrice, storeDiscount.DiscountPercent));
                }
                else
                {
                    overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice);
                }

                int overallOrderCost = overallProductCostConsideringDiscounts + order.SendingCost; //مجموع هزینه کالاها در سفارش = مجموع هزینه سفارش  + PostalCost

                List<OrderHistoryViewModel> lstHistories = new List<OrderHistoryViewModel>();
                foreach (OrderHistory orderHistory in lstOrderHistories)
                {
                    User user = new UserBL().GetById(orderHistory.UserCode);
                    Role role = null;
                    if (user != null)
                        role = new RoleBL().SelectOne((int)user.RoleCode);

                    lstHistories.Add(new OrderHistoryViewModel()
                    {
                        Date = orderHistory.Date,
                        OrderCode = orderHistory.OrderCode,
                        OrderStatus = (EOrderStatus)orderHistory.OrderStatusCode,
                        Time = orderHistory.Time,
                        UserCode = orderHistory.UserCode,
                        UserRoleName = (role != null) ? role.Name : "-"
                    });
                }
                OrderViewModelForAdmins orderViewModel = new OrderViewModelForAdmins()
                {
                    OrderCode = order.Id,
                    OrderType = order.OrderType,
                    PostalCost = order.SendingCost,
                    Date = orderHistoryOfAfterPayment.Date,
                    Time = orderHistoryOfAfterPayment.Time,
                    ShopName = _db.Query<string>("SELECT Name FROM [Store] where [Id]=(select StoreCode from Product where Id=@productCode)", new { productCode = lstProducts[0].ProductCode }).SingleOrDefault(),
                    ProductDetailes = lstProducts,
                    MemberName = _db.Query<string>("SELECT Name FROM [Member] where [Id]=@memberCode", new { memberCode = order.MemberCode }).SingleOrDefault(),
                    OrderStatus = new OrderStatusBL().SelectOne(orderHistoryOfLastStatus.OrderStatusCode),
                    OverallPayment = order.OverallPayment,
                    OverallDiscount = ((lstProducts.Sum(p => p.OverallPrice) + order.SendingCost) - order.OverallPayment), //یعنی اگرم محصولات تخفیفی داشتند اونا رو در نظر نگیریم تا تو تخفیف کل بیان
                    OverallOrderCost = overallOrderCost,
                    MemberUsedBalance = overallOrderCost - order.OverallPayment,
                    StoreDiscount = storeDiscount ?? new StoreDiscount(),
                    OverallOrderCostWithConsideringDiscount = overallOrderCost,
                    OverallOrderCostWithoutConsideringDiscount = lstProducts.Sum(p => p.OverallPrice) + order.SendingCost,
                    OverallProductCostWithConsideringDiscount = overallProductCostConsideringDiscounts,
                    OverallProductCostWithoutConsideringDiscount = lstProducts.Sum(p => p.OverallPrice),
                    IsPony = (new HBPaymentToStoreBL().SelectOne(order.Id) != null),
                    CanPony = CheckCanPony(order.OrderType, orderHistoryOfLastStatus.OrderStatusCode),
                    OrderHistories = lstHistories,
                    TrackingCode = order.TrackingCode,
                    OrderSendingTypeName = (order.OrderSendingTypeCode != null) ? new OrderSendingTypeBL().SelectOne((byte)order.OrderSendingTypeCode).Name : "-"
                };

                EnsureCloseConnection(_db);
                return orderViewModel;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => orderCode),
                        Value = orderCode.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public List<OrderViewModelForAdmins> GetAdminReviewPendingOrders(int pageNumer)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumer);
                parameters.Add("@RowspPage", StaticNembericInBL.CountOfItemsInAdminPages);

                List<Order> lstOrders = _db.Query<Order>("Order_GetAdminReviewPendingOrders", parameters, commandType: CommandType.StoredProcedure).ToList();

                List<OrderViewModelForAdmins> lst = new List<OrderViewModelForAdmins>();
                foreach (Order order in lstOrders)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@orderCode", order.Id);
                    List<OrderProductsViewModel> lstProducts = _db.Query<OrderProductsViewModel>("OrderProducts_GetAllDetailesByOrderCode", parameters, commandType: CommandType.StoredProcedure).ToList();
                    List<OrderHistory> lstOrderHistories = _db.Query<OrderHistory>("SELECT * FROM [OrderHistory] where [OrderCode]=@orderCode", new { orderCode = order.Id }).ToList();
                    OrderHistory orderHistoryOfAfterPayment = lstOrderHistories.Single(o => o.OrderStatusCode == 1); //در انتظار تائید فروشنده
                    OrderHistory orderHistoryOfLastStatus =
                        lstOrderHistories.OrderByDescending(o => o.Date).ThenByDescending(o => o.Time).First();

                    StoreDiscount storeDiscount = null;
                    int overallProductCostConsideringDiscounts; //مجموع هزینه کالاها با احتساب کد تخفیف اگر دارد

                    if (order.StoreDiscountCode != null)
                    {
                        storeDiscount = new StoreDiscountBL().SelectOne((long)order.StoreDiscountCode);
                        overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice - HelperFunctionInBL.GetProfit(p.OverallPrice, storeDiscount.DiscountPercent));
                    }
                    else
                    {
                        overallProductCostConsideringDiscounts = lstProducts.Sum(p => p.OverallPrice);
                    }

                    int overallOrderCost = overallProductCostConsideringDiscounts + order.SendingCost; //مجموع هزینه کالاها در سفارش = مجموع هزینه سفارش  + PostalCost

                    lst.Add(new OrderViewModelForAdmins()
                    {
                        OrderCode = order.Id,
                        OrderType = order.OrderType,
                        Date = orderHistoryOfAfterPayment.Date,
                        Time = orderHistoryOfAfterPayment.Time,
                        ShopName = _db.Query<string>("SELECT Name FROM [Store] where [Id]=(select StoreCode from Product where Id=@productCode)", new { productCode = lstProducts[0].ProductCode }).SingleOrDefault(),
                        MemberName = _db.Query<string>("SELECT Name FROM [Member] where [Id]=@memberCode", new { memberCode = order.MemberCode }).SingleOrDefault(),
                        OrderStatus = new OrderStatusBL().SelectOne(orderHistoryOfLastStatus.OrderStatusCode),
                        OverallPayment = order.OverallPayment,
                        OverallOrderCost = overallOrderCost, //todo: farada bayad tebghe in ba foroshandeha tasviye hesab konam
                        MemberUsedBalance = overallOrderCost - order.OverallPayment,
                        IsPony = (new HBPaymentToStoreBL().SelectOne(order.Id) != null),
                        CanPony = CheckCanPony(order.OrderType, orderHistoryOfLastStatus.OrderStatusCode)
                    });
                }
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                        Value = pageNumer.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }
    }
}
