﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Entities.RelatedToStore;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Api
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        public ShopFinderUserManager ShopFinderUserManager { get; private set; }

        public AccountController() : this(new ShopFinderUserManager()) { }

        public AccountController(ShopFinderUserManager customUserManager)
        {
            ShopFinderUserManager = customUserManager;
        }


        [Route("RegisterMember")]
        public async Task<IHttpActionResult> RegisterMember([FromBody]RegisterMemberDataModel model,[FromUri]string shopname)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            try
            {
                AppUser user = new AppUser { UserName = model.UserName, Role = StaticRole.Member, RoleCode = StaticRole.Member.Id };
                var result = await ShopFinderUserManager.CreateUser(ref user, model.Password, (model.MemberInfo != null) ? model.MemberInfo.Email : string.Empty);

                if (result != null && result.Succeeded == false)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage((result.Errors != null && result.Errors.Any()) ? result.Errors.SingleOrDefault() : string.Empty));
                }
                if (result == null)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                Member member = new Member()
                {
                    UserCode = user.Id
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

                    return Json(JsonResultHelper.SuccessResult());
                }
                    
                new UserBL().DeleteById(user.Id);
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
                            Name = HelperFunctionInBL.GetVariableName(() => model),
                            Value = JObject.FromObject(model).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => model),
                            Value = JObject.FromObject(model).ToString()
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

        [Authorize(Roles = StaticString.Role_Member)]
        [HttpPost]
        [Route("UpgradeToSeller")]
        public IHttpActionResult UpgradeMembershipFromMemberToSeller(StoreRegisterForUpgradeMemberToSallerDataModel storeRegister)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            try
            {
                string userId = this.RequestContext.Principal.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Json(JsonResultHelper.FailedResultOfWrongAccess());

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
                    ListCategoryCode = storeRegister.ListCategoryCode,
                    StoreTypeCode = storeRegister.StoreTypeCode,
                    Website = storeRegister.Website,
                    SallerName = storeRegister.SallerName,
                    NationalCode = storeRegister.NationalCode,
                    SallerComments = storeRegister.SallerComments,
                    IsMale = storeRegister.IsMale,
                }, userId);

                //age movafagh sabt shod
                if (storeRegisterResult.DbMessage.MessageType == MessageType.Success)
                {
                    ////az table Customer delete beshe   //todo: چون اگه خرید کرده باشه اطلاعات خریدشو میخوام و همچنین شاید در آینده خردی کنه
                    //DataModel.Entities.Member member = new MemberBL().GetSummaryForSession(userId);
                    //bool result = false;
                    //if (member != null)
                    //    result = new MemberBL().Delete(member.Id);

                    //if (result)
                    //{
                        //role bayad taghir kone
                        User user = new UserBL().GetById(userId);
                        user.RoleCode = StaticRole.Seller.Id;
                        new UserBL().Update(user);
                        return Json(JsonResultHelper.SuccessResult(storeRegisterResult.DbMessage.Message));
                    //}
                }

                return Json(JsonResultHelper.FailedResultWithMessage(storeRegisterResult.DbMessage.Message));
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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
        [Route("CheackUserName")]
        public IHttpActionResult CheackUserName(string username)
        {
            try
            {
                bool result = new UserBL().IfUsernameExist(username);
                if (!result)
                    return Json(JsonResultHelper.SuccessResult("نام کاربری مجاز است"));
                return Json(JsonResultHelper.FailedResultWithMessage("نام کاربری از قبل موجود میباشد"));
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
                            Name = HelperFunctionInBL.GetVariableName(() => username),
                            Value = username
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
        [Route("getStatesAndCategories")]
        public IHttpActionResult GetStateAndCitiesAndCategories()
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new StatesAndCategoriesViewModel()
                {
                    Categories = new CategoryBL().GetFirstLevel(),
                    States = new StateBL().GetAllStatesCities()
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
        [Route("AddStore")]
        public async Task<IHttpActionResult> AddStore(StoreRegisterDataModel storeRegister)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            AppUser user = null;
            try
            {
                user = new AppUser { UserName = storeRegister.UserName, Role = StaticRole.Seller, RoleCode = StaticRole.Seller.Id };
                var result = await new ShopFinderUserManager().CreateUser(ref user, storeRegister.Password,storeRegister.Email);

                if (result == null || !result.Succeeded)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }

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

                    return Json(JsonResultHelper.SuccessResult(storeRegisterResult.Obj.Id.ToString()));
                }
                    
                if (string.IsNullOrEmpty(user.Id))
                    new UserBL().DeleteById(user.Id);
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (user != null && string.IsNullOrEmpty(user.Id) == false)
                        new UserBL().DeleteById(user.Id);
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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
                    if (user != null && string.IsNullOrEmpty(user.Id) == false)
                        new UserBL().DeleteById(user.Id);
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeRegister),
                            Value = JObject.FromObject(storeRegister).ToString()
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


        [Authorize]
        [Route("getUserProfilePicture")]
        public IHttpActionResult GetUserProfilePicture()
        {
            try
            {
                string userCode = this.RequestContext.Principal.Identity.GetUserId();
                if (string.IsNullOrEmpty(userCode))
                    return null;
                User user = new UserBL().GetById(userCode);
                if (user == null)
                    return null;
                string imgAddress = string.Empty;

                Role userRole = new RoleBL().SelectOne((int)user.RoleCode);
                //now for return user's profile picture
                switch (userRole.Name)
                {
                    case StaticString.Role_Admin:
                        {
                            //imgAddress = new AdminBL().GetPicAddressByUserCode(userCode);
                            break;
                        }
                    case StaticString.Role_Member:
                        {
                            imgAddress = new MemberBL().GetPicAddressByUserCode(userCode);
                            break;
                        }
                    case StaticString.Role_Seller:
                        {
                            imgAddress = new SellerBL().GetPhotoAddresByUserCode(userCode);
                            break;
                        }
                }

                if (string.IsNullOrEmpty(imgAddress))
                    imgAddress = string.Empty;

                return Json(JsonResultHelper.SuccessResult(imgAddress));
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

        //private IAuthenticationManager Authentication
        //{
        //    get { return Request.GetOwinContext().Authentication; }
        //}

        //#region External Login
        ////GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    if (error != null)
        //    {
        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }
        //    AppUser user = await ShopFinderUserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
        //        externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        ClaimsIdentity oAuthIdentity = await ShopFinderUserManager.CreateIdentityAsync(user,
        //            OAuthDefaults.AuthenticationType);
        //        ClaimsIdentity cookieIdentity = await ShopFinderUserManager.CreateIdentityAsync(user,
        //            CookieAuthenticationDefaults.AuthenticationType);
        //        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        //        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
        //    }
        //    else
        //    {
        //        IEnumerable<Claim> claims = externalLogin.GetClaims();
        //        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        //        Authentication.SignIn(identity);
        //    }

        //    return Ok();
        //}

        ////private IHttpActionResult GetErrorResult(IdentityResult result)
        ////{
        ////    if (result == null)
        ////    {
        ////        return InternalServerError();
        ////    }

        ////    if (!result.Succeeded)
        ////    {
        ////        if (result.Errors != null)
        ////        {
        ////            foreach (string error in result.Errors)
        ////            {
        ////                ModelState.AddModelError("", error);
        ////            }
        ////        }

        ////        if (ModelState.IsValid)
        ////        {
        ////            // No ModelState errors are available to send, so just return an empty BadRequest.
        ////            return BadRequest();
        ////        }

        ////        return BadRequest(ModelState);
        ////    }

        ////    return null;
        ////}

        //private class ExternalLoginData
        //{
        //    public string LoginProvider { get; set; }
        //    public string ProviderKey { get; set; }
        //    public string UserName { get; set; }

        //    public IList<Claim> GetClaims()
        //    {
        //        IList<Claim> claims = new List<Claim>();
        //        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        //        if (UserName != null)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        //        }

        //        return claims;
        //    }

        //    public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        //    {
        //        if (identity == null)
        //        {
        //            return null;
        //        }

        //        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        //        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
        //            || String.IsNullOrEmpty(providerKeyClaim.Value))
        //        {
        //            return null;
        //        }

        //        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        //        {
        //            return null;
        //        }

        //        return new ExternalLoginData
        //        {
        //            LoginProvider = providerKeyClaim.Issuer,
        //            ProviderKey = providerKeyClaim.Value,
        //            UserName = identity.FindFirstValue(ClaimTypes.Name)
        //        };
        //    }
        //}
        //#endregion External Login
    }
}