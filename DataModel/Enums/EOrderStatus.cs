namespace DataModel.Enums 
{
    public enum EOrderStatus
    {
        AwaitingPayment=0,        //در انتظار پرداخت
        PendingApprovalSeller=1,  //در انتظار تائید فروشنده
        RejectedBySeller=2,       //رد شده توسط فروشنده
        VerifiedSeller=3,         //تائید فروشنده
        Posted=4,                 //ارسال شده
        BackShaken=5,             //برگشت زده شده
        Received=6,               //دریافت شده
        Closed=7,                 //بسته شده
        RefuseBySeller = 8,       //انصراف فروشنده
        RefuseByMember = 9,       //انصراف خریدار
    }
}