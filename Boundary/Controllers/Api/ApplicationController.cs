using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Http;
using Boundary.Helper;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.ContactsBL;
using BusinessLogic.BussinesLogics.RelatedToProductBL;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using BusinessLogic.Helpers;
using DataModel.Entities;
using DataModel.Entities.RelatedToProduct;
using DataModel.Entities.RelatedToStore;
using DataModel.Enums;
using DataModel.Models.DataModel;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Boundary.Controllers.Api
{
    [RoutePrefix("api/Application")]
    public class ApplicationController : ApiController
    {
        //[HttpGet]
        //[Route("CheckForNewVersion")]
        //public IHttpActionResult CheckForNewVersion(double currentVersion) 
        //{
        //    try
        //    {
        //        //string address = HttpContext.Current.Server.MapPath("~/Content/AndroidAPK");
        //        //string[] apkFiles = Directory.GetFiles(address)
        //        //                     .Select(path => Path.GetFileName(path))
        //        //                     .ToArray();
        //        //foreach (string item in apkFiles)
        //        //{
        //        //    double filename = Convert.ToDouble(Path.GetFileNameWithoutExtension(item));
        //        //    if (filename > currentVersion)
        //        //    {
        //        //        string filePath = address + "/" + item;
        //        //        if (File.Exists(filePath))
        //        //        {
        //        //            FileInfo fileInfo = new FileInfo(filePath);
        //        //            if (fileInfo.Exists)
        //        //            {
        //        //                return Json(JsonResultHelper.SuccessResult("Content/AndroidAPK/" + item));
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        double newVersion= Convert.ToDouble(ConfigurationManager.AppSettings["AndroidApkVersion"]);
        //        if(newVersion>currentVersion)
        //            return Json(JsonResultHelper.SuccessResult(ConfigurationManager.AppSettings["AndroidApkAddress"])); 
        //        return Json(JsonResultHelper.FailedResultWithMessage("there is no new version"));
        //    }
        //    catch (MyExceptionHandler exp1)
        //    {
        //        try
        //        {
        //            List<ActionInputViewModel> lst = new List<ActionInputViewModel>
        //            {
        //                new ActionInputViewModel
        //                {
        //                    Name = HelperFunctionInBL.GetVariableName(() => currentVersion),
        //                    Value = currentVersion.ToString()
        //                }
        //            };
        //            long code = new ErrorLogBL().LogException(exp1, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
        //            return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
        //        }
        //        catch (Exception)
        //        {
        //            return Json(JsonResultHelper.FailedResultWithMessage());
        //        }
        //    }
        //    catch (Exception exp3)
        //    {
        //        try
        //        {
        //            List<ActionInputViewModel> lst = new List<ActionInputViewModel>
        //            {
        //               new ActionInputViewModel
        //               {
        //                    Name = HelperFunctionInBL.GetVariableName(() => currentVersion),
        //                    Value = currentVersion.ToString()
        //                }
        //            };
        //            long code = new ErrorLogBL().LogException(exp3, RequestContext.Principal.Identity.GetUserId() ?? HttpContext.Current.Request.UserHostAddress, JArray.FromObject(lst).ToString());
        //            return Json(JsonResultHelper.FailedResultWithTrackingCode(code));
        //        }
        //        catch (Exception)
        //        {
        //            return Json(JsonResultHelper.FailedResultWithMessage());
        //        }
        //    }
        //}

        [HttpGet]
        [Route("CheckForPublicMessage")]
        public IHttpActionResult CheckForPublicMessage()
        {
            try
            {
                string userId = RequestContext.Principal.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(JsonResultHelper.SuccessResult(new PublicMessageBL().GetNewest(false)));
                }
                return Json(JsonResultHelper.SuccessResult(new PublicMessageBL().GetNewest(true)));
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

        [HttpPost]
        [Route("InstallAppReport")]
        public IHttpActionResult InstallAppReport(MobileInstalled mobileInstalled)
        {
            try
            {
                if (string.IsNullOrEmpty(mobileInstalled.UniqKey) == false &&
                    new MobileInstalledBL().IsInstalledBefore(mobileInstalled.UniqKey) == false)
                    new MobileInstalledBL().Insert(mobileInstalled);
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
                            Name = HelperFunctionInBL.GetVariableName(() => mobileInstalled),
                            Value = JObject.FromObject(mobileInstalled).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => mobileInstalled),
                            Value = JObject.FromObject(mobileInstalled).ToString()
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
        [Route("UserProductReport")]
        public IHttpActionResult UserProductReport(long productCode, byte reportCode)
        {
            try
            {
                long result =
                    new UserProductReportBL().Insert(new UserProductReport
                    {
                        ProductCode = productCode,
                        ReportCode = reportCode
                    });
                if (result > 0)
                    Json(JsonResultHelper.SuccessResult());
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => productCode),
                            Value = productCode.ToString()
                        },new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => reportCode),
                            Value = reportCode.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => productCode),
                            Value = productCode.ToString()
                        },new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => reportCode),
                            Value = reportCode.ToString()
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
        [Route("UserStoreReport")]
        public IHttpActionResult UserStoreReport(long storeCode, byte reportCode)
        {
            try
            {
                long result =
                        new UserStoreReportBL().Insert(new UserStoreReport
                        {
                            StoreCode = storeCode,
                            ReportCode = reportCode
                        });
                if (result > 0)
                    Json(JsonResultHelper.SuccessResult());
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
                        },new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => reportCode),
                            Value = reportCode.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => storeCode),
                            Value = storeCode.ToString()
                        },new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => reportCode),
                            Value = reportCode.ToString()
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
        [Route("UserSurvey")]
        public IHttpActionResult UserSurvey(byte scoreCode)
        {
            try
            {
                long? result = new MobileAppUserSurveyBL().SaveWithDapper(scoreCode);
                if (result != null && result > 0)
                    Json(JsonResultHelper.SuccessResult());
                return Json(JsonResultHelper.FailedResultWithMessage());
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => scoreCode),
                            Value = scoreCode.ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => scoreCode),
                            Value = scoreCode.ToString()
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

        [HttpPost]
        [Route("SaveException")]
        public IHttpActionResult ApplicationError(ErrorLogVM errorLog)
        {
            try
            {
                string userId = RequestContext.Principal.Identity.GetUserId();
                long result=new ErrorLogBL().Insert(new ErrorLog
                {
                    Date = PersianDateTime.Now.Date.ToInt(),
                    Time = PersianDateTime.Now.TimeOfDay.ToShort(),
                    ErrorLogCode = errorLog.ErrorLogCode,
                    Input = errorLog.Input,
                    Message = errorLog.Message,
                    StackTrace = errorLog.StackTrace,
                    UserCode = userId,
                    ClientType = EClientType.Android
                });
                return Json(JsonResultHelper.SuccessResult(result));
            }
            catch (MyExceptionHandler exp1)
            {
                try
                {
                    List<ActionInputViewModel> lst = new List<ActionInputViewModel>
                    {
                        new ActionInputViewModel
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => errorLog),
                            Value = JObject.FromObject(errorLog).ToString()
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
                            Name = HelperFunctionInBL.GetVariableName(() => errorLog),
                            Value = JObject.FromObject(errorLog).ToString()
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
