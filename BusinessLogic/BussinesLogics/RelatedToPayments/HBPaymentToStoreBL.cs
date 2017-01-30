using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToPayments;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToPayments
{
    public class HBPaymentToStoreBL : GenericRepository<HBPaymentToStore, long>
    {
        public new long Insert(HBPaymentToStore hbPaymentToStore)
        {
            try
            {
                long result;
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var query = session.CreateSQLQuery(@"INSERT INTO [dbo].[HBPaymentToStore]
           ([OrderCode]
           ,[AdminCode]
           ,[Date]
           ,[TrackingCode]
           ,[Money])
                    VALUES ( :orderCode , :adminCode , :date ,:trackingCode ,:money)");
                    query.SetParameter("orderCode", hbPaymentToStore.OrderCode);
                    query.SetParameter("adminCode", hbPaymentToStore.AdminCode);
                    query.SetParameter("date", hbPaymentToStore.Date);
                    query.SetParameter("trackingCode", hbPaymentToStore.TrackingCode);
                    query.SetParameter("money", hbPaymentToStore.Money);


                    result = query.ExecuteUpdate();
                    transaction.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(hbPaymentToStore).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(hbPaymentToStore).ToString());
            }
        }

        public new bool Delete(long orderCode)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.CreateSQLQuery("delete from [dbo].[HBPaymentToStore] where OrderCode = :orderCode").SetParameter("orderCode", orderCode).ExecuteUpdate();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("conflicted with the REFERENCE constraint"))
                {
                    throw new MyExceptionHandler("بعلت استفاده شدن در بخش های دیگر امکان حذف نمی باشد", ex, orderCode.ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, orderCode.ToString());
            }
            return true;
        }

        public new HBPaymentToStore SelectOne(long orderCode)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateQuery("from HBPaymentToStore where OrderCode= :x ");
                    query.SetString("x", orderCode.ToString());
                    HBPaymentToStore s = (HBPaymentToStore)query.UniqueResult();
                    return s;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, orderCode.ToString());
            }
        }

        public List<HBPaymentToStore> GetAll_OrderByLast(int pageNumer)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var result = session.Query<HBPaymentToStore>().OrderByDescending(p => p.Date)
                        .Skip((pageNumer - 1) * StaticNembericInBL.CountOfItemsInAdminPages)
                        .Take(StaticNembericInBL.CountOfItemsInAdminPages)
                        .ToList();
                    transaction.Commit();
                    return result;
                }
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
