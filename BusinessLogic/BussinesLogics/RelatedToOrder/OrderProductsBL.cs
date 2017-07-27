using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToOrder;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace BusinessLogic.BussinesLogics.RelatedToOrder
{

    public class OrderProductsBL : GenericRepository<OrderProducts, long>
    {
        public bool SaveWithoutCommit(OrderProducts orderProduct, ISession pSession)
        {
            try
            {
                var query = pSession.CreateSQLQuery(@"INSERT INTO [dbo].[OrderProducts]
           ([OrderCode]
           ,[ProductCode]
           ,[Count]
           ,[CurrentPrice]
           ,[Color]
           ,[Size])
            VALUES ( :orderCode , :productCode , :count , :currentPrice , :color , :size )");
                query.SetParameter("orderCode", orderProduct.OrderCode);
                query.SetParameter("productCode", orderProduct.ProductCode);
                query.SetParameter("count", orderProduct.Count);
                query.SetParameter("currentPrice", orderProduct.CurrentPrice);
                query.SetParameter("color", orderProduct.Color);
                query.SetParameter("size", orderProduct.Size);

                long result = query.ExecuteUpdate();
                return result > 0;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(orderProduct).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(orderProduct).ToString());
            }
        }


        /// <summary>
        /// کد سفارشاتی که این محصول در آن ها وجود دارد
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public List<long> GetOrdersCode(long productCode)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@productCode", productCode);
                List<long> orders = db.Query<long>("SELECT OrderCode FROM [OrderProducts] where [ProductCode]=@productCode", parameters).ToList();
                EnsureCloseConnection(db);
                return orders;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }

        public bool CheckHaveOrderWithThisProduct(long productCode)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@productCode", productCode);
                long order = db.Query<long>("SELECT top 1 OrderCode FROM [OrderProducts] where [ProductCode]=@productCode", parameters).SingleOrDefault();
                EnsureCloseConnection(db);
                return (order != 0);
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }
    }
}
