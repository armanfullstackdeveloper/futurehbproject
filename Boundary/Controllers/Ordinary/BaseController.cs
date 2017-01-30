using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    /// <summary>
    /// کلاس سشن برای انواع کاربران
    /// </summary>
    public class MainSession
    {
        public MainSession(HBAdmin admin)
        {
            Admin = admin;
        }
        public MainSession(StoreSessionDataModel store)
        {
            Store = store;
        }
        public MainSession(Member member)
        {
            Member = member;
        }
        public HBAdmin Admin { get; set; }
        public StoreSessionDataModel Store { get; private set; }
        public Member Member { get; private set; }
        //public Role Role { get; set; } 
    }

    /// <summary>
    /// مدل مورد استفاده برای تشخیص معتبر بودن کاربران
    ///  با توجه به سشن و نام کاربریشان
    /// </summary>
    public class CheckSessionDataModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public MainSession MainSession { get; set; }
    }



    /// <summary>
    /// کنترلر مادر 
    /// برای مدیریت سشن در دیگر کنترلرها
    /// </summary>
    public class BaseController : Controller
    {
        [Authorize]
        [NonAction]
        public CheckSessionDataModel CheckAdminSession()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    HBAdmin findedAdmin;
                    if (Session[userId] != null)
                    {
                        MainSession mainSession = (MainSession)Session[userId];
                        findedAdmin = mainSession.Admin;
                        if (findedAdmin == null || findedAdmin.Id <= 0)
                            return new CheckSessionDataModel()
                            {
                                IsSuccess = false,
                                MainSession = null,
                                Message = StaticString.Message_WrongAccess
                            };
                        return new CheckSessionDataModel()
                        {
                            IsSuccess = true,
                            MainSession = mainSession,
                            Message = StaticString.Message_SuccessFull
                        };
                    }
                    //hala age session nadaram baram sakhte beshe
                    findedAdmin = new HBAdminBL().GetSummaryForSession(userId);
                    //age admin nistam baram sakhte nashe
                    if (findedAdmin == null || findedAdmin.Id <= 0)
                    {
                        return new CheckSessionDataModel()
                        {
                            IsSuccess = false,
                            MainSession = null,
                            Message = StaticString.Message_WrongAccess
                        };
                    }
                    //age admin hastam baram sakhte she
                    Session[userId] = new MainSession(findedAdmin);
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = true,
                        MainSession = new MainSession(findedAdmin),
                        Message = StaticString.Message_SuccessFull
                    };
                }
                //age aslan login nabasham
                return new CheckSessionDataModel()
                {
                    IsSuccess = false,
                    MainSession = null,
                    Message = StaticString.Message_WrongAccess
                };
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
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
                catch (Exception)
                {
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
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
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
                catch (Exception)
                {
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
            }
        }

        [Authorize]
        [NonAction]
        public CheckSessionDataModel CheckSallerSession(bool refresh=false)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                //age user login bod
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    StoreSessionDataModel findedStore;
                    //اگه سشن پر بود و رفرشم نمیخواست
                    if (Session[userId] != null && refresh==false)
                    {
                        MainSession mainSession = (MainSession)Session[userId];
                        findedStore = mainSession.Store;
                        if (findedStore == null || findedStore.StoreCode <= 0)
                            return new CheckSessionDataModel()
                            {
                                IsSuccess = false,
                                Message = StaticString.Message_WrongAccess,
                                MainSession = null
                            };
                        return new CheckSessionDataModel()
                        {
                            IsSuccess = true,
                            Message = StaticString.Message_SuccessFull,
                            MainSession = mainSession
                        };
                    }
                    findedStore = new StoreBL().GetSummaryForSession(userId);
                    if (findedStore == null || findedStore.StoreCode <= 0)
                        return new CheckSessionDataModel()
                        {
                            IsSuccess = false,
                            Message = StaticString.Message_WrongAccess,
                            MainSession = null
                        };
                    Session[userId] = new MainSession(findedStore);
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = true,
                        Message = StaticString.Message_SuccessFull,
                        MainSession = new MainSession(findedStore)
                    };
                }
                return new CheckSessionDataModel()
                {
                    IsSuccess = false,
                    Message = StaticString.Message_WrongAccess,
                    MainSession = null
                };
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp1, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
                catch (Exception)
                {
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
            }
            catch (Exception exp3)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
                catch (Exception)
                {
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
            }
        }


        [Authorize]
        [NonAction]
        public CheckSessionDataModel CheckMembererSession() 
        {
            try
            {
                string userId = User.Identity.GetUserId();
                //age user login bod
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    Member findedMember;
                    if (Session[userId] != null)
                    {
                        MainSession mainSession = (MainSession)Session[userId];
                        findedMember = mainSession.Member;
                        if (findedMember == null || findedMember.Id <= 0)
                            return new CheckSessionDataModel()
                            {
                                IsSuccess = false,
                                Message = StaticString.Message_WrongAccess,
                                MainSession = null
                            };
                        return new CheckSessionDataModel()
                        {
                            IsSuccess = true,
                            Message = StaticString.Message_SuccessFull,
                            MainSession = mainSession
                        };
                    }
                    findedMember = new MemberBL().GetSummaryForSession(userId);
                    if (findedMember == null || findedMember.Id <= 0)
                        return new CheckSessionDataModel()
                        {
                            IsSuccess = false,
                            Message = StaticString.Message_WrongAccess,
                            MainSession = null
                        };
                    Session[userId] = new MainSession(findedMember);
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = true,
                        Message = StaticString.Message_SuccessFull,
                        MainSession = new MainSession(findedMember)
                    };
                }
                return new CheckSessionDataModel()
                {
                    IsSuccess = false,
                    Message = StaticString.Message_WrongAccess,
                    MainSession = null
                };
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
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
                catch (Exception)
                {
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
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
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
                catch (Exception)
                {
                    return new CheckSessionDataModel()
                    {
                        IsSuccess = false,
                        MainSession = null,
                        Message = StaticString.Message_UnSuccessFull
                    };
                }
            }
        }

    }
}