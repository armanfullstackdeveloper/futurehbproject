using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Entities.RelatedToOrder;
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace BusinessLogic.BussinesLogics.RelatedToOrder
{

    public class OrderHistoryBL : GenericRepository<OrderHistory, long>
    {
        public bool SaveWithoutCommit(OrderHistory orderHistory, ISession session)
        {
            try
            {
                var query = session.CreateSQLQuery(@"INSERT INTO [dbo].[OrderHistory]
                    ([OrderCode]
                    ,[OrderStatusCode]
                    ,[Date]
                    ,[Time],[UserCode])
                    VALUES ( :orderCode , :orderStatusCode , :date , :time, :userCode)");
                query.SetParameter("orderCode", orderHistory.OrderCode);
                query.SetParameter("orderStatusCode", orderHistory.OrderStatusCode);
                query.SetParameter("date", orderHistory.Date);
                query.SetParameter("time", orderHistory.Time);
                query.SetParameter("userCode", orderHistory.UserCode);

                long result = query.ExecuteUpdate();
                return result > 0;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(orderHistory).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(orderHistory).ToString());
            }
        }

        public bool Save(OrderHistory orderHistory)
        {
            try
            {
                long result;
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var query = session.CreateSQLQuery(@"INSERT INTO [dbo].[OrderHistory]
                    ([OrderCode]
                    ,[OrderStatusCode]
                    ,[Date]
                    ,[Time],[UserCode])
                    VALUES ( :orderCode , :orderStatusCode , :date , :time, :userCode)");
                    query.SetParameter("orderCode", orderHistory.OrderCode);
                    query.SetParameter("orderStatusCode", orderHistory.OrderStatusCode);
                    query.SetParameter("date", orderHistory.Date);
                    query.SetParameter("time", orderHistory.Time);
                    query.SetParameter("userCode", orderHistory.UserCode);

                    result = query.ExecuteUpdate();
                    transaction.Commit();
                }
                return result > 0;
            }          
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(orderHistory).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(orderHistory).ToString()); 
            }
        }


        /// <summary>
        /// تمامی وضعیت های اخیر یک سفارش را در میاره
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public List<OrderHistory> GetAllForOrder(long orderCode)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@orderCode", orderCode);
                List<OrderHistory> lstOrderHistories = db.Query<OrderHistory>("SELECT * FROM [OrderHistory] where [OrderCode]=@orderCode", parameters).ToList();
                EnsureCloseConnection(db);
                return lstOrderHistories;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, orderCode.ToString());
            }
        }


        /// <summary>
        /// این متد به صورت کلی و برای تمامی رول ها نوشته شده است
        /// توجه داشته باشید اینکه آیا رول خاصی قادر به تغییر وضعیت خاصی هست یا نه به این متد مربوط نمیشود
        /// و از قبل باید بررسی شود
        /// </summary>
        /// <param name="order"></param>
        /// <param name="newStatusCode"></param>
        /// <param name="lastStatusCode"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public UpdateOrderStatusResultViewModel UpdateOrderStatus(Order order, EOrderStatus newStatusCode, EOrderStatus lastStatusCode,string userCode)
        {
            try
            {
                ISession session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                bool result = new OrderHistoryBL().SaveWithoutCommit(new OrderHistory()
                {
                    OrderCode = order.Id,
                    OrderStatusCode = newStatusCode,
                    Date = PersianDateTime.Now.Date.ToInt(),
                    Time = PersianDateTime.Now.TimeOfDay.ToInteger(),
                    UserCode = userCode
                }, session);

                if (!result)
                {
                    session.Transaction.Rollback();
                    session = null;
                    return new UpdateOrderStatusResultViewModel()
                    {
                        IsSuccess = false
                    };
                }

                //اگه وضعیتو "دریافت شده" کنه یعنی پایان فرایند خرید و باید اقدامات مورد نیاز انجام بگیره                
                if (newStatusCode == EOrderStatus.Received)
                {
                    //اگه پرداخت آزاد بوده باشه که یعنی هم از موجودی فعالش در صورت استفاده کسر شده و هم سود خرید به حسابش ریخته شده


                    //اگه پرداخت امن بوده باشه یعنی مبلغ پرداختی هم تو حساب به صورت بلوکه شده هست
                    //و اگه از موجودی فعال هم استفاده شده باشه،هنوز به صورت بلوکه تو حسابشه
                    //و سود خرید هم به حسابش ریخته نشده
                    if (order.OrderType == EOrderType.SecurePayment)
                    {
                        Member member = new MemberBL().SelectOneWhitOutCommitTransaction(order.MemberCode, session);

                        member.Balance -= order.OverallPayment;

                        //مجموع هزینه ای که این سفارش داشته با احتساب تخفیف
                        int currentOrderOverallCost = new OrderBL().GetOverallOrderCost(order.Id, true);

                        //چقدر از موجودی قبلی کاربر استفاده شده
                        int memberUsedBalance = currentOrderOverallCost - order.OverallPayment;

                        //و اگه از موجودیش استفاده شده بود، بروز رسانی می کنیم
                        if (memberUsedBalance > 0)
                        {
                            member.Balance -= memberUsedBalance;
                        }

                        //سود خریدو تو موجودیش هم تاثیر میدیم
                        member.Balance += HelperFunctionInBL.GetProfit(currentOrderOverallCost, StaticNembericInBL.OrderProfitPercentForCustomer);

                        if (!new MemberBL().UpdateWhitOutCommitTransaction(member, session))
                        {
                            session.Transaction.Rollback();
                            session = null;
                            return new UpdateOrderStatusResultViewModel()
                            {
                                IsSuccess = false
                            };
                        }

                        session.Transaction.Commit();
                        return new UpdateOrderStatusResultViewModel()
                        {
                            IsSuccess = true,
                            MemberProfit = HelperFunctionInBL.GetProfit(currentOrderOverallCost, StaticNembericInBL.OrderProfitPercentForCustomer)
                        };
                    }
                }

                //اگه وضعیت دیگه ای درخواست شده باشه هم که بالا لاگش ثبت شدو قطعی میکنیمش
                session.Transaction.Commit();
                return new UpdateOrderStatusResultViewModel(){IsSuccess = true};
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => order),
                            Value = JObject.FromObject(order).ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => newStatusCode),
                            Value = newStatusCode.ToString()
                        },
                    };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }
    }
}
