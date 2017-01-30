using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Areas.Member.Controllers
{
    /// <summary>
    /// use for both api and mvc
    /// </summary>

    public class PanelController : BaseController
    {
        /// <summary>
        /// صفحه دیفالت پنل کاربر
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = StaticString.Role_Member)]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult GetProfile()
        {
            try
            {
                CheckSessionDataModel checkSessionForMember = CheckMembererSession();
                CheckSessionDataModel checkSessionForSeller = CheckSallerSession();
                if (!checkSessionForMember.IsSuccess && !checkSessionForSeller.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
                long memberCode = (checkSessionForMember.IsSuccess) ? checkSessionForMember.MainSession.Member.Id :
                    checkSessionForSeller.MainSession.Store.MemberCode;

                DataModel.Entities.Member m = new MemberBL().SelectOne(memberCode);
                if (m != null)
                {
                    return Json(JsonResultHelper.SuccessResult(new MemberViewModel()
                    {
                        Id = m.Id,
                        Balance = m.Balance,
                        CityCode = m.CityCode,
                        City = (m.CityCode != null) ? new CityBL().SelectOne((long)m.CityCode).Name : "-",
                        Email = new UserBL().GetById(m.UserCode).Email,
                        Latitude = m.Latitude,
                        Longitude = m.Longitude,
                        MobileNumber = m.MobileNumber,
                        Name = m.Name,
                        PhoneNumber = m.PhoneNumber,
                        PicAddress = m.PicAddress,
                        Place = m.Place,
                        PostalCode = m.PostalCode
                    }), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult UpdateProfile(EditMemberDataModel model)
        {
            try
            {
                CheckSessionDataModel checkSessionForMember = CheckMembererSession();
                CheckSessionDataModel checkSessionForSeller = CheckSallerSession();
                if (!checkSessionForMember.IsSuccess && !checkSessionForSeller.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
                long memberCode = (checkSessionForMember.IsSuccess) ? checkSessionForMember.MainSession.Member.Id :
    checkSessionForSeller.MainSession.Store.MemberCode;

                DataModel.Entities.Member m = new MemberBL().SelectOne(memberCode);
                if (m != null)
                {
                    m.Name = model.Name;
                    m.CityCode = model.CityCode;
                    //m.Email = model.Email;
                    m.Latitude = model.Latitude;
                    m.Longitude = model.Longitude;
                    m.MobileNumber = model.MobileNumber;
                    m.PhoneNumber = model.PhoneNumber;
                    m.Place = model.Place;
                    m.PostalCode = model.PostalCode;
                    m.Latitude = m.Latitude;
                    m.Longitude = m.Longitude;

                    User user = new UserBL().GetById(m.UserCode);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        new UserBL().Update(user);
                    }
                        

                    bool result = new MemberBL().Update(m);
                    if (result)
                        return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => model),
                            Value = JObject.FromObject(model).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => model),
                            Value = JObject.FromObject(model).ToString()
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

        [Authorize(Roles = StaticString.Role_Member + "," + StaticString.Role_Seller)]
        public ActionResult GetActiveBalance()
        {
            try
            {
                CheckSessionDataModel checkSessionForMember = CheckMembererSession();
                CheckSessionDataModel checkSessionForSeller = CheckSallerSession();
                if (!checkSessionForMember.IsSuccess && !checkSessionForSeller.IsSuccess)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
                long memberCode = (checkSessionForMember.IsSuccess) ? checkSessionForMember.MainSession.Member.Id :
checkSessionForSeller.MainSession.Store.MemberCode;

                return Json(JsonResultHelper.SuccessResult(new OrderBL().GetMemberActivedBalance(memberCode)), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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