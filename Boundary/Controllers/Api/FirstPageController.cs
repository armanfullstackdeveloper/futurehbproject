﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Boundary.Helper;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.FirstPageBL;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/firstPage")]
    public class FirstPageController : ApiController
    {
        [System.Web.Http.HttpGet]
        [OutputCache(Duration = 60)]
        [System.Web.Http.Route("GetActiveSlider")]
        public IHttpActionResult GetActiveSlider()
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new FirstPage_SliderBL().GetActiveSlider()));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }

        [OutputCache(Duration = 60)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetActiveAdvertise")]
        public IHttpActionResult GetActiveAdvertise()
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new FirstPage_AdvertiseBL().GetActiveAdvertise()));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>();
                    long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
                    return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
                }
                catch (Exception)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
            }
        }

        [OutputCache(Duration = 60)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetNewestStore")]
        public IHttpActionResult GetNewestStore(long? cityCode = null, int? pageNumber = null, int? rowspPage = null)
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new StoreBL().GetNewest(cityCode, pageNumber, rowspPage)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => cityCode),
                            Value = cityCode.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                            Value = rowspPage.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => cityCode),
                            Value = cityCode.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                            Value = rowspPage.ToString()
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

        /// <summary>
        /// سرچ بالای سایت که تمام نتایج اعم از کالاها و فروشگاها رو جستجو میکند
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TopSearchSummeray")]
        public IHttpActionResult TopSearchSummeray(string name)
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new ProductBL().OveralSearchSummary(name,
                    pageNumberForStore: 1, pageNumberForProduct: 1, rowspPageForStore: 10, rowspPageForProduct: 10)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => name),
                            Value = name.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => name),
                            Value = name.ToString()
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


        /// <summary>
        /// این اکشن برای وقتی است در سرچ بالای سایت عبارتی جستجو میشود
        /// و روی بیشتر کلیک میشود تا بقیه نتایج هم مشاهده شوند
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowspPage"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TopSearchForProduct")]
        public IHttpActionResult TopSearchForProduct(string name, int? pageNumber, int? rowspPage)
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new ProductBL().OveralSearchForProducts(name, pageNumber, rowspPage)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                      new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => name),
                            Value = name.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                            Value = rowspPage.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => name),
                            Value = name
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                            Value = rowspPage.ToString()
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

        /// <summary>
        /// این اکشن برای وقتی است در سرچ بالای سایت عبارتی جستجو میشود
        /// و روی بیشتر کلیک میشود تا بقیه نتایج هم مشاهده شوند
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowspPage"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("TopSearchForStore")]
        public IHttpActionResult TopSearchForStore(string name, int? pageNumber, int? rowspPage)
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new StoreBL().OveralSearchForStores(name, pageNumber, rowspPage)));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                      new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => name),
                            Value = name
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                            Value = rowspPage.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => name),
                            Value = name.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumber),
                            Value = pageNumber.ToString()
                        },                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => rowspPage),
                            Value = rowspPage.ToString()
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

        [System.Web.Http.Route("ContactUs")]
        public IHttpActionResult ContactUs(ContactUsDataModel contactUs)
        {
            try
            {
                string strBody = string.Empty;
                strBody += string.Format("<b>Full Name</b>: {0}<br />", contactUs.FullName);
                strBody += string.Format("<b>E-Mail</b>: <a href='mailto:{0}'>{0}</a><br />", contactUs.Email);
                strBody += string.Format("<b>Subject</b>: {0}<br />", contactUs.Subject);
                strBody += string.Format("<b>Description</b>: {0}<br />", contactUs.Description.Replace("\n", "<br />"));

                MailAddress oMailAddress = new MailAddress
                    (
                    contactUs.Email,            //email
                    contactUs.FullName,         //display name
                    System.Text.Encoding.UTF8
                    );

                MailMessage oMailMessage = new MailMessage { From = oMailAddress, Sender = oMailAddress };

                oMailMessage.To.Clear();   //moshakhas mikone ke in email gharare be che kasi ersal shavad
                oMailMessage.CC.Clear();   //nafarate badi ke gharare in email ro dashte bashan
                oMailMessage.Bcc.Clear();  //age email baraye shakhi ke dakhele CC hast nemitone bebine dige baraye che kasai ersal shode
                oMailMessage.ReplyToList.Clear();
                oMailMessage.Attachments.Clear();

                //oMailMessage.ReplyToList.Add(oMailAddress);

                oMailMessage.To.Add(new MailAddress
                        (
                            "info@hoojibooji.com",
                            "HoojiBooji",
                            System.Text.Encoding.UTF8
                        ));

                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                oMailMessage.Body = strBody;
                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;


                //یه پیشوند یا امضا قبل از سابجکت قرار میدهیم تا فیلترینگ
                //ایمیل ها ساده شوند
                oMailMessage.Subject = contactUs.Subject;

                oMailMessage.IsBodyHtml = true;
                oMailMessage.Priority = MailPriority.Normal;
                oMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;

                SmtpClient oSmtpClient = new SmtpClient { Timeout = 100000, EnableSsl = false };
                oSmtpClient.Send(oMailMessage);

                return Json(JsonResultHelper.SuccessResult());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => contactUs),
                            Value = JObject.FromObject(contactUs).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => contactUs),
                            Value = JObject.FromObject(contactUs).ToString()
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

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ForgetPassword")]
        public IHttpActionResult ForgetPassword(string email) 
        {
            try
            {
                List<User> users = new UserBL().GetByEmail(email);
                if (users == null || users.Count == 0 || users.Count > 1)
                {
                    return Json(JsonResultHelper.FailedResultWithMessage("ایمیل نامعتبر است"));
                }

                string newPass = Guid.NewGuid().ToString();
                newPass = newPass.Substring(0, 10);
                string hashPass = HelperFunction.GetMd5Hash(newPass);
                

                User user = users.FirstOrDefault();
                user.Password = hashPass;
                new UserBL().Update(user);

                string strBody = string.Empty;
                strBody += string.Format("<b>E-Mail</b>: <a href='mailto:{0}'>{0}</a><br />", "info@hoojibooji.com");
                strBody += string.Format("<b>Description</b> your'e new pass is : <b>{0}</b>", newPass);

                MailAddress oMailAddress = new MailAddress
                    (
                    "info@hoojibooji.com",            //email
                    " هوجی بوجی ",         //display name
                    System.Text.Encoding.UTF8
                    );

                MailMessage oMailMessage = new MailMessage { From = oMailAddress, Sender = oMailAddress };

                oMailMessage.To.Clear();   //moshakhas mikone ke in email gharare be che kasi ersal shavad
                oMailMessage.CC.Clear();   //nafarate badi ke gharare in email ro dashte bashan
                oMailMessage.Bcc.Clear();  //age email baraye shakhi ke dakhele CC hast nemitone bebine dige baraye che kasai ersal shode
                oMailMessage.ReplyToList.Clear();
                oMailMessage.Attachments.Clear();

                oMailMessage.To.Add(new MailAddress
                        (
                            email,
                            user.UserName,
                            System.Text.Encoding.UTF8
                        ));

                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                oMailMessage.Body = strBody;
                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;


                //یه پیشوند یا امضا قبل از سابجکت قرار میدهیم تا فیلترینگ
                //ایمیل ها ساده شوند
                oMailMessage.Subject = "فراموشی رمز عبور";

                oMailMessage.IsBodyHtml = true;
                oMailMessage.Priority = MailPriority.Normal;
                oMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;

                SmtpClient oSmtpClient = new SmtpClient { Timeout = 100000, EnableSsl = false };
                oSmtpClient.Send(oMailMessage);

                return Json(JsonResultHelper.SuccessResult());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => email),
                            Value = email
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
                            Name = HelperFunctionInBL.GetVariableName(() => email),
                            Value = email
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

        [OutputCache(Duration = 60)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("getMenue")]
        public IHttpActionResult GetMenu()
        {
            try
            {
                return Json(JsonResultHelper.SuccessResult(new CategoryBL().GetAll(withImage:true)));
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
    }
}