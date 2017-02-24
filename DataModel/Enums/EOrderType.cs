using System.ComponentModel;

namespace DataModel.Enums
{
    public enum EOrderType : byte
    {
        [Description("پرداخت امن")]
        SecurePayment = 1,
        [Description("پرداخت آزاد")]
        FreePayment = 2
    }
}