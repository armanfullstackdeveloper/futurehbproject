namespace DataModel.Entities.RelatedToPayments
{
    public class PaymentResponse : EntityBase<PaymentResponse>
    {
        public virtual long Id { get; set; }
        public virtual long PaymentRequestCode { get; set; }
        public virtual byte PaymentResponseStatusCode { get; set; }
        public virtual long TrackingCode { get; set; }
        public virtual int VerifyDate { get; set; }
        public virtual short VerifyTime { get; set; }
    }
}
