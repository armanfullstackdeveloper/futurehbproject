using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Entities.RelatedToProduct;
using DataModel.Models.ViewModel;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class CategoryBL : GenericRepository<Category, long>
    {
        private IDbConnection _db;

        public List<Category> GetFirstLevel()
        {
            _db = EnsureOpenConnection();
            List<Category> lst = _db.Query<Category>("SELECT * FROM dbo.Category_GetFirstLevel()").ToList();
            EnsureCloseConnection(_db);
            return lst;
        }

        public List<Category> GetSecondLevel(long? firstLevelCategoryCode = null)
        {
            try
            {
                _db = EnsureOpenConnection();
                List<Category> lst = _db.Query<Category>("SELECT * FROM dbo.Category_GetSecondLevel(@firstLevelCategoryCode)", new { firstLevelCategoryCode }).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, firstLevelCategoryCode.ToString());
            }
        }

        public List<Category> GetThirdLevel(long? thirdLeveleCategoryCode = null)
        {
            try
            {
                _db = EnsureOpenConnection();
                List<Category> lst = _db.Query<Category>("SELECT * FROM dbo.Category_GetThirdLevel(@thirdLeveleCategoryCode)", new { thirdLeveleCategoryCode }).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, thirdLeveleCategoryCode.ToString());
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool withImage = false)
        {
            _db = EnsureOpenConnection();
            IEnumerable<Category> lstFirstLevel;
            using (var txScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                lstFirstLevel = await _db.QueryAsync<Category>($"SELECT {(withImage ? "*" : "[Id],[Name],[BaseCategoryCode]")} FROM dbo.Category_GetFirstLevel()");
                IEnumerable<Category> lstSecondLevel = await _db.QueryAsync<Category>(
                    $"SELECT {(withImage ? "*" : "[Id],[Name],[BaseCategoryCode]")} FROM dbo.Category_GetSecondLevel(null)");
                IEnumerable<Category> lstThirdLevel = await _db.QueryAsync<Category>(
                   $"SELECT {(withImage ? "*" : "[Id],[Name],[BaseCategoryCode]")} FROM dbo.Category_GetThirdLevel(null)");
                foreach (Category category1 in lstFirstLevel)
                {
                    foreach (Category category2 in lstSecondLevel)
                    {
                        if (category1.Id == category2.BaseCategoryCode)
                        {
                            foreach (Category category3 in lstThirdLevel)
                            {
                                if (category2.Id == category3.BaseCategoryCode)
                                {
                                    if (category2.SubCategories == null) category2.SubCategories = new List<Category>();
                                    category2.SubCategories.Add(category3);
                                }
                            }
                            if (category1.SubCategories == null) category1.SubCategories = new List<Category>();
                            category1.SubCategories.Add(category2);
                        }
                    }
                }
                txScope.Complete();
            }
            EnsureCloseConnection(_db);
            return lstFirstLevel;
        }

        public async Task<IEnumerable<Category>> GetAllForMenueAsync()
        {
            _db = EnsureOpenConnection();
            IEnumerable<Category> lstFirstLevel;
            using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                lstFirstLevel = await _db.QueryAsync<Category>("SELECT * FROM dbo.Category_GetFirstLevel()");
                IEnumerable<Category> lstSecondLevel = await _db.QueryAsync<Category>("SELECT * FROM dbo.Category_GetSecondLevel(null)");
                IEnumerable<Category> lstThirdLevel = await _db.QueryAsync<Category>("SELECT * FROM dbo.Category_GetThirdLevelForMenue(null)");
                foreach (Category category1 in lstFirstLevel)
                {
                    foreach (Category category2 in lstSecondLevel)
                    {
                        if (category1.Id == category2.BaseCategoryCode)
                        {
                            foreach (Category category3 in lstThirdLevel)
                            {
                                if (category2.Id == category3.BaseCategoryCode)
                                {
                                    if (category2.SubCategories == null)
                                        category2.SubCategories = new List<Category>();
                                    category2.SubCategories.Add(category3);
                                }
                            }
                            if (category1.SubCategories == null)
                                category1.SubCategories = new List<Category>();
                            if (category2.SubCategories!= null && category2.SubCategories.Count > 0)
                                category1.SubCategories.Add(category2);
                        }
                    }
                }
                lstFirstLevel.ToList().RemoveAll(cat => cat.SubCategories==null || cat.SubCategories.All(sub => sub.SubCategories==null || sub.SubCategories.Count == 0));
                tx.Complete();
            }
            EnsureCloseConnection(_db);
            return lstFirstLevel;
        }

        public async Task<ProductRegisterRequireItemsViewModel> GetRequiredItemsForNewProductAsync(long catCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                ProductRegisterRequireItemsViewModel result = new ProductRegisterRequireItemsViewModel();
                List<AttributeValue> lstAttributeValues;
                List<AttributeValue> lstMultiSelectAttributeValues;
                var parameters = new DynamicParameters();
                parameters.Add("@catCode", catCode);

                using (var multipleResults = _db.QueryMultiple("Category_GetRequiredItemsForNewProduct", parameters, commandType: CommandType.StoredProcedure))
                {
                    result.ProductAttributeWithoutItems = (await multipleResults.ReadAsync<ProductAttributeWithoutItemsViewModel>()).ToList();
                    result.ProductSelectAttributeWithItems = (await multipleResults.ReadAsync<ProductAttributeWithItemsViewModel>()).ToList();
                    lstAttributeValues = (await multipleResults.ReadAsync<AttributeValue>()).ToList();
                    result.ProductMultiSelectAttributeWithItems = (await multipleResults.ReadAsync<ProductAttributeWithItemsViewModel>()).ToList();
                    lstMultiSelectAttributeValues = (await multipleResults.ReadAsync<AttributeValue>()).ToList();
                    //
                    result.Brands = (await multipleResults.ReadAsync<DropDownItemsModel>()).ToList();
                    result.Colors = (await multipleResults.ReadAsync<ColorViewModel>()).ToList();
                }

                foreach (AttributeValue attributeValue in lstAttributeValues)
                {
                    foreach (ProductAttributeWithItemsViewModel model in result.ProductSelectAttributeWithItems)
                    {
                        if (attributeValue.AttributeCode == model.AttributeCode)
                        {
                            if (model.AttributeItems == null)
                                model.AttributeItems = new List<DropDownItemsModel>();
                            model.AttributeItems.Add(new DropDownItemsModel()
                            {
                                Value = attributeValue.Id,
                                Text = attributeValue.Value
                            });
                            break;
                        }
                    }
                }

                foreach (AttributeValue attributeValue in lstMultiSelectAttributeValues)
                {
                    foreach (ProductAttributeWithItemsViewModel model in result.ProductMultiSelectAttributeWithItems)
                    {
                        if (attributeValue.AttributeCode == model.AttributeCode)
                        {
                            if (model.AttributeItems == null)
                                model.AttributeItems = new List<DropDownItemsModel>();
                            model.AttributeItems.Add(new DropDownItemsModel()
                            {
                                Value = attributeValue.Id,
                                Text = attributeValue.Value
                            });
                            break;
                        }
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, catCode.ToString());
            }
        }

        public async Task<ProductSearchRequireItemsViewModel> GetRequiredItemsForSearchAsync(long catCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                ProductSearchRequireItemsViewModel result = new ProductSearchRequireItemsViewModel();
                List<AttributeValue> lstAttributeValues;
                List<City> lstCities;
                var parameters = new DynamicParameters();
                parameters.Add("@catCode", catCode);

                using (var multipleResults = _db.QueryMultiple("Category_GetRequiredItemsForSearch", parameters, commandType: CommandType.StoredProcedure))
                {
                    result.ProductAttributeWithItems = (await multipleResults.ReadAsync<ProductAttributeWithItemsViewModel>()).ToList();
                    lstAttributeValues = (await multipleResults.ReadAsync<AttributeValue>()).ToList();
                    //
                    result.Brands = (await multipleResults.ReadAsync<DropDownItemsModel>()).ToList();
                    result.Colors = (await multipleResults.ReadAsync<ColorViewModel>()).ToList();
                    //
                    lstCities = (await multipleResults.ReadAsync<City>()).ToList();
                    result.States = (await multipleResults.ReadAsync<StateViewModel>()).ToList();
                    result.MaxPrice = (await multipleResults.ReadAsync<int>()).SingleOrDefault();
                }

                foreach (AttributeValue attributeValue in lstAttributeValues)
                {
                    foreach (ProductAttributeWithItemsViewModel model in result.ProductAttributeWithItems)
                    {
                        if (attributeValue.AttributeCode == model.AttributeCode)
                        {
                            if (model.AttributeItems == null)
                                model.AttributeItems = new List<DropDownItemsModel>();
                            model.AttributeItems.Add(new DropDownItemsModel()
                            {
                                Value = attributeValue.Id,
                                Text = attributeValue.Value
                            });
                            break;
                        }
                    }
                }

                foreach (City city in lstCities)
                {
                    foreach (StateViewModel model in result.States)
                    {
                        if (city.StateCode == model.Value)
                        {
                            if (model.Cities == null)
                                model.Cities = new List<DropDownItemsModel>();
                            model.Cities.Add(new DropDownItemsModel()
                            {
                                Value = city.Id,
                                Text = city.Name
                            });
                            break;
                        }
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, catCode.ToString());
            }
        }

        public List<Category> GetAllForStore(long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                List<Category> lstFirstLevel;
                using (var txScope = new TransactionScope())
                {
                    lstFirstLevel = _db.Query<Category>(@"SELECT * FROM dbo.Category_GetFirstLevel() 
                            where Id in (select [CatCode] from [dbo].[CatsOfStore] where [StoreCode]= @StoreCode)", new { StoreCode = storeCode }).ToList();
                    List<Category> lstSecondLevel = _db.Query<Category>(@"SELECT * FROM dbo.Category_GetSecondLevel(null)
                            where [BaseCategoryCode] in @catList", new { catList = lstFirstLevel.Select(cat => cat.Id).ToArray() }).ToList();
                    List<Category> lstThirdLevel = _db.Query<Category>(@"SELECT * FROM dbo.Category_GetThirdLevel(null)
                            where [BaseCategoryCode] in @catList", new { catList = lstSecondLevel.Select(cat => cat.Id).ToArray() }).ToList();
                    foreach (Category category1 in lstFirstLevel)
                    {
                        foreach (Category category2 in lstSecondLevel)
                        {
                            if (category1.Id == category2.BaseCategoryCode)
                            {
                                foreach (Category category3 in lstThirdLevel)
                                {
                                    if (category2.Id == category3.BaseCategoryCode)
                                    {
                                        if (category2.SubCategories == null) category2.SubCategories = new List<Category>();
                                        category2.SubCategories.Add(category3);
                                    }
                                }
                                if (category1.SubCategories == null) category1.SubCategories = new List<Category>();
                                category1.SubCategories.Add(category2);
                            }
                        }
                    }
                    txScope.Complete();
                }
                EnsureCloseConnection(_db);
                return lstFirstLevel;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }
    }
}
