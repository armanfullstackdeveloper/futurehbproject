using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using Boundary.com.arianpal.merchant;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using BusinessLogic.BussinesLogics.RelatedToPayments;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToPayments;
using DataModel.Enums;
using DataModel.Models.ViewModel;
using Fluentx.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Ordinary
{
    public class PaymentRequestController : BaseController
    {
        /// <summary>
        /// تائید نهایی و رفتن به درگاه پرداخت
        /// </summary>
        /// <param name="paymentRequestCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Pay(long paymentRequestCode)
        {
            try
            {
                PaymentRequest paymentRequest = new PaymentRequestBL().SelectOne(paymentRequestCode);
                var orders = new OrderBL().GetOrdersByPaymentRequestCode(paymentRequestCode);
                if (orders == null || orders.Count == 0)
                    return Json(JsonResultHelper.FailedResultWithMessage("خطا در دریافت اطلاعات خرید"), JsonRequestBehavior.AllowGet);

                int finalPriceToPay = orders.Sum(o => o.OverallPayment);

                //خب حالا طبق درگاه انتخابی پیش میریم
                switch (paymentRequest.PaymentGateway)
                {
                    case EPaymentGateway.Arianpal:
                        {
                            AppSettingsReader appRead = new AppSettingsReader();
                            string redirectAddress = appRead.GetValue("Arianpal_RedirectAddress", typeof(string)).ToString();

                            WebService ws = new WebService();
                            PaymentRequestResult requestResult =
                                ws.RequestPayment(WebConfigurationManager.AppSettings["MerchantID"],
                                    WebConfigurationManager.AppSettings["Password"], finalPriceToPay, string.Empty, string.Empty,
                                    string.Empty, string.Empty, paymentRequest.Id.ToString(), redirectAddress);

                            if (requestResult.ResultStatus == ResultValues.Succeed)
                            {
                                paymentRequest.PaymentRequestStatusCode = (byte)ResultValues.Succeed;
                                bool updateResult = new PaymentRequestBL().Update(paymentRequest);
                                if (!updateResult)
                                {
                                    return Json(JsonResultHelper.FailedResultWithMessage(HelperFunction.PaymentRequestResultMessage((byte)requestResult.ResultStatus)), JsonRequestBehavior.AllowGet);
                                }

                                //آدرس مورد نیاز برای وصل شدن به درگاه
                                //todo: redirect konam
                                return Json(JsonResultHelper.SuccessResult(requestResult.PaymentPath), JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                if (finalPriceToPay < StaticNemberic.MinimumTarakoneshValue)
                                {
                                    //حواسم باشه طبق دیتابیس تنظیم شده این
                                    paymentRequest.PaymentRequestStatusCode = (byte)EPaymentRequestResultValues.MinimumTransactionFailed;
                                }
                                else
                                {
                                    paymentRequest.PaymentRequestStatusCode = (byte)requestResult.ResultStatus;
                                }
                                new PaymentRequestBL().Update(paymentRequest);
                                return Json(JsonResultHelper.FailedResultWithMessage(HelperFunction.PaymentRequestResultMessage((byte)requestResult.ResultStatus)), JsonRequestBehavior.AllowGet);
                            }
                        }
                    case EPaymentGateway.Pasargad:
                    default:
                        {
                            AppSettingsReader appRead = new AppSettingsReader();
                            //دریافت تنظیمات از web.config
                            string merchantCode = appRead.GetValue("PasargadBank_MerchantCode", typeof(string)).ToString();
                            string terminalCode = appRead.GetValue("PasargadBank_TerminalCode", typeof(string)).ToString();
                            string redirectAddress = appRead.GetValue("PasargadBank_RedirectAddress", typeof(string)).ToString();
                            string privateKey = appRead.GetValue("PasargadBank_PrivateKey", typeof(string)).ToString();

                            //تاریخ فاکتور و زمان اجرای عملیات از سیستم گرفته می شود
                            //شما می توانید تاریخ فاکتور را به صورت دستی وارد نمایید 
                            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            string invoiceDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            string ActionIs = "1003"; //نوع تراکنش برای خرید

                            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                            rsa.FromXmlString(privateKey);

                            finalPriceToPay = finalPriceToPay * 10; //convert to rial
                            string data = "#" + merchantCode + "#" + terminalCode + "#" + paymentRequestCode +
                                "#" + invoiceDate + "#" + finalPriceToPay + "#" + redirectAddress + "#" + ActionIs + "#" + timeStamp + "#";

                            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(data), new
                            SHA1CryptoServiceProvider());
                            string signedString = Convert.ToBase64String(signedData);

                            Dictionary<string, object> postData = new Dictionary<string, object>
                                {
                                    {"merchantCode", merchantCode},
                                    {"terminalCode", terminalCode},
                                    {"amount", finalPriceToPay.ToString()},
                                    {"redirectAddress", redirectAddress},
                                    {"invoiceNumber", paymentRequestCode.ToString()},
                                    {"invoiceDate", invoiceDate},
                                    {"action", ActionIs},
                                    {"sign", signedString},
                                    {"timeStamp", timeStamp}
                                };
                            return new RedirectAndPostActionResult("https://pep.shaparak.ir/gateway.aspx", postData);
                        }
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
                            Name = HelperFunctionInBL.GetVariableName(() => paymentRequestCode),
                            Value = paymentRequestCode.ToString()
                        }
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
                            Name = HelperFunctionInBL.GetVariableName(() => paymentRequestCode),
                            Value = paymentRequestCode.ToString()
                        }
                    };
                    long code = new ErrorLogBL().LogException(exp3, User.Identity.GetUserId() ?? Request.UserHostAddress, JObject.FromObject(lst).ToString());
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