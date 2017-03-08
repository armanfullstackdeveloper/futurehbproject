using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Enums;

namespace BusinessLogic.BussinesLogics
{
    public class ErrorLogBL : GenericRepository<ErrorLog, long>
    {
        /// <summary>
        /// خروجی کد رهگیری خطا هست که به کاربر نهایی نشان می دهیم
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="userCode"></param>
        /// <param name="actionInput"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public long LogException(MyExceptionHandler exception, string userCode, string actionInput = null,string userAgent="")
        {
            List<Exception> lst = new List<Exception>();
            Exception currentException = exception;
            while (currentException.InnerException != null)
            {
                lst.Add(currentException.InnerException);
                currentException = currentException.InnerException;
            }

            lst.Reverse(); //چون میخوایم با فورایچ کار کنیم

            long? temp = null, errorLogCode = 0;
            int date = PersianDateTime.Now.Date.ToInt();
            short time = PersianDateTime.Now.TimeOfDay.ToShort();

            foreach (Exception item in lst)
            {
                temp = new ErrorLogBL().Insert(new ErrorLog()
                {
                    Input = actionInput,
                    Message = item.Message,
                    StackTrace = item.StackTrace,
                    UserCode = string.IsNullOrEmpty(userCode) ? null : userCode,
                    ErrorLogCode = temp,
                    Date = date,
                    Time = time,
                    LogBy = ELogBy.WebBrowser,
                    UserAgent = userAgent
                });
                if (lst.IndexOf(item) == 0)
                    errorLogCode = temp;
            }



            //و در انتها آخرین اکسپشن که همان اکسپشن خودمان استو ذخیره میکنیم
            new ErrorLogBL().Insert(new ErrorLog()
            {
                Input = exception.Input,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                UserCode = string.IsNullOrEmpty(userCode) ? null : userCode,
                ErrorLogCode = temp,
                Date = date,
                Time = time,
                LogBy = ELogBy.WebBrowser,
                UserAgent = userAgent
            });
            return (long) errorLogCode;
        }


        public long LogException(Exception exception, string userCode, string actionInput = null, string userAgent = "")
        {
            List<Exception> lst = new List<Exception>() { exception };
            Exception currentException = exception;
            while (currentException.InnerException != null)
            {
                lst.Add(currentException.InnerException);
                currentException = currentException.InnerException;
            }

            lst.Reverse(); //چون میخوایم با فورایچ کار کنیم

            long? temp = null, errorLogCode = 0;
            int date = PersianDateTime.Now.Date.ToInt();
            short time = PersianDateTime.Now.TimeOfDay.ToShort();

            foreach (Exception item in lst)
            {
                temp = new ErrorLogBL().Insert(new ErrorLog()
                {
                    Input = actionInput,
                    Message = item.Message,
                    StackTrace = item.StackTrace,
                    UserCode = string.IsNullOrEmpty(userCode) ? null : userCode,
                    ErrorLogCode = temp,
                    Date = date,
                    Time = time,
                    LogBy = ELogBy.WebBrowser,
                    UserAgent = userAgent
                });
                if (lst.IndexOf(item) == 0)
                    errorLogCode = temp;
            }

            return (long) errorLogCode;
        }



        public List<ErrorLog> GetNewExceptions() 
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                List<ErrorLog> lstResult = db.Query<ErrorLog>("select * from [dbo].[ErrorLog] where ErrorLogCode is null order by Id desc", null).ToList();
                EnsureCloseConnection(db);
                return lstResult;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, null);
            }
        }
    }
}
