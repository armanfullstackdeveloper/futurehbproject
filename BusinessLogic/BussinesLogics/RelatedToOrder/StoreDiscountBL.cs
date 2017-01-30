using System;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToOrder;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToOrder {

    public class StoreDiscountBL : GenericRepository<StoreDiscount, long>
    {
        public StoreDiscount SelectByCode(string discountedCode)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    return session.Query<StoreDiscount>().SingleOrDefault(s => s.Code == discountedCode);
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, discountedCode);
            }
        }

        public bool InactiveIfIsDisposable(long? storeDiscountCode)
        {
            try
            {
                if (storeDiscountCode == null || storeDiscountCode==0)
                    return false;
                StoreDiscount storeDiscount = new StoreDiscountBL().SelectOne((long)storeDiscountCode);
                if (storeDiscount == null || storeDiscount.Id == 0)
                    return false;
                if (storeDiscount.IsDisposable)
                {
                    storeDiscount.IsActive = false;
                    new StoreDiscountBL().Update(storeDiscount);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeDiscountCode.ToString());
            }
        }

        public StoreDiscount SelectOneByDiscountCode(string discountCode)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                StoreDiscount storeDiscount = db.Query<StoreDiscount>("SELECT * FROM [StoreDiscount] where Code=@discountCode", new { discountCode }).SingleOrDefault();
                EnsureCloseConnection(db);
                return storeDiscount;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, discountCode);
            }
        }
    }
}
