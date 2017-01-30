namespace DataModel.Entities.RelatedToPayments
{
    public class PaymentRequest : EntityBase<PaymentRequest>
    {
        public virtual long Id { get; set; }
        public virtual byte PaymentRequestStatusCode { get; set; }
        public virtual string Description { get; set; }
    }

}
