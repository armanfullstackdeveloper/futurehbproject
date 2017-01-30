namespace BusinessLogic.Helpers
{
    public class StaticNembericInBL
    {
        /// <summary>
        /// درصدی که هوجی بوجی برای هر سفارش میگیرد
        /// </summary>
        public static byte OrderProfitPercentForHoojiBooji { get { return 5; } }

        /// <summary>
        /// درصد سودی که از هر خرید نصیب خریدار می شود
        /// </summary>
        public static byte OrderProfitPercentForCustomer { get { return 5; } }

        /// <summary>
        /// تعداد آیتم هایی که برای ادمین در جداول نمایش داده خواهند شد
        /// </summary>
        public static int CountOfItemsInAdminPages { get { return 12; } }
    }
}
