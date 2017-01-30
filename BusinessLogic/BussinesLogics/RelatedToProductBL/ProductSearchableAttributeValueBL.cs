using System;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToProduct;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class ProductSearchableAttributeValueBL : GenericRepository<ProductSearchableAttributeValue, long>
    {
        public bool SaveWithoutCommit(ProductSearchableAttributeValue product, ISession pSession)
        {
            try
            {
                var query = pSession.CreateSQLQuery("INSERT INTO [dbo].[ProductSearchableAttributeValue] ([ProductCode],[AttributeCode],[AttributeValueCode]) VALUES ( :productCode , :attributeCode , :attributeValueCode)");
                query.SetParameter("productCode", product.ProductCode);
                query.SetParameter("attributeCode", product.AttributeCode);
                query.SetParameter("attributeValueCode", product.AttributeValueCode);


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
                String hql = "delete from [dbo].[ProductSearchableAttributeValue]  where [ProductCode]= :productCode";
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
