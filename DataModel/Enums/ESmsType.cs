using System.ComponentModel;

namespace DataModel.Enums
{
    public enum ESmsType : byte
    {
        [Description("ثبت نام")]
        Registeration = 0,
        [Description("فراموشی رمز عبور")]
        ForgetPassword = 1,
        [Description("سفارش جدید")]
        NewOrder = 2,
        [Description("تغییر وضعیت سفارش")]
        ChangeOrderStatus = 3
    }
}
