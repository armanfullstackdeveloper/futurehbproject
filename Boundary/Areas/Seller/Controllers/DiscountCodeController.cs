using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Areas.Seller.Models;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToOrder;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Seller.Controllers
{
    /// <summary>
    /// use for both mvc and api
    /// </summary>
    [Authorize(Roles = StaticString.Role_Seller)]
    public class DiscountCodeController : BaseController
    {
        public ActionResult GetAll(int pageNumer = 1, int rowsPage = 100) 
        {
            try
            {
                CheckSessionDataModel checkSession = CheckSallerSession();
                if (!checkSession.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                List<StoreDiscount> lst = (List<StoreDiscount>)new StoreDiscountBL().FindObject_ExcludeZeroes_WithPageSize(new StoreDiscount()
                {
                    StoreCode = checkSession.MainSession.Store.StoreCode,
                    IsActive = true
                }, pageNumer, (short)rowsPage);
                return View(lst);

            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                            Value = pageNumer.ToString()
                        },  new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowsPage),
                            Value = rowsPage.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                            Value = pageNumer.ToString()
                        },  new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowsPage),
                            Value = rowsPage.ToString()
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
        public ActionResult Index(int pageNumer = 1, int rowsPage = 100)
        {
            try
            {
                CheckSessionDataModel checkSession = CheckSallerSession();
                if (!checkSession.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                List<StoreDiscount> lst = (List<StoreDiscount>)new StoreDiscountBL().FindObject_ExcludeZeroes_WithPageSize(new StoreDiscount()
                    {
                        StoreCode = checkSession.MainSession.Store.StoreCode,
                        IsActive = true
                    }, pageNumer, (short)rowsPage);
                return Json(JsonResultHelper.SuccessResult(lst), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                            Value = pageNumer.ToString()
                        },  new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowsPage),
                            Value = rowsPage.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                            Value = pageNumer.ToString()
                        },  new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowsPage),
                            Value = rowsPage.ToString()
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

        public ActionResult CreateDiscountCode(StoreDiscountRegisterDataModel storeDiscountInput)
        {
            try
            {
                if (string.IsNullOrEmpty(storeDiscountInput.Code) || storeDiscountInput.DiscountPercent <= 0 || storeDiscountInput.DiscountPercent > 100)
                    return Json(JsonResultHelper.FailedResultWithMessage(StaticString.Message_InvalidInputs), JsonRequestBehavior.AllowGet);

                CheckSessionDataModel checkSession = CheckSallerSession();
                if (!checkSession.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                StoreDiscount storeDiscount = new StoreDiscountBL().SelectOneByDiscountCode(storeDiscountInput.Code);
                if (storeDiscount != null)
                    return Json(JsonResultHelper.FailedResultWithMessage("کد تخفیف از قبل موجود می باشد! کد تخفیف دیگری را انتخاب کنید"), JsonRequestBehavior.AllowGet);

                long result = new StoreDiscountBL().Insert(new StoreDiscount()
                {
                    Code = storeDiscountInput.Code,
                    DiscountPercent = storeDiscountInput.DiscountPercent,
                    IsActive = true,
                    IsDisposable = storeDiscountInput.IsDisposable,
                    StoreCode = checkSession.MainSession.Store.StoreCode
                });

                if (result <= 0)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                      new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeDiscountInput),
                            Value = JObject.FromObject(storeDiscountInput).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeDiscountInput),
                            Value = JObject.FromObject(storeDiscountInput).ToString()
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

        public ActionResult InactiveDiscountCode(long id)
        {
            try
            {
                CheckSessionDataModel checkSession = CheckSallerSession();
                if (!checkSession.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                //first find it
                StoreDiscount storeDiscount = new StoreDiscountBL().SelectOne(id);

                if (storeDiscount == null)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                if (storeDiscount.IsActive == true)
                {
                    //now check for if it is used before
                    int usedCount = new OrderBL().SelectCountOfUsingDiscountCode(storeDiscount.Id);
                    if (usedCount > 0)
                    {
                        storeDiscount.IsActive = false;
                        bool result = new StoreDiscountBL().Update(storeDiscount);
                        if (result)
                            return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
                        return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        new StoreDiscountBL().Delete(storeDiscount.Id);
                    }
                }
                return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
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