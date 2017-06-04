using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToProduct;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Seller.Controllers
{
    [Authorize(Roles = StaticString.Role_Seller)]
    public class ProductManagmentController : BaseController
    {
        public ActionResult AddProduct()
        {
            try
            {
                CheckSessionDataModel checkSession = CheckSallerSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                // کتگوری هایی که این فروشنده قادر به درج کالا برایشان است
                // که ساب کت هاشونم داخل لیستی داخل همین کتگوری هاست
                List<Category> lsCategories = new CategoryBL().GetAllForStore(checkSession.MainSession.Store.StoreCode);
                return View(lsCategories);
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

        public async Task<ActionResult> ProductList()
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }

            SearchResultViewModel result = await new ProductBL().SearchAsync(new SearchParametersDataModel()
            {
                StoreCode = checkSession.MainSession.Store.StoreCode
            }, new List<EProductStatus>() { EProductStatus.Active, EProductStatus.Suspended });

            return View(result);
        }

        public ActionResult EditProduct()
        {
            try
            {
                CheckSessionDataModel checkSession = CheckSallerSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                // کتگوری هایی که این فروشنده قادر به درج کالا برایشان است
                // که ساب کت هاشونم داخل لیستی داخل همین کتگوری هاست
                List<Category> lsCategories = new CategoryBL().GetAllForStore(checkSession.MainSession.Store.StoreCode);
                return View(lsCategories);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
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

        public ActionResult DeleteProduct()
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult OrdersList()
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View(new OrderBL().GetStoreOrders(checkSession.MainSession.Store.StoreCode));
        }
    }
}