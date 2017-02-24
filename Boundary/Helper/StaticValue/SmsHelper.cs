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

        public static string NewOrderForBuyer(string trackingCode) 
        {
            return $"خرید شما با موفقیت ثبت شد. کد پیگیری خرید شما {trackingCode} می باشد. با تشکر از همراهی شما";
        }

        public static string NewOrderForSeller()
        {
            return $"سفارش جدیدی برای شما در هوجی بوجی ایجاد شده است. لطفا وارد پنل خود شده و نسبت به ویرایش وضعیت سفارش اقدام کنید. با تشکر از همراهی شما.";
        }

        public static string NewOrderForAdmin()
        {
            return $"اطللاع رسانی جهت ایجاد سفارش جدید در هوجی بوجی";
        }
    }
}