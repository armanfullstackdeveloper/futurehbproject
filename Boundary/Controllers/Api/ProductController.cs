using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using WebApi.OutputCache.V2;

namespace Boundary.Controllers.Api
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        /// <summary>
        /// جزئیات محصول
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetProduct(long id)
        {
            try
            {
                if (id <= 0)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                CompleteProductForOne completeProduct = new ProductBL().GetOneProduct(id);
                if (completeProduct.Product == null || completeProduct.Product.Id == 0)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                switch (completeProduct.Product.Status)
                {
                    case EProductStatus.Active:
                        break;
                    case EProductStatus.Inactive:
                        return Json(JsonResultHelper.FailedResultWithMessage());
                    case EProductStatus.Suspended:
                    case EProductStatus.New: //اگه فروشنده نیست نمیتونه ببینه
                        {
                            string userId = RequestContext.Principal.Identity.GetUserId();
                            StoreSessionDataModel store = new StoreBL().GetSummaryForSession(userId);
                            if (store == null)
                                return Json(JsonResultHelper.FailedResultWithMessage());
                            break;
                        }
                        

                }
                return Json(JsonResultHelper.SuccessResult(completeProduct));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = id.ToString()
                        }
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                      new ActionInputViewModel
                      {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = id.ToString()
                        }
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
        /// سرچ پیشرفته
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 600)]
        [Route("Search")]
        public async Task<IHttpActionResult> Search(SearchParametersDataModel filters)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(JsonResultHelper.FailedResultOfInvalidInputs());

                List<EProductStatus> lstStatus = new List<EProductStatus> { EProductStatus.Active };
                #region getting store Id

                string userId = RequestContext.Principal.Identity.GetUserId();
                StoreSessionDataModel store = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    store = new StoreBL().GetSummaryForSession(userId);
                    //که اگه خود فروشنده بود بتونه همه محصولاتشو چه فعال و چه معلق رو ببینه
                    if (store != null)
                    {
                        lstStatus.Add(EProductStatus.New);
                        lstStatus.Add(EProductStatus.Suspended);
                    }

                }

                #endregion getting store Id

                bool? haveImage = null;
                if (store == null)
                    haveImage = true;

                SearchResultViewModel result = await new ProductBL().Search(searchParameters: filters, status: lstStatus, haveImage: haveImage);

                //اگه قرار بود محصولات معلقو هم بیاره باید  فقط اونایی رو بیاره که مال خود فروشگاه لاگین باشند
                //یعنی نباید معلق های بقیه رو ببینه
                if ((lstStatus.Contains(EProductStatus.Suspended) || lstStatus.Contains(EProductStatus.New)) && store != null)
                {
                    result.ProductsSummery.RemoveAll(
                        p => p.StoreCode != store.StoreCode && (p.Status == EProductStatus.Suspended || p.Status == EProductStatus.New));
                }
                return Json(JsonResultHelper.SuccessResult(result));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => filters),
                            Value = JObject.FromObject(filters).ToString()
                        }
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => filters),
                            Value = JObject.FromObject(filters).ToString()
                        }
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
        /// اطلاعات مورد نیاز صفحه سرچ
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 600)]
        [Route("getRequiredItemsForSearch")]
        public async Task<IHttpActionResult> GetRequiredItemsForSearch(long categoryCode)
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(await new CategoryBL().GetRequiredItemsForSearchAsync(categoryCode)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => categoryCode),
                            Value = categoryCode.ToString()
                        }
                    };
                    long? code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(new { success = false, response = StaticString.Message_UnSuccessFull, trackingCode = code });
                }
                catch (Exception)
                {
                    return Json(new { success = false, response = StaticString.Message_UnSuccessFull, trackingCode = string.Empty });
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                       new ActionInputViewModel
                       {
                            Name = HelperFunctionInBL.GetVariableName(() => categoryCode),
                            Value = categoryCode.ToString()
                        }
                    };
                    long? code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(new { success = false, response = StaticString.Message_UnSuccessFull, trackingCode = code });
                }
                catch (Exception)
                {
                    return Json(new { success = false, response = StaticString.Message_UnSuccessFull, trackingCode = string.Empty });
                }
            }
        }
    }
}
