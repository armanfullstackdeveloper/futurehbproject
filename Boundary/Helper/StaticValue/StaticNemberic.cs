using BusinessLogic.Helpers;

namespace Boundary.Helper.StaticValue 
{
    public class StaticNemberic : StaticNembericInBL
    {
        /// <summary>
        /// حداقل مبلغ مجاز پرداخت ها
        /// </summary>
        public static int MinimumTarakoneshValue { get { return 1000; } }

        /// <summary>
        /// حداکثر تعداد تصاویر مجاز برای یک محصول
        /// </summary>
        public static int MaximumProductImage { get { return 4; } }
    }
}