using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToProduct;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class ProductBL : GenericRepository<Product, long>
    {
        private IDbConnection _db;

        public CompleteProductForOne GetOneProduct(long productId)
        {
            try
            {
                _db = EnsureOpenConnection();
                CompleteProductForOne completeProduct = new CompleteProductForOne();
                List<NameValueDataModel> productAttributeForNonSearchable = null;
                List<NameValueDataModel> productAttributeForSearchable = null;
                List<NameValueDataModel> productAdditionalAttributes = null;

                var parameters = new DynamicParameters();
                parameters.Add("@productId", productId);
                using (var multipleResults = _db.QueryMultiple("Product_GetProductById", parameters, commandType: CommandType.StoredProcedure))
                {
                    completeProduct.Product = multipleResults.Read<ProductDetailsViewModel>().SingleOrDefault();
                    if (completeProduct.Product != null)
                    {
                        completeProduct.Product.OtherImagesAddress = multipleResults.Read<ImageViewModel>().ToList();
                        completeProduct.StoreSummery = multipleResults.Read<StoreSummery>().SingleOrDefault();
                        completeProduct.Colors = multipleResults.Read<Color>().ToList();
                        productAttributeForNonSearchable = multipleResults.Read<NameValueDataModel>().ToList();
                        productAttributeForSearchable = multipleResults.Read<NameValueDataModel>().ToList();
                        productAdditionalAttributes = multipleResults.Read<NameValueDataModel>().ToList();
                    }
                }
                EnsureCloseConnection(_db);

                if (productAttributeForNonSearchable != null && productAttributeForNonSearchable.Count > 0)
                {
                    completeProduct.ProductAttrbiutesViewModels = new List<ProductAttrbiutesViewModel>();
                    foreach (NameValueDataModel model in productAttributeForNonSearchable)
                    {
                        if (!completeProduct.ProductAttrbiutesViewModels.Exists(atr => atr.AttributeName == model.Name))
                        {
                            completeProduct.ProductAttrbiutesViewModels.Add(new ProductAttrbiutesViewModel()
                            {
                                AttributeName = model.Name,
                                AttributeValues = new List<string>() { model.Value }
                            });
                        }
                        else
                        {
                            completeProduct.ProductAttrbiutesViewModels.Single(atr => atr.AttributeName == model.Name).AttributeValues.Add(model.Value);
                        }
                    }
                }

                if (productAttributeForSearchable != null && productAttributeForSearchable.Count > 0)
                {
                    if (completeProduct.ProductAttrbiutesViewModels == null)
                        completeProduct.ProductAttrbiutesViewModels = new List<ProductAttrbiutesViewModel>();
                    foreach (NameValueDataModel model in productAttributeForSearchable)
                    {
                        if (!completeProduct.ProductAttrbiutesViewModels.Exists(atr => atr.AttributeName == model.Name))
                        {
                            completeProduct.ProductAttrbiutesViewModels.Add(new ProductAttrbiutesViewModel()
                            {
                                AttributeName = model.Name,
                                AttributeValues = new List<string>() { model.Value }
                            });
                        }
                        else
                        {
                            completeProduct.ProductAttrbiutesViewModels.Single(atr => atr.AttributeName == model.Name).AttributeValues.Add(model.Value);
                        }
                    }
                }

                if (productAdditionalAttributes != null && productAdditionalAttributes.Count > 0)
                {
                    if (completeProduct.ProductAttrbiutesViewModels == null)
                        completeProduct.ProductAttrbiutesViewModels = new List<ProductAttrbiutesViewModel>();
                    foreach (NameValueDataModel model in productAdditionalAttributes)
                    {
                        if (!completeProduct.ProductAttrbiutesViewModels.Exists(atr => atr.AttributeName == model.Name))
                        {
                            completeProduct.ProductAttrbiutesViewModels.Add(new ProductAttrbiutesViewModel()
                            {
                                AttributeName = model.Name,
                                AttributeValues = new List<string>() { model.Value }
                            });
                        }
                        else
                        {
                            completeProduct.ProductAttrbiutesViewModels.Single(atr => atr.AttributeName == model.Name).AttributeValues.Add(model.Value);
                        }
                    }
                }

                return completeProduct;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productId.ToString());
            }
        }

        public async Task<SearchResultViewModel> Search(SearchParametersDataModel searchParameters, List<EProductStatus> status, long? productCode = null,bool? haveImage=null)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@name", searchParameters.Name);
                parameters.Add("@categoryCode", searchParameters.CategoryCode);
                parameters.Add("@storeCode", searchParameters.StoreCode);
                searchParameters.PageNumber = searchParameters.PageNumber ?? 1;
                parameters.Add("@PageNumber", searchParameters.PageNumber);
                searchParameters.RowsPage = searchParameters.RowsPage ?? 24;
                parameters.Add("@RowspPage", searchParameters.RowsPage);
                searchParameters.TileShow = searchParameters.TileShow ?? true;
                parameters.Add("@tileShowView", searchParameters.TileShow);
                parameters.Add("@minPrice", searchParameters.MinPrice);
                parameters.Add("@maxPrice", searchParameters.MaxPrice);
                parameters.Add("@justExsisted", searchParameters.JustExsisted);
                parameters.Add("@haveImage", haveImage);
                searchParameters.SortBy = searchParameters.SortBy ?? 1;
                parameters.Add("@sortBy", searchParameters.SortBy);
                searchParameters.Ascending = searchParameters.Ascending ?? false;
                parameters.Add("@ascending", searchParameters.Ascending);
                parameters.Add("@cityCode", searchParameters.City);
                parameters.Add("@stateCode", searchParameters.State);
                parameters.Add("@productCount", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                if (searchParameters.Brands == null)
                    searchParameters.Brands = new List<long>();
                parameters.Add("@BrandsCode", searchParameters.Brands.AsTableValuedParameter("dbo.IdTable"));
                if (searchParameters.Colors == null)
                    searchParameters.Colors = new List<long>();
                parameters.Add("@ColoresCode", searchParameters.Colors.AsTableValuedParameter("dbo.IdTable"));
                parameters.Add("@Latitude", searchParameters.Latitude);
                parameters.Add("@Longitude", searchParameters.Longitude);
                
                List<long> lstStatus = status == null ? new List<long>() : status.Select(item => (long)item).ToList();
                parameters.Add("@status", lstStatus.AsTableValuedParameter("dbo.IdTable"));
                
                parameters.Add("@productCode", productCode);

                List<AttributeFilterDataModel> lstFilters;
                if (searchParameters.Attributes == null)
                    lstFilters = new List<AttributeFilterDataModel>();
                else
                {
                    lstFilters = (from model in searchParameters.Attributes
                                  from attributeValueCode in model.Values
                                  select new AttributeFilterDataModel()
                                  {
                                      AttributeCode = model.Code,
                                      AttributeValueCode = attributeValueCode
                                  }).ToList();
                }
                parameters.Add("@attributeFilters",
                    lstFilters.AsTableValuedParameter("[dbo].[SearchParameter]", new List<string>() { "AttributeCode", "AttributeValueCode" }));

                SearchResultViewModel searchResult = new SearchResultViewModel();
                List<ProductAttrbiutesDataModel> lstProductAttrbiutesDataModels = null;
                using (var multipleResults = await _db.QueryMultipleAsync("Search", parameters,
                    commandType: CommandType.StoredProcedure))
                {
                    searchResult.ProductsSummery = multipleResults.Read<ProductSummary>().ToList();
                    if (searchParameters.TileShow != null && (bool)!searchParameters.TileShow)
                        lstProductAttrbiutesDataModels = multipleResults.Read<ProductAttrbiutesDataModel>().ToList();
                    searchResult.ProductsCount = parameters.Get<int>("@productCount");
                }
                EnsureCloseConnection(_db);

                if (lstProductAttrbiutesDataModels == null || lstProductAttrbiutesDataModels.Count <= 0)
                    return searchResult;
                foreach (ProductAttrbiutesDataModel model in lstProductAttrbiutesDataModels)
                {
                    foreach (ProductSummary productSummary in searchResult.ProductsSummery)
                    {
                        if (model.ProductCode != productSummary.Id) continue;
                        if (productSummary.ProductAttrbiutesViewModels == null)
                            productSummary.ProductAttrbiutesViewModels = new List<ProductAttrbiutesViewModel>();

                        if (productSummary.ProductAttrbiutesViewModels.Exists(p => p.AttributeName == model.AttributeName))
                        {
                            productSummary.ProductAttrbiutesViewModels.Single(p => p.AttributeName == model.AttributeName)
                                .AttributeValues.Add(model.AttributeValue);
                        }
                        else
                        {
                            productSummary.ProductAttrbiutesViewModels.Add(new ProductAttrbiutesViewModel()
                            {
                                AttributeName = model.AttributeName,
                                AttributeValues = new List<string>() { model.AttributeValue }
                            });
                        }
                    }
                }
                return searchResult;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(searchParameters).ToString());
            }
        }

        public QueryResult<Product> UpdateMainImag(ProductImage productImage, long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ImgAddress", productImage.ImgAddress);
                parameters.Add("@ProductCode", productImage.ProductCode);
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("Product_UpdateMainImg", parameters, commandType: CommandType.StoredProcedure);
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

        public string GetImgAddressById(long productId)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@productId", productId);
                string address = _db.Query<string>("Product_GetImgAddressById", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return address;
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
                parameters.Add("@productId", productId);
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("Product_Delete", parameters, commandType: CommandType.StoredProcedure);
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

        public OveralSearchResultDataModel OveralSearchSummary(string name, int? pageNumberForProduct,
            int? rowspPageForProduct, int? pageNumberForStore, int? rowspPageForStore)
        {
            try
            {
                _db = EnsureOpenConnection();
                pageNumberForProduct = pageNumberForProduct ?? 1;
                rowspPageForProduct = rowspPageForProduct ?? 14;
                pageNumberForStore = pageNumberForStore ?? 1;
                rowspPageForStore = rowspPageForStore ?? 14;
                OveralSearchResultDataModel overalSearchResult = new OveralSearchResultDataModel();
                var parameters = new DynamicParameters();
                parameters.Add("@name", name);
                parameters.Add("@PageNumberForProduct", pageNumberForProduct);
                parameters.Add("@RowspPageForProduct", rowspPageForProduct);
                parameters.Add("@PageNumberForStore", pageNumberForStore);
                parameters.Add("@RowspPageForStore", rowspPageForStore);

                using (var multipleResults = _db.QueryMultiple("OverallSearch_Summary", parameters, commandType: CommandType.StoredProcedure))
                {
                    overalSearchResult.ProductsResult = multipleResults.Read<ProductsResultDataModel>().ToList();
                    overalSearchResult.StoresResult = multipleResults.Read<StoresResultDataModel>().ToList();
                }
                return overalSearchResult;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => name),
                        Value = name.ToString()
                    },
                    new ActionInputViewModel(){
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumberForProduct),
                        Value = pageNumberForProduct.ToString()
                    },
                    new ActionInputViewModel(){
                        Name = HelperFunctionInBL.GetVariableName(() => rowspPageForStore),
                        Value = rowspPageForStore.ToString()
                    },
                    new ActionInputViewModel() {
                        Name = HelperFunctionInBL.GetVariableName(() => rowspPageForProduct),
                        Value = rowspPageForProduct.ToString()
                    },
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumberForStore),
                        Value = pageNumberForStore.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public List<ProductSummary> OveralSearchForProducts(string name, int? pageNumber, int? rowspPage)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                if (string.IsNullOrWhiteSpace(name))
                    return new List<ProductSummary>();
                parameters.Add("@name", name);
                pageNumber = pageNumber ?? 1;
                parameters.Add("@PageNumber", pageNumber);
                rowspPage = rowspPage ?? 10;
                parameters.Add("@RowspPage", rowspPage);

                List<ProductSummary> lst = _db.Query<ProductSummary>("OverallSearch_Products", parameters, commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => name),
                        Value = name
                    },
                                         new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                        Value = pageNumber.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                        Value = rowspPage.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public List<ProductAttributeWithoutItemsDataModel> GetProductTextAttributeForEdit(long productCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                List<ProductAttributeWithoutItemsDataModel> lst = _db.Query<ProductAttributeWithoutItemsDataModel>(@"
            select AttributeCode,Value from [dbo].[ProductNonSearchableAttributeValue] 
                where [ProductCode]=@productCode", new { productCode }).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }

        public List<ProductAttributeWithItemsForSaveAndUpdate> GetProductDropDownAttributeForEdit(long productCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@productCode", productCode);

                List<ProductEditDataModel> lst = _db.Query<ProductEditDataModel>("ProductSearchableAttributeValue_GetModelForEdit", parameters, commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                List<ProductAttributeWithItemsForSaveAndUpdate> result = new List<ProductAttributeWithItemsForSaveAndUpdate>();
                foreach (ProductEditDataModel model in lst)
                {
                    if (result.Exists(atr => atr.Code == model.AttributeCode))
                    {
                        result.Single(atr => atr.Code == model.AttributeCode).Values.Add(model.AttributeValueCode);
                    }
                    else
                    {
                        result.Add(new ProductAttributeWithItemsForSaveAndUpdate()
                        {
                            Code = model.AttributeCode,
                            Values = new List<long>() { model.AttributeValueCode }
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }

        public string GetProductMainImageAddress(long productCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                string address = _db.Query<string>("SELECT [ImgAddress] FROM [dbo].[Product] where Id=@productCode", new { productCode }).SingleOrDefault();
                EnsureCloseConnection(_db);
                return address;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, productCode.ToString());
            }
        }

        public List<Product> GetProductsByCodes(List<long> codes)
        {
            try
            {
                List<Product> result;
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    result = session.Query<Product>().Where(p => codes.Contains(p.Id)).ToList();
                    transaction.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(codes).ToString());
            }
        }

        public List<Product> GetAllProductByStoreCode(long storeCode)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    List<Product> result = session.Query<Product>().Where(p => p.StoreCode == storeCode).ToList();
                    transaction.Commit();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }
    }
}
