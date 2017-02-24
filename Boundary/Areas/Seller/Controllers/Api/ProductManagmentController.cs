using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToProduct;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using ImageResizer;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Boundary.Areas.Seller.Controllers.Api
{
    [Authorize(Roles = StaticString.Role_Seller)]
    [RoutePrefix("api/store/productmanagment")]
    public class ProductManagmentController : ApiController
    {
        [HttpGet]
        [Route("getCategories")]
        public IHttpActionResult GetStoreCategories()
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                List<Category> lsCategories = new CategoryBL().GetAllForStore(store.StoreCode);
                return Json(JsonResultHelper.SuccessResult(lsCategories));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        //new ActionInputViewModel()
                        //{
                        //    Name = HelpfulFunction.GetVariableName(() => categoryCode),
                        //    Value = categoryCode.ToString()
                        //},
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        //new ActionInputViewModel()
                        // {
                        //     Name = HelpfulFunction.GetVariableName(() => categoryCode),
                        //     Value = categoryCode.ToString()
                        // },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }

        [HttpGet]
        [Route("getRequiredItemsForNewProduct")]
        public IHttpActionResult GetRequiredItemsForNewProduct(long catCode)
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new CategoryBL().GetRequiredItemsForNewProduct(catCode)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                       new ActionInputViewModel()
                         {
                             Name = HelperFunctionInBL.GetVariableName(() => catCode),
                             Value = catCode.ToString()
                         },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                         {
                             Name = HelperFunctionInBL.GetVariableName(() => catCode),
                             Value = catCode.ToString()
                         },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [HttpPost]
        [Route("save")]
        public IHttpActionResult Save(ProductRegisterDataModel productRegisterDataModel)
        {
            ISession session = null;
            try
            {
                if (!ModelState.IsValid || productRegisterDataModel.ProductMainAttributeDataModels == null
                    || productRegisterDataModel.ProductMainAttributeDataModels.CategoryCode < 1)
                {
                    return Json(JsonResultHelper.FailedResultOfInvalidInputs());
                }

                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                //اگه وضعیتش فعال نباشه یعنی نمیتونه کالای جدید درج کنه
                if (store.Status != EStoreStatus.Active)
                    return Json(JsonResultHelper.FailedResultWithMessage(StaticString.Message_SuspendedStoreStatus));

                if (productRegisterDataModel.ProductMainAttributeDataModels.CanSend &&
                    (productRegisterDataModel.ProductMainAttributeDataModels.PostalCostInCountry == null || productRegisterDataModel.ProductMainAttributeDataModels.PostalCostInTown == null))
                    return Json(JsonResultHelper.FailedResultWithMessage("هزینه های ارسال کالا پر نشده اند"));

                #region transaction

                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                long registeredProductCode = new ProductBL().InsertWhitOutCommitTransaction(new Product()
                {
                    Name = productRegisterDataModel.ProductMainAttributeDataModels.Name,
                    BrandCode = productRegisterDataModel.ProductMainAttributeDataModels.BrandCode,
                    CanGiveBack = productRegisterDataModel.ProductMainAttributeDataModels.CanGiveBack ?? false,
                    CanSend = productRegisterDataModel.ProductMainAttributeDataModels.CanSend,
                    PostalCostInCountry = productRegisterDataModel.ProductMainAttributeDataModels.PostalCostInCountry,
                    PostalCostInTown = productRegisterDataModel.ProductMainAttributeDataModels.PostalCostInTown,
                    CategoryCode = productRegisterDataModel.ProductMainAttributeDataModels.CategoryCode,
                    Changeability = productRegisterDataModel.ProductMainAttributeDataModels.Changeability ?? false,
                    DiscountedPrice = productRegisterDataModel.ProductMainAttributeDataModels.DiscountedPrice,
                    Price = productRegisterDataModel.ProductMainAttributeDataModels.Price,
                    IsExist = productRegisterDataModel.ProductMainAttributeDataModels.IsExist ?? true,
                    Warranty = productRegisterDataModel.ProductMainAttributeDataModels.Warranty,
                    MadeIn = productRegisterDataModel.ProductMainAttributeDataModels.MadeIn,
                    //
                    StoreCode = store.StoreCode,
                    Status = EProductStatus.New,
                    RegisterDate = PersianDateTime.Now.Date.ToInt()
                }, session);

                if (registeredProductCode <= 0)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                if (productRegisterDataModel.ProductAttributeWithoutItemsDataModels != null &&
                    productRegisterDataModel.ProductAttributeWithoutItemsDataModels.Count > 0)
                {
                    foreach (ProductAttributeWithoutItemsDataModel model in productRegisterDataModel.ProductAttributeWithoutItemsDataModels)
                    {
                        new ProductNonSearchableAttributeValueBL().SaveWithoutCommit(
                            new ProductNonSearchableAttributeValue()
                            {
                                AttributeCode = model.AttributeCode,
                                ProductCode = registeredProductCode,
                                Value = model.Value
                            }, session);
                    }
                }

                List<long> multiSelectAttr = new AttributeBL().
                    GetByCategoryCode(productRegisterDataModel.ProductMainAttributeDataModels.CategoryCode).Where(atr => atr.AttributeTypeCode == 3).Select(atr => atr.Id).ToList();
                if (productRegisterDataModel.ProductAttributeWithItemsDataModels != null &&
                  productRegisterDataModel.ProductAttributeWithItemsDataModels.Count > 0)
                {
                    foreach (ProductAttributeWithItemsForSaveAndUpdate model in productRegisterDataModel.ProductAttributeWithItemsDataModels)
                    {
                        if (model.Code != 0 && model.Values.Count > 0)
                        {
                            foreach (long attrCode in model.Values)
                            {
                                new ProductSearchableAttributeValueBL().SaveWithoutCommit(
                                     new ProductSearchableAttributeValue()
                                     {
                                         AttributeCode = model.Code,
                                         AttributeValueCode = attrCode,
                                         ProductCode = registeredProductCode
                                     }, session);

                                //in sharto mizaram ke nashe baraye onaii ke multiSelect nistan, chand value ezafe kard
                                if (multiSelectAttr.Contains(model.Code) == false)
                                    break;
                            }
                        }


                        if (model.TextValues != null && model.TextValues.Count > 0)
                        {
                            //check is exsist before
                            var currentValues = new AttributeValueBL().GetByAttributeCode(model.Code);
                            //for new value seller inport
                            foreach (string attrValue in model.TextValues)
                            {
                                if (currentValues != null && currentValues.Count > 0)
                                {
                                    if (!currentValues.Select(a => a.Value).Contains(attrValue.Trim()))
                                    {
                                        long attrCode = new AttributeValueBL().InsertWhitOutCommitTransaction(new AttributeValue(),
                                            session);

                                        new ProductSearchableAttributeValueBL().SaveWithoutCommit(
                                            new ProductSearchableAttributeValue()
                                            {
                                                AttributeCode = model.Code,
                                                AttributeValueCode = attrCode,
                                                ProductCode = registeredProductCode
                                            }, session);

                                        //in sharto mizaram ke nashe baraye onaii ke multiSelect nistan, chand value ezafe kard
                                        if (multiSelectAttr.Contains(model.Code) == false)
                                            break;
                                    }
                                }
                            }
                        }

                    }
                }

                if (productRegisterDataModel.ProductMainAttributeDataModels.Colors != null &&
                    productRegisterDataModel.ProductMainAttributeDataModels.Colors.Count > 0)
                {
                    foreach (long colorCode in productRegisterDataModel.ProductMainAttributeDataModels.Colors)
                    {
                        new ProductColorBL().SaveWithoutCommit(new ProductColor()
                        {
                            ColorCode = colorCode,
                            ProductCode = registeredProductCode
                        }, session);
                    }
                }


                if (productRegisterDataModel.ProductAdditionalAttrbiutes != null &&
                    productRegisterDataModel.ProductAdditionalAttrbiutes.Count > 0)
                {
                    foreach (ProductAdditionalAttrbiuteDataModel model in productRegisterDataModel.ProductAdditionalAttrbiutes)
                    {
                        new ProductAdditionalAttrbiuteBL().InsertWhitOutCommitTransaction(new ProductAdditionalAttrbiute()
                        {
                            ProductCode = registeredProductCode,
                            Title = model.Title,
                            Value = model.Value
                        },
                            session);
                    }
                }


                session.Transaction.Commit();

                #endregion

                return Json(JsonResultHelper.SuccessResult(registeredProductCode));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productRegisterDataModel),
                            Value = JObject.FromObject(productRegisterDataModel).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                          new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productRegisterDataModel),
                            Value = JObject.FromObject(productRegisterDataModel).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [HttpGet]
        [Route("GetRequiredItemsForUpdateProduct")]
        public IHttpActionResult GetRequiredItemsForUpdateProduct(long productCode)
        {
            ISession session = null;
            try
            {
                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                ProductRegisterDataModel model = new ProductRegisterDataModel();
                Product temp = new ProductBL().SelectOneWhitOutCommitTransaction(productCode, session);
                model.ProductMainAttributeDataModels = new ProductMainAttributeDataModel()
                {
                    BrandCode = temp.BrandCode,
                    CanGiveBack = temp.CanGiveBack,
                    CanSend = temp.CanSend,
                    CategoryCode = temp.CategoryCode,
                    Changeability = temp.Changeability,
                    DiscountedPrice = temp.DiscountedPrice,
                    IsExist = temp.IsExist,
                    MadeIn = temp.MadeIn,
                    Id = temp.Id,
                    Name = temp.Name,
                    Price = temp.Price,
                    Warranty = temp.Warranty,
                    PostalCostInCountry = temp.PostalCostInCountry,
                    PostalCostInTown = temp.PostalCostInTown
                };
                model.ProductMainAttributeDataModels.Colors = session.Query<ProductColor>().Where(p => p.ProductCode == productCode).Select(p => p.ColorCode).ToList();
                List<ProductImage> lstImages = new ProductImageBL().GetAllProductImgAddress(productCode);
                model.ProductMainAttributeDataModels.OtherImagesAddress = (from x in lstImages
                                                                           select new ImageViewModel()
                                                                           {
                                                                               ImgAddress = x.ImgAddress,
                                                                               ImgCode = x.Id
                                                                           }).ToList();

                model.ProductAttributeWithoutItemsDataModels =
                    new ProductBL().GetProductTextAttributeForEdit(productCode);
                model.ProductAttributeWithItemsDataModels =
                    new ProductBL().GetProductDropDownAttributeForEdit(productCode);

                model.ProductAdditionalAttrbiutes = new ProductAdditionalAttrbiuteBL().GetAllForProductEdit(productCode);

                session.Transaction.Commit();
                return Json(JsonResultHelper.SuccessResult(model));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productCode),
                            Value = productCode.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                    new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productCode),
                            Value = productCode.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [HttpPost]
        [Route("edit")]
        public IHttpActionResult Edit(ProductRegisterDataModel productEditDataModel)
        {
            ISession session = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }

                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                #region transaction

                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();


                Product recentProduct =
                    new ProductBL().SelectOneWhitOutCommitTransaction(
                        productEditDataModel.ProductMainAttributeDataModels.Id, session);

                if (recentProduct.StoreCode != store.StoreCode)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());


                recentProduct.Name = productEditDataModel.ProductMainAttributeDataModels.Name;
                recentProduct.BrandCode = productEditDataModel.ProductMainAttributeDataModels.BrandCode;
                recentProduct.CanGiveBack = productEditDataModel.ProductMainAttributeDataModels.CanGiveBack ?? false;
                recentProduct.CanSend = productEditDataModel.ProductMainAttributeDataModels.CanSend;
                recentProduct.PostalCostInCountry = productEditDataModel.ProductMainAttributeDataModels.PostalCostInCountry;
                recentProduct.PostalCostInTown = productEditDataModel.ProductMainAttributeDataModels.PostalCostInTown;
                recentProduct.CategoryCode = productEditDataModel.ProductMainAttributeDataModels.CategoryCode;
                recentProduct.Changeability = productEditDataModel.ProductMainAttributeDataModels.Changeability ?? false;
                recentProduct.DiscountedPrice = productEditDataModel.ProductMainAttributeDataModels.DiscountedPrice;
                recentProduct.Price = productEditDataModel.ProductMainAttributeDataModels.Price;
                recentProduct.IsExist = productEditDataModel.ProductMainAttributeDataModels.IsExist ?? false;
                recentProduct.Warranty = productEditDataModel.ProductMainAttributeDataModels.Warranty;
                recentProduct.MadeIn = productEditDataModel.ProductMainAttributeDataModels.MadeIn;
                //
                recentProduct.StoreCode = store.StoreCode;
                //recentProduct.Status = ;
                recentProduct.RegisterDate = PersianDateTime.Now.Date.ToInt();

                bool result = new ProductBL().UpdateWhitOutCommitTransaction(recentProduct, session);
                if (!result)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                if (productEditDataModel.ProductAttributeWithoutItemsDataModels != null &&
                    productEditDataModel.ProductAttributeWithoutItemsDataModels.Count > 0)
                {
                    new ProductNonSearchableAttributeValueBL().DeleteAllByProductCode_WhitOutCommitTransaction(productEditDataModel.ProductMainAttributeDataModels.Id, session);
                    foreach (ProductAttributeWithoutItemsDataModel model in productEditDataModel.ProductAttributeWithoutItemsDataModels)
                    {
                        new ProductNonSearchableAttributeValueBL().SaveWithoutCommit(
                            new ProductNonSearchableAttributeValue()
                            {
                                AttributeCode = model.AttributeCode,
                                ProductCode = productEditDataModel.ProductMainAttributeDataModels.Id,
                                Value = model.Value
                            }, session);
                    }
                }

                List<long> multiSelectAttr = new AttributeBL().
                    GetByCategoryCode(productEditDataModel.ProductMainAttributeDataModels.CategoryCode).Where(atr => atr.AttributeTypeCode == 3).Select(atr => atr.Id).ToList();
                if (productEditDataModel.ProductAttributeWithItemsDataModels != null &&
                  productEditDataModel.ProductAttributeWithItemsDataModels.Count > 0)
                {
                    new ProductSearchableAttributeValueBL().DeleteAllByProductCode_WhitOutCommitTransaction(productEditDataModel.ProductMainAttributeDataModels.Id, session);
                    foreach (ProductAttributeWithItemsForSaveAndUpdate model in productEditDataModel.ProductAttributeWithItemsDataModels)
                    {
                        if (model.Code != 0 && model.Values.Count > 0)
                        {
                            foreach (long attrCode in model.Values)
                            {
                                new ProductSearchableAttributeValueBL().SaveWithoutCommit(
                             new ProductSearchableAttributeValue()
                             {
                                 AttributeCode = model.Code,
                                 AttributeValueCode = attrCode,
                                 ProductCode = productEditDataModel.ProductMainAttributeDataModels.Id
                             }, session);

                                //in sharto mizaram ke nashe baraye onaii ke multiSelect nistan, chand value ezafe kard
                                if (multiSelectAttr.Contains(model.Code) == false)
                                    break;
                            }
                        }


                        if (model.TextValues != null && model.TextValues.Count > 0)
                        {
                            //check is exsist before
                            var currentValues = new AttributeValueBL().GetByAttributeCode(model.Code);
                            //for new value seller inport
                            foreach (string attrValue in model.TextValues)
                            {
                                if (currentValues != null && currentValues.Count > 0)
                                {
                                    if (!currentValues.Select(a => a.Value).Contains(attrValue.Trim()))
                                    {
                                        long attrCode = new AttributeValueBL().InsertWhitOutCommitTransaction(new AttributeValue(),
                                            session);

                                        new ProductSearchableAttributeValueBL().SaveWithoutCommit(
                                            new ProductSearchableAttributeValue()
                                            {
                                                AttributeCode = model.Code,
                                                AttributeValueCode = attrCode,
                                                ProductCode = productEditDataModel.ProductMainAttributeDataModels.Id
                                            }, session);

                                        //in sharto mizaram ke nashe baraye onaii ke multiSelect nistan, chand value ezafe kard
                                        if (multiSelectAttr.Contains(model.Code) == false)
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (productEditDataModel.ProductMainAttributeDataModels.Colors != null &&
                    productEditDataModel.ProductMainAttributeDataModels.Colors.Count > 0)
                {
                    new ProductColorBL().DeleteAllByProductCode_WhitOutCommitTransaction(productEditDataModel.ProductMainAttributeDataModels.Id, session);
                    foreach (long colorCode in productEditDataModel.ProductMainAttributeDataModels.Colors)
                    {
                        new ProductColorBL().SaveWithoutCommit(new ProductColor()
                        {
                            ColorCode = colorCode,
                            ProductCode = productEditDataModel.ProductMainAttributeDataModels.Id
                        }, session);
                    }
                }


                if (productEditDataModel.ProductAdditionalAttrbiutes != null &&
                    productEditDataModel.ProductAdditionalAttrbiutes.Count > 0)
                {
                    new ProductAdditionalAttrbiuteBL().DeleteAllByProductCode_WhitOutCommitTransaction(
                        productEditDataModel.ProductMainAttributeDataModels.Id, session);
                    foreach (ProductAdditionalAttrbiuteDataModel model in productEditDataModel.ProductAdditionalAttrbiutes)
                    {
                        new ProductAdditionalAttrbiuteBL().InsertWhitOutCommitTransaction(new ProductAdditionalAttrbiute()
                        {
                            ProductCode = productEditDataModel.ProductMainAttributeDataModels.Id,
                            Title = model.Title,
                            Value = model.Value
                        },
                            session);
                    }
                }


                session.Transaction.Commit();

                #endregion


                return Json(JsonResultHelper.SuccessResult(null));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productEditDataModel),
                            Value = JObject.FromObject(productEditDataModel).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                  new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productEditDataModel),
                            Value = JObject.FromObject(productEditDataModel).ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [HttpPost]
        [Route("remove")]
        public IHttpActionResult Remove(int productId)
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                //عکس های فرعی همواره باید حذف شه
                List<string> images = new ProductImageBL().GetAllProductImgAddress(productId).Select(i => i.ImgAddress).ToList();

                //چک کنم برا این محصول سفارشی ثبت شده یا نه
                bool haveOrder = new OrderProductsBL().CheckHaveOrderWithThisProduct(productId);
                if (haveOrder)
                {
                    Product currentProduct = new ProductBL().SelectOne(productId);
                    currentProduct.Status = EProductStatus.Inactive;
                    if (new ProductBL().Update(currentProduct))
                    {
                        //فقط عکسای فرعیشو پاک کنم
                        foreach (string address in images)
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + address)))
                            {
                                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + address));
                            }
                        }
                        return Json(JsonResultHelper.SuccessResult());
                    }
                }
                else
                {
                    //عکس اصلیشم بره جزو عکس هایی که قراره حذف شن
                    images.Add(new ProductBL().GetImgAddressById(productId));

                    var result = new ProductBL().DeleteWithCheckingIsItForThatStore(productId, store.StoreCode);

                    if (result.DbMessage.MessageType == MessageType.Success)
                    {
                        foreach (string address in images)
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + address)))
                            {
                                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + address));
                            }
                        }
                        return Json(JsonResultHelper.SuccessResult(result.DbMessage.Message));
                    }
                }

                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productId),
                            Value = productId.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                         {
                             Name = HelperFunctionInBL.GetVariableName(() => productId),
                             Value = productId.ToString()
                         },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }

        /// <summary>
        /// برای عکس های فرعی
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("imageremove")]
        public IHttpActionResult DeleteImg(int imageId)
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                ProductImage productImage = new ProductImageBL().SelectOne(imageId);

                var result = new ProductImageBL().DeleteWithCheckingIsItForThatStore(imageId, store.StoreCode);
                if (result.DbMessage.MessageType == MessageType.Success)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/" + productImage.ImgAddress)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath("~/" + productImage.ImgAddress));
                    }
                    return Json(JsonResultHelper.SuccessResult());
                }

                return Json(JsonResultHelper.FailedResultWithMessage(result.DbMessage.Message));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => imageId),
                            Value = imageId.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => imageId),
                            Value = imageId.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }


        [Route("uploadimage")]
        public IHttpActionResult PostFile(long productId, bool isMainImg, int? top = 0, int? left = 0, int? bottom = 0, int? right = 0)
        {
            top = top >= 0 ? top : 0;
            left = left >= 0 ? left : 0;
            bottom = bottom >= 0 ? bottom : 0;
            right = right >= 0 ? right : 0;
            if (productId <= 0)
                return Json(JsonResultHelper.FailedResultWithMessage());

            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                if (store == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                if (!isMainImg)
                {
                    if (new ProductImageBL().GetProductImageCount(productId) >= StaticNemberic.MaximumProductImage)
                        return Json(JsonResultHelper.FailedResultWithMessage("حداکثر تعداد عکس های قابل ثبت، چهار عدد می باشد"));
                }

                var httpRequest = HttpContext.Current.Request;

                if (httpRequest != null && httpRequest.Files != null && httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var recivedFile = httpRequest.Files[file];
                        if (recivedFile == null) continue;

                        string lowerFilename = recivedFile.FileName.ToLower();

                        if (lowerFilename == "blob") //یعنی آپلودره جواده
                        {
                            lowerFilename = httpRequest.Form[3].ToLower();
                        }

                        //age jozve anvae mojaz nabod hichi
                        if (!lowerFilename.EndsWith(".jpeg") &&
                            !lowerFilename.EndsWith(".jpg") &&
                            !lowerFilename.EndsWith(".png") &&
                            !lowerFilename.EndsWith(".gif"))
                        {
                            return Json(JsonResultHelper.FailedResultWithMessage("فایل نامعتبر است"));
                        }

                        string path = "Majazi";
                        if (store.StateCode > 0 && store.CityCode > 0)
                            path = store.StateCode + "/" + +store.CityCode;
                        string filePath = HttpContext.Current.Server.MapPath("~/Content/Images/Saller/"
                                                                                       + path + "/" + store.StoreCode +
                                                                                       "/Products/" + productId);
                        WebImage image = new WebImage(recivedFile.InputStream);
                        image = image.Crop((int)top, (int)left, (int)bottom, (int)right);

                        if (Directory.Exists(filePath) == false)
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        if (isMainImg)
                        {
                            filePath = Path.Combine(filePath, "MainImage");
                            if (Directory.Exists(filePath) == false)
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            else
                            {
                                string mainImgAddress = new ProductBL().GetProductMainImageAddress(productId);
                                Array.ForEach(Directory.GetFiles(filePath), File.Delete);
                                mainImgAddress = mainImgAddress.Replace(@"\MainImage", string.Empty);
                                if (string.IsNullOrWhiteSpace(mainImgAddress) == false && File.Exists(HttpContext.Current.Server.MapPath("~/" + mainImgAddress)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath("~/" + mainImgAddress));
                                }
                            }
                        }

                        string newName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(lowerFilename);
                        string imageAddress = filePath + "/" + newName + extension;
                        image.Save(imageAddress);

                        string rootPath = filePath.Substring(filePath.IndexOf("Content", StringComparison.Ordinal));
                        rootPath = Path.Combine(rootPath, newName + extension);

                        if (isMainImg)
                        {
                            string rootPathForResizedImg = filePath.Substring(filePath.IndexOf("Content", StringComparison.Ordinal));
                            rootPathForResizedImg = Path.Combine(rootPathForResizedImg, newName + extension);

                            QueryResult<Product> result = new ProductBL().UpdateMainImag(new ProductImage()
                            {
                                ImgAddress = rootPathForResizedImg,
                                ProductCode = productId
                            }, store.StoreCode);

                            if (result != null && result.DbMessage.MessageType == MessageType.Success &&
                                new ProductImageBL().GetProductImageCount(productId) < StaticNemberic.MaximumProductImage)
                            {
                                ImageHelper.Resise(imageAddress, StaticNemberic.MaximumImageHeightSize, StaticNemberic.MaximumImageWidthSize, extension.Replace(".", ""));
                                dynamic jsonObject = new JObject();
                                jsonObject.ImageAddress = rootPath;
                                return Ok(JsonResultHelper.SuccessResult(jsonObject));
                            }
                        }
                        else
                        {
                            int result = new ProductImageBL().SaveImageWithCheckingIsItForThatStore(new ProductImage()
                            {
                                ImgAddress = rootPath,
                                ProductCode = productId
                            }, store.StoreCode);
                            if (result != 0)
                            {
                                Product product = new ProductBL().SelectOne(productId);
                                product.RegisterDate = PersianDateTime.Now.Date.ToInt();
                                new ProductBL().Update(product);

                                ImageHelper.Resise(imageAddress, StaticNemberic.MaximumImageHeightSize, StaticNemberic.MaximumImageWidthSize, extension.Replace(".", ""));

                                dynamic jsonObject = new JObject();
                                jsonObject.ImageAddress = rootPath;
                                jsonObject.ImageId = result;
                                return Ok(JsonResultHelper.SuccessResult(jsonObject));
                            }
                        }
                    }
                }
                return Json(JsonResultHelper.FailedResultWithMessage("تصویری ارسال نشده است"));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productId),
                            Value = productId.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => isMainImg),
                            Value = isMainImg.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => top),
                            Value = top.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => left),
                            Value = left.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => bottom),
                            Value = bottom.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => right),
                            Value = right.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productId),
                            Value = productId.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => isMainImg),
                            Value = isMainImg.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => top),
                            Value = top.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => left),
                            Value = left.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => bottom),
                            Value = bottom.ToString()
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => right),
                            Value = right.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }
    }
}
