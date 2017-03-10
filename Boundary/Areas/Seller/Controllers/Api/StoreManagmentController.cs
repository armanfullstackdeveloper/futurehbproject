using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using Boundary.Controllers.Api;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Seller.Controllers.Api
{
    [Authorize(Roles = StaticString.Role_Seller)]
    [RoutePrefix("api/store/storemanagment")]
    public class StoreManagmentController : ApiController
    {
        [HttpGet]
        [Route("editStore")]
        public IHttpActionResult EditStore()
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                StoreEditDataModel storeEditDataModel = new StoreBL().GetStoreForEdit(storeSessionDataModel.StoreCode);
                if (storeEditDataModel == null || storeEditDataModel.StoreCode == 0)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
                return Json(JsonResultHelper.SuccessResult(storeEditDataModel));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
        /// دریافت تمامی دسته بندی های سطح اول
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getCategories")]
        public IHttpActionResult GetCategories() 
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new StatesAndCategoriesViewModel()
                {
                    Categories = new CategoryBL().GetFirstLevel()
                }));
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

        [HttpPost]
        [Route("editStore")]
        public IHttpActionResult EditStore(StoreEditDataModel storeEditDataModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                storeEditDataModel.StoreCode = storeSessionDataModel.StoreCode;
                if (!new StoreBL().EditStore(storeEditDataModel))
                {
                    User user = new UserBL().GetById(userId);
                    if (user != null)
                    {
                        user.Email = storeEditDataModel.Email;
                        new UserBL().Update(user);
                    }

                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
                return Json(JsonResultHelper.SuccessResult());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeEditDataModel),
                            Value = JObject.FromObject(storeEditDataModel).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeEditDataModel),
                            Value = JObject.FromObject(storeEditDataModel).ToString()
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
        [Route("editSeller")]
        public IHttpActionResult EditSallerProfile()
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                DataModel.Entities.RelatedToStore.Seller saller = new SellerBL().SelectOne(storeSessionDataModel.SellerCode);
                if (saller == null || saller.Id == 0)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
                return Json(JsonResultHelper.SuccessResult(saller));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }

        [Route("editSeller")]
        public IHttpActionResult EditSallerProfile(DataModel.Entities.RelatedToStore.Seller saller)
        {
            if (!ModelState.IsValid)
                return Json(JsonResultHelper.FailedResultWithMessage());
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                DataModel.Entities.RelatedToStore.Seller storeSaller = new SellerBL().SelectOne(storeSessionDataModel.SellerCode);
                storeSaller.Comments = saller.Comments;
                storeSaller.IsMale = saller.IsMale;
                storeSaller.Name = saller.Name;
                storeSaller.NationalCode = saller.NationalCode;

                bool editResult = new SellerBL().Update(storeSaller);
                if (!editResult)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
                return Json(JsonResultHelper.SuccessResult());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => saller),
                            Value = JObject.FromObject(saller).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => saller),
                            Value = JObject.FromObject(saller).ToString()
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

        [Route("editStorePhoto")]
        public IHttpActionResult EditStoreMainPhoto(int? top = 0, int? left = 0, int? bottom = 0, int? right = 0)
        {
            top = top >= 0 ? top : 0;
            left = left >= 0 ? left : 0;
            bottom = bottom >= 0 ? bottom : 0;
            right = right >= 0 ? right : 0;
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                var httpRequest = System.Web.HttpContext.Current.Request;
                string rootPath;
                HttpPostedFile postedFile = null;
                string filePath = null;
                string newName = null;
                string extension = null;

                if (httpRequest.Files.Count > 0)
                {
                    postedFile = httpRequest.Files[0];

                    string upperFilename = postedFile.FileName.ToLower();
                    //age jozve anvae mojaz nabod hichi
                    if (!upperFilename.EndsWith(".jpeg") &&
                        !upperFilename.EndsWith(".jpg") &&
                        !upperFilename.EndsWith(".png") &&
                        !upperFilename.EndsWith(".gif"))
                    {
                        return Json(JsonResultHelper.FailedResultWithMessage("فایل نامعتبر است"));
                    }

                    filePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Saller/"
                            + storeSessionDataModel.StateCode + "/" + storeSessionDataModel.CityCode + "/" +
                            storeSessionDataModel.StoreCode + "/StorePhoto");

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    newName = Guid.NewGuid().ToString();
                    extension = Path.GetExtension(postedFile.FileName);

                    rootPath = filePath.Substring(filePath.IndexOf("Content", System.StringComparison.Ordinal));
                    rootPath = System.IO.Path.Combine(rootPath, newName + extension);
                }
                else
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }

                string lastBoxImgAddress = new StoreBL().GetStorePhotoAddres(storeSessionDataModel.StoreCode);
                bool result = new StoreBL().EditStorePhotoAddress(storeSessionDataModel.StoreCode, rootPath);
                //age sabt shod,aks ghabli hazf va jadid zakhire beshe
                if (result)
                {
                    if (httpRequest.Files.Count > 0 && string.IsNullOrEmpty(rootPath) == false)
                    {
                        WebImage orginalImage = new WebImage(postedFile.InputStream);
                        orginalImage.Crop((int)top, (int)left, (int)bottom, (int)right);
                        if (orginalImage.Width > 1360 || orginalImage.Height > 380)
                        {
                            orginalImage.Resize(1360, 380);
                        }

                        orginalImage.Save(filePath + "/" + newName + extension);

                        //ghabli ro ham hazf mikonim
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress)))
                        {
                            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress));
                        }
                    }
                    return Json(JsonResultHelper.SuccessResult(rootPath));
                }
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }

        [Route("editStoreLogo")]
        public IHttpActionResult EditStoreLogo(int? top = 0, int? left = 0, int? bottom = 0, int? right = 0)
        {
            top = top >= 0 ? top : 0;
            left = left >= 0 ? left : 0;
            bottom = bottom >= 0 ? bottom : 0;
            right = right >= 0 ? right : 0;
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                var httpRequest = System.Web.HttpContext.Current.Request;
                string rootPath;
                HttpPostedFile postedFile;
                string filePath;
                string newName;
                string extension;

                if (httpRequest.Files.Count > 0)
                {
                    postedFile = httpRequest.Files[0];

                    string upperFilename = postedFile.FileName.ToLower();
                    //age jozve anvae mojaz nabod hichi
                    if (!upperFilename.EndsWith(".jpeg") &&
                        !upperFilename.EndsWith(".jpg") &&
                        !upperFilename.EndsWith(".png") &&
                        !upperFilename.EndsWith(".gif"))
                    {
                        return Json(JsonResultHelper.FailedResultWithMessage("فایل نامعتبر است"));
                    }

                    filePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Saller/"
                            + storeSessionDataModel.StateCode + "/" + storeSessionDataModel.CityCode + "/" +
                            storeSessionDataModel.StoreCode + "/StoreLogo");

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    newName = Guid.NewGuid().ToString();
                    extension = Path.GetExtension(postedFile.FileName);

                    rootPath = filePath.Substring(filePath.IndexOf("Content", System.StringComparison.Ordinal));
                    rootPath = System.IO.Path.Combine(rootPath, newName + extension);
                }
                else
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }

                string lastBoxImgAddress = new StoreBL().GetLogoAddres(storeSessionDataModel.StoreCode);
                bool result = new StoreBL().EditStoreLogoAddress(storeSessionDataModel.StoreCode, rootPath);
                //age sabt shod,aks ghabli hazf va jadid zakhire beshe
                if (result)
                {
                    if (httpRequest.Files.Count > 0 && string.IsNullOrEmpty(rootPath) == false)
                    {
                        WebImage orginalImage = new WebImage(postedFile.InputStream);
                        orginalImage.Crop((int)top, (int)left, (int)bottom, (int)right);
                        if (orginalImage.Width > 500 || orginalImage.Height > 500)
                        {
                            orginalImage.Resize(500, 500);
                        }

                        orginalImage.Save(filePath + "/" + newName + extension);

                        //ghabli ro ham hazf mikonim
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress)))
                        {
                            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress));
                        }
                    }
                    return Json(JsonResultHelper.SuccessResult(rootPath));
                }
                return Json(JsonResultHelper.FailedResultWithMessage());
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

        [Route("editSallerPhoto")]
        public IHttpActionResult EditSallerPhoto(int? top = 0, int? left = 0, int? bottom = 0, int? right = 0)
        {
            top = top >= 0 ? top : 0;
            left = left >= 0 ? left : 0;
            bottom = bottom >= 0 ? bottom : 0;
            right = right >= 0 ? right : 0;
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                var httpRequest = System.Web.HttpContext.Current.Request;
                string rootPath;
                HttpPostedFile postedFile = null;
                string filePath = null;
                string newName = null;
                string extension = null;

                if (httpRequest.Files.Count > 0)
                {
                    postedFile = httpRequest.Files[0];

                    string upperFilename = postedFile.FileName.ToLower();
                    //age jozve anvae mojaz nabod hichi
                    if (!upperFilename.EndsWith(".jpeg") &&
                        !upperFilename.EndsWith(".jpg") &&
                        !upperFilename.EndsWith(".png") &&
                        !upperFilename.EndsWith(".gif"))
                    {
                        return Json(JsonResultHelper.FailedResultWithMessage("فایل نامعتبر است"));
                    }

                    filePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Saller/"
                            + storeSessionDataModel.StateCode + "/" + storeSessionDataModel.CityCode + "/" +
                            storeSessionDataModel.StoreCode + "/SallerPhoto");

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    newName = Guid.NewGuid().ToString();
                    extension = Path.GetExtension(postedFile.FileName);

                    rootPath = filePath.Substring(filePath.IndexOf("Content", System.StringComparison.Ordinal));
                    rootPath = System.IO.Path.Combine(rootPath, newName + extension);
                }
                else
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }

                string lastBoxImgAddress = new SellerBL().GetSellerPhotoAddres(storeSessionDataModel.SellerCode);
                bool result = new SellerBL().EditSallerPhotoAddress(storeSessionDataModel.SellerCode, rootPath);
                //age sabt shod,aks ghabli hazf va jadid zakhire beshe
                if (result)
                {
                    if (httpRequest.Files.Count > 0 && string.IsNullOrEmpty(rootPath) == false)
                    {
                        WebImage orginalImage = new WebImage(postedFile.InputStream);
                        orginalImage.Crop((int)top, (int)left, (int)bottom, (int)right);
                        if (orginalImage.Width > 500 || orginalImage.Height > 500)
                        {
                            orginalImage.Resize(500, 500);
                        }

                        orginalImage.Save(filePath + "/" + newName + extension);

                        //ghabli ro ham hazf mikonim
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress)))
                        {
                            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + lastBoxImgAddress));
                        }
                    }
                    return Json(JsonResultHelper.SuccessResult(rootPath));
                }
                return Json(JsonResultHelper.FailedResultWithMessage());
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

        //[Route("UpdateUserInfo")]
        //public IHttpActionResult UpdateUserInfo(StoreRegisterDataModel userInfo)
        //{
        //    if (!ModelState.IsValid)
        //        return Json(JsonResultHelper.FailedResultWithMessage());
        //    try
        //    {
        //        #region getting store Id

        //        string userId = this.RequestContext.Principal.Identity.GetUserId();
        //        StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
        //        if (storeSessionDataModel == null)
        //            return Json(JsonResultHelper.FailedResultOfWrongAccess());

        //        #endregion getting store Id

        //        User user = new UserBL().GetById(userId);
        //        //user.UserName = userInfo.UserName;
        //        user.Password = userInfo.Password;
        //        user.Email = userInfo.Email;
        //        var result = new UserBL().Update(user);


        //        if (result.DbMessage.MessageType != MessageType.Success)
        //            return Json(JsonResultHelper.FailedResultWithMessage());
        //        return Json(JsonResultHelper.SuccessResult());
        //    }
        //    catch (MyExceptionHandler exp1)
        //    {
        //        try
        //        {
        //            List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
        //            {
        //                new ActionInputViewModel()
        //                {
        //                    Name = HelperFunctionInBL.GetVariableName(() => userInfo),
        //                    Value = JObject.FromObject(userInfo).ToString()
        //                },
        //            };
        //            long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
        //            return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
        //        }
        //        catch (Exception)
        //        {
        //            return Json(JsonResultHelper.FailedResultWithMessage());
        //        }
        //    }
        //    catch (Exception exp3)
        //    {
        //        try
        //        {
        //            List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
        //            {
        //                new ActionInputViewModel()
        //                {
        //                    Name = HelperFunctionInBL.GetVariableName(() => userInfo),
        //                    Value = JObject.FromObject(userInfo).ToString()
        //                },
        //            };
        //            long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
        //            return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
        //        }
        //        catch (Exception)
        //        {
        //            return Json(JsonResultHelper.FailedResultWithMessage());
        //        }
        //    }
        //}

        [HttpGet]
        [Route("removeStore")]
        public IHttpActionResult Delete()
        {
            try
            {
                #region getting store Id

                string userId = this.RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel storeSessionDataModel = new StoreBL().GetSummaryForSession(userId);
                if (storeSessionDataModel == null)
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

                #endregion getting store Id

                StoreRemovingResultDataModel result = new StoreBL().RemoveStore(storeSessionDataModel.StoreCode);
                if (result.Success)
                {
                    foreach (string address in result.ImageAddressList)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + address)))
                        {
                            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/" + address));
                        }
                    }
                    return Json(JsonResultHelper.SuccessResult());
                }
                return Json(JsonResultHelper.FailedResultWithMessage());
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
    }
}
