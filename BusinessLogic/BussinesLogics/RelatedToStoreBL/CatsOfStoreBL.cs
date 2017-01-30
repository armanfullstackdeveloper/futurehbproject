using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToStore;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToStoreBL
{
    public class CatsOfStoreBL
    {
        public bool Save(CatsOfStore catsOfStore)
        {
            try
            {
                CatsOfStore result;
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    result = (CatsOfStore)session.Save(catsOfStore);

                    transaction.Commit();
                }
                return (result != null && result.CatCode > 0 && result.StoreCode > 0);
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(catsOfStore).ToString());
            }
        }

        public bool SaveWhitOutCommitTransaction(CatsOfStore catsOfStore, ISession pSession)
        {
            try
            {
                CatsOfStore result = (CatsOfStore)pSession.Save(catsOfStore);
                return (result != null && result.CatCode > 0 && result.StoreCode > 0);
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(catsOfStore).ToString());
            }
        }

        public void DeleteWhitOutCommitTransaction(CatsOfStore catsOfStore, ISession pSession)
        {
            try
            {
                pSession.Delete(catsOfStore);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("refre"))
                {
                    throw new MyExceptionHandler("بعلت استفاده شدن در بخش های دیگر امکان حذف نمی باشد", ex, JObject.FromObject(catsOfStore).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(catsOfStore).ToString());
            }
        }

        public List<long> GetCatsByStoreCode(long storeCode)
        {
            try
            {
                List<long> result;
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    result = session.Query<CatsOfStore>().Where(c => c.StoreCode == storeCode).Select(c => c.CatCode).ToList();
                    transaction.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }

        public List<CatsOfStore> GetByStoreCode(long storeCode)
        {
            try
            {
                List<CatsOfStore> result;
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    result = session.Query<CatsOfStore>().Where(c => c.StoreCode == storeCode).ToList();
                    transaction.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }
    }
}
