using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Boundary.Areas.Admin.Models;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToPayments;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToPayments;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using NHibernate;

namespace Boundary.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticString.Role_Admin + "," + StaticString.Role_SuperAdmin)]
    public class PaymentToMemberController : BaseController
    {
        public ActionResult Index(int pageNumber = 1)
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                pageNumber = (pageNumber > 0) ? pageNumber : 1;
                ViewBag.PageNumber = pageNumber;
                List<HBPaymentToMemberViewModel> lst =
                    (from x in new HBPaymentToMemberBL().GetAll_OrderByLast(pageNumber)
                     select new HBPaymentToMemberViewModel()
                     {
                         Date = x.Date,
                         AdminName = new HBAdminBL().SelectOne(x.AdminCode).Name,
                         Money = x.Money,
                         MemberName = new MemberBL().SelectOne(x.MemberCode).Name,
                         TrackingCode = x.TrackingCode,   
                         Id = x.Id,
                         MemberCode = x.MemberCode
                     }).ToList();

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
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
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

        public ActionResult Create()
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
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

        public ActionResult PostCreate(HBPaymentToMember hbPaymentToMember)
        {
            ISession session = null;
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                DataModel.Entities.Member member = new MemberBL().SelectOne(hbPaymentToMember.MemberCode);
                if (member.Balance < hbPaymentToMember.Money)
                    return Json(JsonResultHelper.FailedResultWithMessage("موجودی کاربر در هوجی بوجی کمتر از مقدار مورد نظر شما می باشد"), JsonRequestBehavior.AllowGet);

                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                long insertResult = new HBPaymentToMemberBL().InsertWhitOutCommitTransaction(new HBPaymentToMember()
                {
                    Date = PersianDateTime.Now.Date.ToInt(),
                    AdminCode = checkSession.MainSession.Admin.Id,
                    TrackingCode = hbPaymentToMember.TrackingCode,
                    Money = hbPaymentToMember.Money,
                    MemberCode = hbPaymentToMember.MemberCode,
                }, session);

                if (insertResult > 0)
                {
                    //موجودی کاربرو ویرایش کنیم
                    member.Balance -= hbPaymentToMember.Money;
                    bool updateResult = new MemberBL().UpdateWhitOutCommitTransaction(member, session);
                    
                    if (updateResult)
                    {
                        session.Transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }

                session.Transaction.Rollback();
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToMember),
                            Value = JObject.FromObject(hbPaymentToMember).ToString()
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
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                       new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToMember),
                            Value = JObject.FromObject(hbPaymentToMember).ToString()
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

        public ActionResult Edit(long id)
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                HBPaymentToMember hbPaymentToMember = new HBPaymentToMemberBL().SelectOne(id);
                return View(hbPaymentToMember);
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

        public ActionResult PostEdit(HBPaymentToMember hbPaymentToMember)
        {
            ISession session = null;
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                HBPaymentToMember item = new HBPaymentToMemberBL().SelectOne(hbPaymentToMember.Id);
                DataModel.Entities.Member member = new MemberBL().SelectOne(hbPaymentToMember.MemberCode);

                int tafazoleMeghdareJadidAzGhabli = hbPaymentToMember.Money - item.Money;

                //اگه بزرگتر از صفره باید بررسی بشه
                if (tafazoleMeghdareJadidAzGhabli > 0)
                {
                    if (member.Balance < tafazoleMeghdareJadidAzGhabli)
                        return Json(JsonResultHelper.FailedResultWithMessage("موجودی کاربر در هوجی بوجی کمتر از مقدار مورد نظر شما می باشد"), JsonRequestBehavior.AllowGet);
                }
                
                item.AdminCode = checkSession.MainSession.Admin.Id;
                item.MemberCode = hbPaymentToMember.MemberCode;
                item.Date = PersianDateTime.Now.Date.ToInt();
                item.Money = hbPaymentToMember.Money;
                item.TrackingCode = hbPaymentToMember.TrackingCode;

                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                bool result = new HBPaymentToMemberBL().UpdateWhitOutCommitTransaction(item,session);
                if (result)
                {
                    //موجودی کاربرو ویرایش کنیم
                    member.Balance -= tafazoleMeghdareJadidAzGhabli;
                    bool updateResult = new MemberBL().UpdateWhitOutCommitTransaction(member, session);

                    if (updateResult)
                    {
                        session.Transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }

                session.Transaction.Rollback();
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToMember),
                            Value = JObject.FromObject(hbPaymentToMember).ToString()
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
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                       new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => hbPaymentToMember),
                            Value = JObject.FromObject(hbPaymentToMember).ToString()
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

        public ActionResult Delete(long id)
        {
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                HBPaymentToMember hbPaymentToMember = new HBPaymentToMemberBL().SelectOne(id);
                HBPaymentToMemberViewModel model = new HBPaymentToMemberViewModel()
                {
                    Date = hbPaymentToMember.Date,
                    AdminName = new HBAdminBL().SelectOne(hbPaymentToMember.AdminCode).Name,
                    Money = hbPaymentToMember.Money,
                    TrackingCode = hbPaymentToMember.TrackingCode,
                    MemberCode = hbPaymentToMember.MemberCode,
                    Id = hbPaymentToMember.Id,
                    MemberName = new MemberBL().SelectOne(hbPaymentToMember.MemberCode).Name,
                };
                return View(model);
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

        public ActionResult PostDelete(long id)
        {
            ISession session = null;
            try
            {
                CheckSessionDataModel checkSession = CheckAdminSession();
                if (!checkSession.IsSuccess)
                {
                    if (checkSession.Message == StaticString.Message_WrongAccess)
                        return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                    if (checkSession.Message == StaticString.Message_UnSuccessFull)
                        return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                    return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
                }

                HBPaymentToMember item=new HBPaymentToMemberBL().SelectOne(id);
                DataModel.Entities.Member member=new MemberBL().SelectOne(item.MemberCode);

                session = NHibernateConfiguration.OpenSession();
                session.BeginTransaction();

                bool result = new HBPaymentToMemberBL().DeleteWhitOutCommitTransaction(id, session);
                if (result)
                {
                    member.Balance += item.Money;
                    bool updateResult = new MemberBL().UpdateWhitOutCommitTransaction(member, session);

                    if (updateResult)
                    {
                        session.Transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }

                session.Transaction.Rollback();
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = JObject.FromObject(id).ToString()
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
                    if (session != null)
                        session.Transaction.Rollback();
                    session = null;
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                       new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => id),
                            Value = JObject.FromObject(id).ToString()
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