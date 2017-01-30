using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Boundary.Helper;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Entities.RelatedToProduct;
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Api
{
    [RoutePrefix("api/store")]
    public class StoreController : ApiController
    {
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(long storeCode)
        {
            try
            {
                StoreDetailsViewModel store = new StoreBL().GetOneStoreDetails(storeCode);

                //اگه فروشگاه غیر فعال باشه اطلاعاتش نمایش داده نمیشه
                if (store != null && store.StoreStatus!=EStoreStatus.Inactive)
                    return Json(JsonResultHelper.SuccessResult(store));
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
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

    public class StatesAndCategoriesViewModel
    {
        public List<State> States { get; set; }
        public List<Category> Categories { get; set; }
    }
}
