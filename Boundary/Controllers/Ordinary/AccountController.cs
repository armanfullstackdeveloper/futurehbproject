using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Entities.RelatedToStore;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    public class AccountController : Controller
    {
        public ShopFinderUserManager ShopFinderUserManager { get; private set; }

        public AccountController() : this(new ShopFinderUserManager()) { }

        public AccountController(ShopFinderUserManager shopFinderUserManager)
        {
            ShopFinderUserManager = shopFinderUserManager;
        }

        public ActionResult RegisterMember()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterMember([FromBody]RegisterMemberDataModel model, [FromUri]string shopname)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            try
            {
                AppUser user = new AppUser { UserName = model.UserName, Role = StaticRole.Member, RoleCode = StaticRole.Member.Id };
                var result = await ShopFinderUserManager.CreateUser(ref user, model.Password,(model.MemberInfo!=null)? model.MemberInfo.Email:string.Empty);

                if (result != null && result.Succeeded == false)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage((result.Errors!=null && result.Errors.Any())?result.Errors.SingleOrDefault():string.Empty), JsonRequestBehavior.AllowGet);
                }
                if (result == null)
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);


                Member member = new Member()
                {
                    UserCode = user.Id,
                };

                if (model.MemberInfo != null)
                {
                    member.Name = model.MemberInfo.Name;
                    member.CityCode = model.MemberInfo.CityCode;
                    //member.Email = model.MemberInfo.Email;
                    member.Latitude = model.MemberInfo.Latitude;
                    member.Longitude = model.MemberInfo.Longitude;
                    member.MobileNumber = model.MemberInfo.MobileNumber;
                    member.PhoneNumber = model.MemberInfo.PhoneNumber;
                    member.Place = model.MemberInfo.Place;
                    member.PostalCode = model.MemberInfo.PostalCode;
                }

                var cutomerRegisterResult = new MemberBL().Insert(member);

                if (cutomerRegisterResult > 0)
                {
                    await SignInAsync(user, isPersistent: false);

                    //ببینم مشتری فروشگاه خاصی هست یا نه
                    long? storeCode = new StoreBL().GetStoreCodeByHomePage(shopname);
                    if (storeCode != null && storeCode > 0)
                    {
                        new StoreCustomerBL().Save(new StoreCustomer()
                        {
                            MemberCode = cutomerRegisterResult,
                            StoreCode = storeCode
                        });
                    }

                    return Json(JsonResultHelper.SuccessResult(member.Id), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    new UserBL().DeleteById(user.Id);
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }
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


        [System.Web.Mvc.Authorize(Roles = StaticString.Role_Member)]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpgradeToSeller(StoreRegisterForUpgradeMemberToSallerDataModel storeRegister)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }

                string userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Json(JsonResultHelper.FailedResultOfWrongAccess(), JsonRequestBehavior.AllowGet);

                var storeRegisterResult = new StoreBL().FullRegister(new StoreRegisterDataModel()
                {
                    StoreName = storeRegister.StoreName,
                    StoreComments = storeRegister.StoreComments,
                    CommercialCode = storeRegister.CommercialCode,
                    ReagentPhoneNumber = storeRegister.ReagentPhoneNumber,
                    CityCode = storeRegister.CityCode,
                    Place = storeRegister.Place,
                    Latitude = storeRegister.Latitude,
                    Longitude = storeRegister.Longitude,
                    PhoneNumber = storeRegister.PhoneNumber,
                    StoreTypeCode = storeRegister.StoreTypeCode,
                    Website = storeRegister.Website,
                    SallerName = storeRegister.SallerName,
                    NationalCode = storeRegister.NationalCode,
                    SallerComments = storeRegister.SallerComments,
                    IsMale = storeRegister.IsMale,
                    //ListCategoryCode = storeRegister.ListCategoryCode,
                    ListCategoryCode = storeRegister.CategoryCodes.Split(',').Select(Int64.Parse).ToList() // |: javad!
                }, userId);

                //age movafagh sabt shod
                if (storeRegisterResult.DbMessage.MessageType == MessageType.Success)
                {
                    //az table Customer delete beshe //todo: چون اگه خرید کرده باشه اطلاعات خریدشو میخوام و همچنین شاید در آینده خردی کنه
                    //Member member = new MemberBL().GetSummaryForSession(userId);
                    //bool result = false;
                    //if (member != null)
                    //    result = new MemberBL().Delete(member.Id);

                    //if (result)
                    //{
                    //role bayad taghir kone
                    User user = new UserBL().GetById(userId);
                    user.RoleCode = StaticRole.Seller.Id;
                    new UserBL().Update(user);

                    //log off user
                    string userCode = User.Identity.GetUserId();
                    if (!string.IsNullOrEmpty(userCode))
                        Session[userCode] = null;
                    AuthenticationManager.SignOut();

                    //login again
                    await SignInAsync(new AppUser()
                    {
                        Id = user.Id,
                        IsActive = user.IsActive,
                        RoleCode = user.RoleCode,
                        UserName = user.UserName,
                        Role = StaticRole.Seller
                    }, isPersistent: false);

                    return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
                    //}
                }
                return Json(JsonResultHelper.FailedResultWithMessage(storeRegisterResult.DbMessage.Message), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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

        public ActionResult GetStates()
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new StateBL().GetAllStatesCities()), JsonRequestBehavior.AllowGet);
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

        public ActionResult IsUsernameExsisted(string username)
        {
            try
            {
                bool result = new UserBL().IfUsernameExist(username);
                return Json(JsonResultHelper.SuccessResult(result), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => username),
                            Value = username
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
                            Name = HelperFunctionInBL.GetVariableName(() => username),
                            Value = username
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

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStore(StoreRegisterDataModel storeRegister)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }

            AppUser user = null;
            try
            {
                user = new AppUser { UserName = storeRegister.UserName, Role = StaticRole.Seller, RoleCode = StaticRole.Seller.Id };
                var result = await new ShopFinderUserManager().CreateUser(ref user, storeRegister.Password, storeRegister.Email);

                if (result == null || result.Succeeded == false)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                }


                storeRegister.ListCategoryCode = storeRegister.CategoryCodes.Split(',').Select(Int64.Parse).ToList();
                storeRegister.PhoneNumber = Convert.ToDecimal(user.UserName);
                var storeRegisterResult = new StoreBL().FullRegister(storeRegister, user.Id);
                if (storeRegisterResult.DbMessage.MessageType == MessageType.Success)
                {
                    Member member = new Member()
                    {
                        UserCode = user.Id,
                        CityCode = storeRegister.CityCode
                    };
                    new MemberBL().Insert(member);

                    //login user
                    await SignInAsync(user, isPersistent: false);
                    return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(user.Id))
                    new UserBL().DeleteById(user.Id);
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (user != null && string.IsNullOrEmpty(user.Id) == false) new UserBL().DeleteById(user.Id);

                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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
                    if (user != null && string.IsNullOrEmpty(user.Id) == false) new UserBL().DeleteById(user.Id);

                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

            try
            {
                AppUser user = await ShopFinderUserManager.FindAsync(model.UserName, model.Password);
                if(user==null)
                    return Json(JsonResultHelper.FailedResultWithMessage("نام کاربری یا رمز عبور نامعتبر است"), JsonRequestBehavior.AllowGet);

                user.Role = new RoleBL().SelectOne((int)user.RoleCode);
                if (user.Role != null && user.Role.Id > 0)
                {
                    if (!user.IsActive)
                        return Json(JsonResultHelper.FailedResultWithMessage("وضعیت کاربری شما غیر فعال میباشد"), JsonRequestBehavior.AllowGet);

                    await SignInAsync(user, model.RememberMe);

                    //now initialize MainSession
                    if (user.Role.Name == StaticString.Role_Seller)
                    {
                        StoreSessionDataModel store = new StoreBL().GetSummaryForSession(user.Id);
                        if (store == null)
                            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                        Session[user.Id] = new MainSession(store);
                    }
                    else if (user.Role.Name == StaticString.Role_Member)
                    {
                        Member member = new MemberBL().GetSummaryForSession(user.Id);
                        if (member == null)
                            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                        Session[user.Id] = new MainSession(member);
                    }
                    else if (user.Role.Name == StaticString.Role_Admin)
                    {
                        HBAdmin admin = new HBAdminBL().GetSummaryForSession(user.Id);
                        if (admin == null)
                            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                        Session[user.Id] = new MainSession(admin);
                    }
                    else if (user.Role.Name == StaticString.Role_SuperAdmin)
                    {
                        HBAdmin admin = new HBAdminBL().GetSummaryForSession(user.Id);
                        if (admin == null)
                            return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

                        Session[user.Id] = new MainSession(admin);
                    }
                    else
                    {
                        return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
                    }
                    return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonResultHelper.FailedResultWithMessage("نام کاربری یا رمز عبور نامعتبر است"), JsonRequestBehavior.AllowGet);
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


        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult LogOff()
        {
            string userCode = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userCode))
                Session[userCode] = null;
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [System.Web.Mvc.Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing && ShopFinderUserManager != null)
            {
                ShopFinderUserManager.Dispose();
                ShopFinderUserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [System.Web.Mvc.NonAction]
        [System.Web.Mvc.Authorize]
        public async Task SignInAsync(AppUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var identity = await ShopFinderUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            #region add information to user's role

            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name ?? string.Empty));

            #endregion


            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        #endregion
    }
}