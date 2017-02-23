using DataModel.Enums;

namespace DataModel.Entities.RelatedToPayments
{
    public class PaymentResponse : EntityBase<PaymentResponse>
    {
        public virtual long Id { get; set; }
        public virtual long PaymentRequestCode { get; set; }
        public virtual EPaymentResponseStatus PaymentResponseStatus { get; set; }
        public virtual string ShomareMarja { get; set; }
        public virtual string ShomareErja { get; set; }
        public virtual string PaymentResult { get; set; } 
        public virtual string TrackingCode { get; set; }
        public virtual int VerifyDate { get; set; }
        public virtual short VerifyTime { get; set; }
    }
}
