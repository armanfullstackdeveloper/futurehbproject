using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Helper;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    public class StoreController : BaseController
    {
        [Route("Shop")]
        public ActionResult ShopPage(string shopname,long? id=null) 
        {
            try
            {
                long? storeCode = id ?? new StoreBL().GetStoreCodeByHomePage(shopname);
                if (storeCode == null || storeCode <= 0)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                
                StoreDetailsViewModel storeDetails = new StoreBL().GetOneStoreDetails((long) storeCode);
                
                //اگه فروشگاه وجود نداشت و یا غیر فعال بود، نمایش داده نمیشه
                if (storeDetails == null || storeDetails.StoreStatus == EStoreStatus.Inactive)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);


                List<EProductStatus> lstStatus = new List<EProductStatus>() { EProductStatus.Active };
                #region getting store Id

                CheckSessionDataModel checkSession = CheckSallerSession();
                if (checkSession.IsSuccess)
                {
                    if(checkSession.MainSession.Store!=null)
                        lstStatus.Add(EProductStatus.Suspended);
                }

                #endregion getting store Id

                SearchResultViewModel result = new ProductBL().Search(new SearchParametersDataModel()
                {
                    StoreCode = storeCode
                }, lstStatus);

                //اگه قرار بود محصولات معلقو هم بیاره باید  فقط اونایی رو بیاره که مال خود فروشگاه لاگین باشند
                //یعنی نباید معلق های بقیه رو ببینه
                //چون این اکشنو هم میتونه خود فروشنده فراخوانی کنه هم دیگران
                if (lstStatus.Contains(EProductStatus.Suspended) && checkSession.MainSession.Store != null)
                {
                    result.ProductsSummery.RemoveAll(p => p.StoreCode != checkSession.MainSession.Store.StoreCode && 
                        p.Status == EProductStatus.Suspended);
                }

                StoreViewModel storeViewModel=new StoreViewModel()
                {
                    StoreDetailes = storeDetails,
                    Products = result
                };
                return View(storeViewModel);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => shopname),
                            Value = shopname
                        },
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = id.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
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
                            Name = HelperFunctionInBL.GetVariableName(() => shopname),
                            Value = shopname
                        },new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = id.ToString()
                        },
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult AddStore()
        {
            try
            {
                return View(new CategoryBL().GetFirstLevel());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        //new ActionInputViewModel()
                        //{
                        //    Name = HelpfulFunction.GetVariableName(() => X),
                        //    Value = X
                        //},
                    };
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        //new ActionInputViewModel()
                        //{
                        //    Name = HelpfulFunction.GetVariableName(() => X),
                        //    Value = X
                        //},
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult AddShop()
        {
            try
            {
                return View(new CategoryBL().GetFirstLevel());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        //new ActionInputViewModel()
                        //{
                        //    Name = HelpfulFunction.GetVariableName(() => X),
                        //    Value = X
                        //},
                    };
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        //new ActionInputViewModel()
                        //{
                        //    Name = HelpfulFunction.GetVariableName(() => X),
                        //    Value = X
                        //},
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}