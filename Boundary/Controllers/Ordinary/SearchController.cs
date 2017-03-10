using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Helper;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.Helpers;
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    public class SearchController : BaseController
    {
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// صفحه ی جزئیات محصول
        /// </summary>
        /// <param name="id"></param>
        /// <param name="store"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [OutputCache(Duration = 60)]
        [Route("Product")]
        public ActionResult GetProduct(int id,string store="",string name="")
        {
            try
            {
                if (id <= 0)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                CompleteProductForOne completeProduct = new ProductBL().GetOneProduct(id);
                if (completeProduct.Product == null || completeProduct.Product.Id == 0)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                switch (completeProduct.Product.Status)
                {
                    case EProductStatus.Active:
                        break;
                    case EProductStatus.Inactive:
                        return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                    case EProductStatus.Suspended: //اگه فروشنده نیست نمیتونه ببینه
                    {
                        CheckSessionDataModel checkSession = CheckSallerSession();
                        if (!checkSession.IsSuccess)
                        {
                            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                        }
                    }
                        break;
                }

                return View(completeProduct);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
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
    }
}