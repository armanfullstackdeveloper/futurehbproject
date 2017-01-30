using System;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToProduct;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class ProductColorBL : GenericRepository<ProductColor, long>
    {
        public bool SaveWithoutCommit(ProductColor productColor, ISession pSession)
        {
            try
            {
                var query = pSession.CreateSQLQuery("INSERT INTO [dbo].[ProductColor] ([ProductCode],[ColorCode]) VALUES ( :productCode , :colorCode)");
                query.SetParameter("productCode", productColor.ProductCode);
                query.SetParameter("colorCode", productColor.ColorCode);

                query.ExecuteUpdate();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(productColor).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(productColor).ToString());
            }
            return true;
        }

        public bool DeleteAllByProductCode_WhitOutCommitTransaction(long productCode, ISession pSession)
        {
            try
            {
                String hql = "delete from [dbo].[ProductColor] where [ProductCode]= :productCode";
                pSession.CreateSQLQuery(hql).SetParameter("productCode", productCode).ExecuteUpdate();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("refre"))
                {
                    throw new MyExceptionHandler("بعلت استفاده شدن در بخش های دیگر امکان حذف نمی باشد", ex, productCode.ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
            return true;
        }
    }
}
