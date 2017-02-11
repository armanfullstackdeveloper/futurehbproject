using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.Sms;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Entities.Sms;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Api
{
    [RoutePrefix("api/Sms")]
    public class SmsController : ApiController
    {
        [HttpGet]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(string phoneNumber)
        {
            if (phoneNumber.Length != 11 || phoneNumber.StartsWith("09") == false)
                return Json(JsonResultHelper.FailedResultOfInvalidInputs());
            int verificationCode = new Random().Next(1000, 9000);
            try
            {
                //check konam az ghabl sabtenam karde ya na
                User user = new UserBL().GetByUserName(phoneNumber);
                if (user!=null && user.RoleCode!=ERole.NotRegister)
                    return Json(JsonResultHelper.FailedResultWithMessage("تلفن همراه وارد شده، از قبل موجود می باشد"));
                //check konam bish az 3 bar dar roz nabode bashe baraye ip va shomare khas
                int count = new SmsBL().TodayAttempt(Convert.ToInt64(phoneNumber.Remove(0, 1)),ESmsType.Registeration);
                if (count >= Convert.ToInt32(WebConfigurationManager.AppSettings["mellipayamak_maxAttemptPerDay"]))
                    return Json(JsonResultHelper.FailedResultWithMessage("عملیات با خطا مواجه شد"));

                SendSmsDataModel smsDataModel = new SendSmsDataModel
                {
                    From = WebConfigurationManager.AppSettings["mellipayamak_number"],
                    Username = WebConfigurationManager.AppSettings["mellipayamak_username"],
                    PassWord = WebConfigurationManager.AppSettings["mellipayamak_password"],
                    To = phoneNumber.Remove(0, 1),
                    Text = SmsHelper.RegisterMessage(verificationCode)
                };

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("http://rest.payamak-panel.com/api/SendSMS/SendSMS", smsDataModel);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                SendSmsResultDataModel result = await response.Content.ReadAsAsync<SendSmsResultDataModel>();
                long returnValue = Convert.ToInt64(result.Value);
                var exists = returnValue <= 255 && Enum.IsDefined(typeof(ESendSmsResponseStatus), (byte)returnValue);
                Sms sms = new Sms
                {
                    Text = verificationCode.ToString(),
                    Reciver = Convert.ToInt64(smsDataModel.To),
                    CreationDate = PersianDateTime.Now.Date.ToInt(),
                    CreationTime = PersianDateTime.Now.TimeOfDay.ToShort(),
                    SmsType = ESmsType.Registeration
                };
                if (exists)
                {
                    //something happen
                    sms.ResponseStatus = (ESendSmsResponseStatus)returnValue;
                    new SmsBL().Insert(sms);
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
                //everything ok
                sms.ResponseStatus = ESendSmsResponseStatus.Successfull;
                sms.TrackingCode = Convert.ToInt64(result.Value);
                new SmsBL().Insert(sms);
                return Json(JsonResultHelper.SuccessResult());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => phoneNumber),
                            Value = phoneNumber
                        }
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                       new ActionInputViewModel
                       {
                            Name = HelperFunctionInBL.GetVariableName(() => phoneNumber),
                            Value = phoneNumber
                        }
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
        [Route("ForgetPassword")]
        public async Task<IHttpActionResult> ForgetPassword(string phoneNumber)
        {
            if (phoneNumber.Length != 11 || phoneNumber.StartsWith("09") == false)
                return Json(JsonResultHelper.FailedResultOfInvalidInputs());
            int newPass = new Random().Next(100000, 900000);
            try
            {
                //check konam az ghabl sabtenam karde ya na
                User user = new UserBL().GetByUserName(phoneNumber);
                if (user == null)
                    return Json(JsonResultHelper.FailedResultWithMessage("تلفن همراه وارد شده، موجود نیست"));
                //check konam bish az 3 bar dar roz nabode bashe baraye ip va shomare khas
                int count = new SmsBL().TodayAttempt(Convert.ToInt64(phoneNumber.Remove(0, 1)),ESmsType.ForgetPassword);
                if (count >= Convert.ToInt32(WebConfigurationManager.AppSettings["mellipayamak_maxAttemptPerDay"]))
                    return Json(JsonResultHelper.FailedResultWithMessage());

                user.Password = HelperFunction.GetMd5Hash(newPass.ToString());
                if(new UserBL().Update(user).DbMessage.MessageType!=MessageType.Success)
                    return Json(JsonResultHelper.FailedResultWithMessage());

                SendSmsDataModel smsDataModel = new SendSmsDataModel
                {
                    From = WebConfigurationManager.AppSettings["mellipayamak_number"],
                    Username = WebConfigurationManager.AppSettings["mellipayamak_username"],
                    PassWord = WebConfigurationManager.AppSettings["mellipayamak_password"],
                    To = phoneNumber.Remove(0, 1),
                    Text = SmsHelper.NewPass(newPass)
                };

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("http://rest.payamak-panel.com/api/SendSMS/SendSMS", smsDataModel);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                SendSmsResultDataModel result = await response.Content.ReadAsAsync<SendSmsResultDataModel>();
                long returnValue = Convert.ToInt64(result.Value);
                var exists = returnValue <= 255 && Enum.IsDefined(typeof(ESendSmsResponseStatus), (byte)returnValue);
                Sms sms = new Sms
                {
                    Text = newPass.ToString(),
                    Reciver = Convert.ToInt64(smsDataModel.To),
                    CreationDate = PersianDateTime.Now.Date.ToInt(),
                    CreationTime = PersianDateTime.Now.TimeOfDay.ToShort(),
                    SmsType = ESmsType.Registeration
                };
                if (exists)
                {
                    //something happen
                    sms.ResponseStatus = (ESendSmsResponseStatus)returnValue;
                    new SmsBL().Insert(sms);
                    return Json(JsonResultHelper.FailedResultWithMessage());
                }
                //everything ok
                sms.ResponseStatus = ESendSmsResponseStatus.Successfull;
                sms.TrackingCode = Convert.ToInt64(result.Value);
                new SmsBL().Insert(sms);
                return Json(JsonResultHelper.SuccessResult());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => phoneNumber),
                            Value = phoneNumber
                        }
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
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                       new ActionInputViewModel
                       {
                            Name = HelperFunctionInBL.GetVariableName(() => phoneNumber),
                            Value = phoneNumber
                        }
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
