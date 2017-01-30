using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToProduct;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class ProductAdditionalAttrbiuteBL : GenericRepository<ProductAdditionalAttrbiute, long>
    {
        private IDbConnection _db;

        public QueryResult<ProductAdditionalAttrbiute> MultipleSave(long productCode,
            List<ProductAdditionalAttrbiuteDataModel> lstProductAdditionalAttrbiutes)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ProductCode", productCode);
                lstProductAdditionalAttrbiutes = lstProductAdditionalAttrbiutes ?? new List<ProductAdditionalAttrbiuteDataModel>();
                parameters.Add("@ProductAdditionalAttrbiutes",
                    lstProductAdditionalAttrbiutes.AsTableValuedParameter("dbo.ProductAdditionalAttrbiuteDataType", new List<string>() { "Title", "Value" }));
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("ProductAdditionalAttrbiute_MultipleSave", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");

                EnsureCloseConnection(_db);
                return new QueryResult<ProductAdditionalAttrbiute>(null, null, procResult);
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => productCode),
                        Value = productCode.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => lstProductAdditionalAttrbiutes),
                        Value = JArray.FromObject(lstProductAdditionalAttrbiutes).ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public List<ProductAdditionalAttrbiuteDataModel> GetAllForProductEdit(long productCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                List<ProductAdditionalAttrbiuteDataModel> list = _db.Query<ProductAdditionalAttrbiuteDataModel>(@"
            SELECT [Title],[Value] FROM [dbo].[ProductAdditionalAttrbiute] where [ProductCode]=@productCode", new { productCode }).ToList();
                EnsureCloseConnection(_db);
                return list;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }

        public bool DeleteAllByProductCode_WhitOutCommitTransaction(long productCode, ISession pSession)
        {
            try
            {
                String hql = "delete from [dbo].[ProductAdditionalAttrbiute] where [ProductCode]= :productCode";
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
