using System;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToProduct;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class ProductNonSearchableAttributeValueBL : GenericRepository<ProductNonSearchableAttributeValue, long>
    {
        public bool SaveWithoutCommit(ProductNonSearchableAttributeValue product, ISession pSession)
        {
            try
            {
                var query = pSession.CreateSQLQuery(@"INSERT INTO [dbo].[ProductNonSearchableAttributeValue] 
           ([ProductCode]
           ,[AttributeCode]
           ,[Value])
             VALUES
           (:productCode 
           ,:attributeCode 
           ,:value)");
                query.SetParameter("productCode", product.ProductCode);
                query.SetParameter("attributeCode", product.AttributeCode);
                query.SetParameter("value", product.Value);


                long result = query.ExecuteUpdate();
                return result > 0;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(product).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(product).ToString());
            }
        }

        public bool DeleteAllByProductCode_WhitOutCommitTransaction(long productCode, ISession pSession)
        {
            try
            {
                String query = "delete from [dbo].[ProductNonSearchableAttributeValue]  where [ProductCode]= :productCode";
                pSession.CreateSQLQuery(query).SetParameter("productCode", productCode).ExecuteUpdate();
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
