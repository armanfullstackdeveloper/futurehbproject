using System;
using System.Configuration;
using BusinessLogic.Helpers;

namespace Boundary.Helper.StaticValue 
{
    /// <summary>
    /// مقادیر ثابت
    /// </summary>
    public class StaticNemberic : StaticNembericInBL
    {
        /// <summary>
        /// حداقل مبلغ مجاز پرداخت ها
        /// </summary>
        public static int MinimumTarakoneshValue => 1000;

        /// <summary>
        /// حداکثر تعداد تصاویر مجاز برای یک محصول
        /// </summary>
        public static int MaximumProductImage => 4;

        /// <summary>
        /// حداکثر ارتفاع عکس های کاربران
        /// </summary>
        public static int MaximumImageHeightSize => Convert.ToInt32(ConfigurationManager.AppSettings["MaximumImageHeightSize"]);

        /// <summary>
        /// حداکثر طول عکس های کاربران
        /// </summary>
        public static int MaximumImageWidthSize => Convert.ToInt32(ConfigurationManager.AppSettings["MaximumImageWidthSize"]);

    }
}