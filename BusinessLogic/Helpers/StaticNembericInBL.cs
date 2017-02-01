using System;
using System.Configuration;

namespace BusinessLogic.Helpers
{
    public class StaticNembericInBL
    {
        /// <summary>
        /// درصدی که هوجی بوجی برای هر سفارش میگیرد
        /// </summary>
        public static byte OrderProfitPercentForHoojiBooji => Convert.ToByte(ConfigurationManager.AppSettings["OrderProfitPercentForHoojiBooji"]);

        /// <summary>
        /// درصد سودی که از هر خرید نصیب خریدار می شود
        /// </summary>
        public static byte OrderProfitPercentForCustomer => Convert.ToByte(ConfigurationManager.AppSettings["OrderProfitPercentForCustomer"]);

        /// <summary>
        /// تعداد آیتم هایی که برای ادمین در جداول نمایش داده خواهند شد
        /// </summary>
        public static int CountOfItemsInAdminPages => Convert.ToByte(ConfigurationManager.AppSettings["CountOfItemsInAdminPages"]);
    }
}
