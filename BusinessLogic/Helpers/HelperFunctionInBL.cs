using System;
using System.Linq.Expressions;
using DataModel.Enums;

namespace BusinessLogic.Helpers
{
    public class HelperFunctionInBL
    {
        public static string GetVariableName<T>(Expression<Func<T>> expr)
        {
            var body = (MemberExpression)expr.Body;
            return body.Member.Name;
        }

        public static int GetProfit(int cost, byte percent)
        {
            int profit = cost * percent;
            profit /= 100;
            return profit;
        }

        public static string GetOrderTypeName(EOrderType orderType)
        {
            switch (orderType)
            {
                case EOrderType.SecurePayment:
                    return "پرداخت امن";
                case EOrderType.FreePayment:
                    return "پرداخت آزاد";
                default:
                    return "-";
            }
        }
    }
}
