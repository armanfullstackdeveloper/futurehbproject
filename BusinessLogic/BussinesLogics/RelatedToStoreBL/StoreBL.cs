using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Entities.RelatedToOrder;
using DataModel.Entities.RelatedToProduct;
using DataModel.Entities.RelatedToStore;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToStoreBL
{
    public class StoreBL : GenericRepository<Store, long>
    {
        private IDbConnection _db;

        public StoreSessionDataModel GetSummaryForSession(string userId)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                StoreSessionDataModel storeSessionDataModel =
                    _db.Query<StoreSessionDataModel>("Store_SelectByUserCodeForSession", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return storeSessionDataModel;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userId);
            }
        }

        public QueryResult<Store> FullRegister(StoreRegisterDataModel storeRegister, string userCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@StoreCode", dbType: DbType.Int64, direction: ParameterDirection.InputOutput);
                parameters.Add("@StoreName", storeRegister.StoreName);
                parameters.Add("@StoreComments", storeRegister.StoreComments);
                parameters.Add("@CommercialCode", storeRegister.CommercialCode);
                parameters.Add("@ReagentPhoneNumber", storeRegister.ReagentPhoneNumber);
                parameters.Add("@UserCode", userCode);
                parameters.Add("@CityCode", storeRegister.CityCode == 0 ? null : storeRegister.CityCode);
                parameters.Add("@Place", storeRegister.Place);
                parameters.Add("@Latitude", storeRegister.Latitude);
                parameters.Add("@Longitude", storeRegister.Longitude);
                parameters.Add("@SallerName", storeRegister.SallerName);
                parameters.Add("@NationalCode", storeRegister.NationalCode);
                parameters.Add("@SallerComments", storeRegister.SallerComments);
                parameters.Add("@IsMale", storeRegister.IsMale);
                parameters.Add("@Type", storeRegister.StoreTypeCode);
                parameters.Add("@Website", storeRegister.Website);
                parameters.Add("@CatsCode", storeRegister.ListCategoryCode.AsTableValuedParameter("dbo.IdTable"));
                parameters.Add("@PhoneNumber", string.IsNullOrEmpty(storeRegister.PhoneNumber)?null:storeRegister.PhoneNumber);
                parameters.Add("@storeStatus", (byte)EStoreStatus.Active);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("Store_FullRegister", parameters, commandType: CommandType.StoredProcedure);
                int procResult = parameters.Get<int>("@ProcResult");
                long storeCode = parameters.Get<long>("@StoreCode");

                EnsureCloseConnection(_db);
                return new QueryResult<Store>(new Store() { Id = storeCode }, null, procResult);
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                     new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                        Value = JObject.FromObject(storeRegister).ToString()
                    },
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => userCode),
                        Value = userCode.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public List<StoreSummery> OveralSearchForStores(string name, int? pageNumber, int? rowspPage)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                if (string.IsNullOrWhiteSpace(name))
                    return new List<StoreSummery>();
                parameters.Add("@name", name);
                pageNumber = pageNumber ?? 1;
                parameters.Add("@PageNumber", pageNumber);
                rowspPage = rowspPage ?? 10;
                parameters.Add("@RowspPage", rowspPage);

                List<StoreSummery> lst = _db.Query<StoreSummery>("OverallSearch_Stores", parameters, commandType: CommandType.StoredProcedure).ToList();
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
                    },  new ActionInputViewModel()
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

        public async Task<IEnumerable<StoreSummery>> GetNewest(long? cityCode = null, int? pageNumber = null, int? rowspPage = null)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@cityCode", cityCode);
                pageNumber = (pageNumber == 0 ? 1 : pageNumber) ?? 1;
                parameters.Add("@PageNumber", pageNumber);
                rowspPage = rowspPage ?? 12;
                parameters.Add("@RowspPage", rowspPage);

                IEnumerable<StoreSummery> lst = await _db.QueryAsync<StoreSummery>("Store_GetNewest", parameters, commandType: CommandType.StoredProcedure);
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => cityCode),
                        Value = cityCode.ToString()
                    },  new ActionInputViewModel()
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

        public string GetStorePhotoAddres(long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@storeCode", storeCode);

                string st = _db.Query<string>("Store_GetMainPhoto", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return st;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }

        public bool EditStorePhotoAddress(long storeCode, string rootPath)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@newAddress", rootPath);
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("Store_EditPhotoAddress", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");
                EnsureCloseConnection(_db);
                return (procResult == 111);
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                        Value = storeCode.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => rootPath),
                        Value = rootPath.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public string GetLogoAddres(long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@storeCode", storeCode);

                string st = _db.Query<string>("Store_GetLogoAddress", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return st;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }

        public string GetLogoAddresByUserCode(string userCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@userCode", userCode);

                string st = _db.Query<string>("select LogoAddress from Store where UserCode=@userCode", parameters).SingleOrDefault();
                EnsureCloseConnection(_db);
                return st;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userCode);
            }
        }

        public bool EditStoreLogoAddress(long storeCode, string rootPath)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@newAddress", rootPath);
                parameters.Add("@storeCode", storeCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                _db.Execute("Store_EditLogoAddress", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");
                EnsureCloseConnection(_db);
                return (procResult == 111);
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                        Value = storeCode.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => rootPath),
                        Value = rootPath.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public StoreDetailsViewModel GetOneStoreDetails(long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                StoreDetailsViewModel storeDetails;
                var parameters = new DynamicParameters();
                parameters.Add("@storeCode", storeCode);
                using (var multipleResults = _db.QueryMultiple("Store_GetOneStoreDetails", parameters, commandType: CommandType.StoredProcedure))
                {
                    storeDetails = multipleResults.Read<StoreDetailsViewModel>().SingleOrDefault();
                    if (storeDetails != null)
                    {
                        storeDetails.Tells = multipleResults.Read<string>().ToList();
                        storeDetails.Images = multipleResults.Read<string>().ToList();
                        storeDetails.Categories = multipleResults.Read<string>().ToList();
                    }
                }
                EnsureCloseConnection(_db);
                return storeDetails;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }

        public StoreEditDataModel GetStoreForEdit(long storeCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                Store store = new StoreBL().SelectOne(storeCode);
                List<string> lstTell = new StoreTellBL().GetTellsById(storeCode);
                List<long> catsList = new CatsOfStoreBL().GetCatsByStoreCode(storeCode);

                string email = new UserBL().GetById(store.UserCode).Email;
                StoreEditDataModel storeEditDataModel = new StoreEditDataModel()
                {
                    CityCode = store.CityCode,
                    CommercialCode = store.CommercialCode,
                    Email = string.IsNullOrEmpty(email) ? "" : email,
                    ReagentPhoneNumber = store.ReagentPhoneNumber,
                    Latitude = store.Latitude,
                    Longitude = store.Longitude,
                    StoreCode = store.Id,
                    PhoneNumbers = lstTell,
                    Place = store.Place,
                    ListCategoryCode = catsList,
                    StoreComments = store.Comments,
                    Website = store.Website,
                    StoreTypeCode = store.StoreTypeCode,
                    StoreName = store.Name,
                    HomePage = store.HomePage
                };

                return storeEditDataModel;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }

        public bool EditStore(StoreEditDataModel storeEditDataModel)
        {
            try
            {
                ISession session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                Store storedStore = new StoreBL().SelectOneWhitOutCommitTransaction(storeEditDataModel.StoreCode,
                    session);
                storedStore.CityCode = storeEditDataModel.CityCode;
                storedStore.CommercialCode = storeEditDataModel.CommercialCode;
                storedStore.ReagentPhoneNumber = storeEditDataModel.ReagentPhoneNumber;
                storedStore.Latitude = storeEditDataModel.Latitude;
                storedStore.Longitude = storeEditDataModel.Longitude;
                storedStore.Place = storeEditDataModel.Place;
                storedStore.Comments = storeEditDataModel.StoreComments;
                storedStore.Website = storeEditDataModel.Website;
                storedStore.StoreTypeCode = (byte)(storeEditDataModel.StoreTypeCode != 0 ? storeEditDataModel.StoreTypeCode : storedStore.StoreTypeCode);
                storedStore.Name = storeEditDataModel.StoreName;
                storedStore.HomePage = storeEditDataModel.HomePage;
                new StoreBL().UpdateWhitOutCommitTransaction(storedStore, session);

                //for email
                var user = new UserBL().GetById(storedStore.UserCode);
                user.Email = storeEditDataModel.Email;
                new UserBL().Update(user);

                //age category hash khali omad hamon ghabli ro hefz kon
                if (storeEditDataModel.ListCategoryCode != null && storeEditDataModel.ListCategoryCode.Any())
                {
                    List<CatsOfStore> catsList = new CatsOfStoreBL().GetByStoreCode(storeEditDataModel.StoreCode);
                    foreach (CatsOfStore item in catsList)
                    {
                        if (storeEditDataModel.ListCategoryCode != null && !storeEditDataModel.ListCategoryCode.Contains(item.CatCode))
                            new CatsOfStoreBL().DeleteWhitOutCommitTransaction(item, session);
                    }
                    if (storeEditDataModel.ListCategoryCode != null)
                    {
                        foreach (long item in storeEditDataModel.ListCategoryCode)
                        {
                            if (!catsList.Contains(new CatsOfStore() { CatCode = item, StoreCode = storeEditDataModel.StoreCode }))
                                new CatsOfStoreBL().SaveWhitOutCommitTransaction(new CatsOfStore() { CatCode = item, StoreCode = storeEditDataModel.StoreCode }, session);
                        }
                    }
                }

                List<string> lstTell = new StoreTellBL().GetTellsById(storeEditDataModel.StoreCode);
                foreach (string item in lstTell)
                {
                    if (storeEditDataModel.PhoneNumbers != null && !storeEditDataModel.PhoneNumbers.Contains(item))
                        new StoreTellBL().Delete(new StoreTell() { PhoneNumber = item, StoreCode = storeEditDataModel.StoreCode });
                }
                if (storeEditDataModel.PhoneNumbers != null)
                {
                    foreach (string item in storeEditDataModel.PhoneNumbers)
                    {
                        if (!lstTell.Contains(item))
                            new StoreTellBL().Save(new StoreTell() { PhoneNumber = item, StoreCode = storeEditDataModel.StoreCode });
                    }
                }
                session.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(storeEditDataModel).ToString());
            }
        }

        public long? GetStoreCodeByHomePage(string shopname)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@shopname", shopname);
                long storeCode = _db.Query<long>("Store_SelectIdByHomePage", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return storeCode;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, shopname.ToString());
            }
        }


        /// <summary>
        /// برای وقتی که بخوایم چک کنم آیا سفارش فلان، مربوط به فروشگاه فلان هست یا نه
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool CheckHaveOrder(long storeCode, long orderCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@orderCode", orderCode);
                OrderProducts orderProducts = _db.Query<OrderProducts>("select top 1 * from [OrderProducts] where OrderCode=@orderCode", parameters).Single();
                EnsureCloseConnection(_db);

                Product product = new ProductBL().SelectOne(orderProducts.ProductCode);
                return product.StoreCode == storeCode;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => orderCode),
                            Value = orderCode.ToString()
                        }, new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
                        },
                    };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public List<long> SearchForAdmin(string codeOrName, string username, byte? storeType, EStoreStatus? status,
            int? pageNumber = null, int? rowspPage = null)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@codeOrName", string.IsNullOrEmpty(codeOrName) ? null : codeOrName);
                parameters.Add("@username", string.IsNullOrEmpty(username) ? null : username);
                parameters.Add("@storeType", storeType);
                parameters.Add("@status", status);
                parameters.Add("@pageNumber", (pageNumber != 0 && pageNumber > 0) ? pageNumber : 1);
                parameters.Add("@rowspPage", (rowspPage != 0 && rowspPage > 0) ? rowspPage : StaticNembericInBL.CountOfItemsInAdminPages);

                List<long> storeCodes = db.Query<long>(@"select Id from [dbo].[Store] where
                        (@username is null or [dbo].[Store].UserCode in(select Id from [User] where [UserName] like @username or [UserName] like '%'+@username or [UserName] like '%'+@username+'%')) and
                        (@storeType is null or [StoreTypeCode]=@storeType) and 
                        (@status is null or [StoreStatus]=@status) and
                        (@codeOrName is null or 
                        ([Name] like @codeOrName or [Name] like '%'+@codeOrName or [Name] like '%'+@codeOrName+'%' or 
                        [Id] like @codeOrName or [Id] like '%'+@codeOrName or [Id] like '%'+@codeOrName+'%')) 
                          order by [RegisterDate] desc
                            OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
                            FETCH NEXT @RowspPage ROWS ONLY;", parameters).ToList();
                EnsureCloseConnection(db);
                return storeCodes;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, codeOrName);
            }
        }

        public bool AfterStatusChange(string userCode, long storeCode, EStoreStatus status, ISession session)
        {
            try
            {
                switch (status)
                {
                    case EStoreStatus.Active:
                        {
                            //فعال شدن تمام محصولاتی که معلق بوده اند
                            List<Product> result = session.Query<Product>().Where(p => p.StoreCode == storeCode &&
                                p.Status == EProductStatus.Suspended).ToList();
                            foreach (Product item in result)
                            {
                                item.Status = EProductStatus.Active;
                                new ProductBL().UpdateWhitOutCommitTransaction(item, session);
                            }

                            //فعال سازی کاربر جهت دسترسی به پنل
                            User user = new UserBL().GetById(userCode);
                            user.IsActive = true;
                            new UserBL().Update(user);
                        }
                        break;
                    case EStoreStatus.Inactive:
                        {
                            //معلق شدن تمام محصولات فعال
                            List<Product> result = session.Query<Product>().Where(p => p.StoreCode == storeCode &&
                                p.Status == EProductStatus.Active).ToList();
                            foreach (Product item in result)
                            {
                                item.Status = EProductStatus.Suspended;
                                new ProductBL().UpdateWhitOutCommitTransaction(item, session);
                            }

                            //غیر فعال سازی کاربر جهت عدم دسترسی به پنل
                            User user = new UserBL().GetById(userCode);
                            user.IsActive = false;
                            new UserBL().Update(user);
                        }
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => status),
                            Value = status.ToString()
                        }, new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
                        },
                    };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        /// <summary>
        /// این تابع دقیقا چک میکنه اگه فروشگاه محصولی داشته باشه که تو سفارشی باشه، دیگه حذفش نمیکنه و غیرفعالش میکنه
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public StoreRemovingResultDataModel RemoveStore(long storeCode)
        {
            ISession session = null;

            try
            {
                List<string> imagesWantToDelete = new List<string>();
                var lstProducts = new ProductBL().GetAllProductByStoreCode(storeCode);
                bool haveAnyOrder = false;

                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                foreach (Product item in lstProducts)
                {
                    //چک کنم برا این محصول سفارشی ثبت شده یا نه
                    bool haveOrder = new OrderProductsBL().CheckHaveOrderWithThisProduct(item.Id);
                    if (haveOrder)
                    {
                        haveAnyOrder = true;

                        //عکس های فرعی محصول باید حذف شه
                        List<ProductImage> lstImages = new ProductImageBL().GetAllProductImgAddress(item.Id);
                        imagesWantToDelete.AddRange(lstImages.Select(productImage => productImage.ImgAddress));
                        foreach (ProductImage productImage in lstImages)
                        {
                            new ProductImageBL().DeleteWhitOutCommitTransaction(productImage.Id, session);
                        }

                        //وضعیت محصولم باید غیر فعال شه
                        item.Status = EProductStatus.Inactive;
                        new ProductBL().UpdateWhitOutCommitTransaction(item, session);
                    }
                    else
                    {
                        //عکس اصلی محصول
                        imagesWantToDelete.Add(item.ImgAddress);

                        //عکس های فرعی محصول باید حذف شه
                        List<ProductImage> lstImages = new ProductImageBL().GetAllProductImgAddress(item.Id);
                        imagesWantToDelete.AddRange(lstImages.Select(productImage => productImage.ImgAddress));

                        //محصول باید حذف شه و چون با عکس های فرعی کسکید هست اونا هم حذف میشن
                        new ProductBL().DeleteWhitOutCommitTransaction(item.Id, session);
                    }
                }

                Store store = new StoreBL().SelectOne(storeCode);
                //تصاویر فروشگاه
                List<StoreImage> lstStoreImages = new StoreImageBL().GetAllByStoreCode(storeCode);
                imagesWantToDelete.AddRange(lstStoreImages.Select(image => image.ImgAddress));

                bool result;
                if (haveAnyOrder)
                {
                    //عکس های فرعی فروشگاه حذف شن
                    foreach (StoreImage storeImage in lstStoreImages)
                    {
                        new StoreImageBL().DeleteWhitOutCommitTransaction(storeImage.Id, session);
                    }

                    //وضعیت فروشگاه غیر فعال شه
                    store.StoreStatus = EStoreStatus.Inactive;
                    result = new StoreBL().UpdateWhitOutCommitTransaction(store, session);
                    if (result)
                        new StoreBL().AfterStatusChange(store.UserCode, store.Id, EStoreStatus.Inactive, session);
                }
                else
                {
                    //فروشگاه حذف شه با تمام متعلقاتش
                    imagesWantToDelete.Add(store.ImgAddress);
                    imagesWantToDelete.Add(store.LogoAddress);
                    imagesWantToDelete.Add(new SellerBL().GetSellerPhotoAddres(store.SallerCode));

                    new SellerBL().Delete(new SellerBL().SelectOne(store.SallerCode).Id);
                    result = new UserBL().DeleteById(store.UserCode);
                }

                if (result)
                {
                    session.Transaction.Commit();
                }

                return new StoreRemovingResultDataModel()
                {
                    Success = result,
                    ImageAddressList = imagesWantToDelete
                };
            }
            catch (Exception ex)
            {
                if (session != null)
                    session.Transaction.Rollback();
                session = null;
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }
    }
}
