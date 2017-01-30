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

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class ProductImageBL : GenericRepository<ProductImage, long>
    {
        private IDbConnection _db;

        public int SaveImageWithCheckingIsItForThatStore(ProductImage productImage, long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ImgAddress", productImage.ImgAddress);
                parameters.Add("@ProductCode", productImage.ProductCode);
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("ProductImage_Save", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");
                EnsureCloseConnection(_db);
                return procResult;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                     new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => productImage),
                        Value = JObject.FromObject(productImage).ToString()
                    },
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                        Value = storeCode.ToString()
                    },
                     
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }


        public List<ProductImage> GetAllProductImgAddress(long productId)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@productId", productId);
                List<ProductImage> lst = _db.Query<ProductImage>("ProductImage_GetProductImgAddress", parameters, commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productId.ToString());
            }
        }


        public QueryResult<Product> DeleteWithCheckingIsItForThatStore(long productId, long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", productId);
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("ProductImage_Delete", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");
                EnsureCloseConnection(_db);
                return new QueryResult<Product>(null, null, procResult);
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {               
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => productId),
                        Value = productId.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                        Value = storeCode.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public int GetProductImageCount(long productCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                int? count = _db.Query<int>("SELECT count(ProductCode) FROM ProductImage where ProductCode=@productCode", new { productCode }).SingleOrDefault();
                EnsureCloseConnection(_db);
                return (int)count;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }

    }
}
