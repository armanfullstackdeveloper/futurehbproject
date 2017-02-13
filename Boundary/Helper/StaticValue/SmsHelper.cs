using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boundary.Helper.StaticValue
{
    public class SmsHelper
    {
        /// <summary>
        /// registeration code message 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string RegisterMessage(int code)
        {
            return $"کد فعال سازی شما برای ثبت نام در هوجی بوجی: {code}";
        }

        public static string NewPass(int pass)
        {
            return $"رمز عبور جدید شما: {pass}";
        }
    }
}