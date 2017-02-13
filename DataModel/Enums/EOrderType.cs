using System.ComponentModel;

namespace DataModel.Enums 
{
    public enum EOrderType
    {
        [Description("پرداخت امن")]SecurePayment=1,
        [Description("پرداخت آزاد")]FreePayment = 2
    }
}