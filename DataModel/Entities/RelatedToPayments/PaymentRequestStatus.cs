namespace DataModel.Entities.RelatedToPayments {

    public class PaymentRequestStatus : EntityBase<PaymentRequestStatus>
    {
        public PaymentRequestStatus() { }
        public virtual byte Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Comments { get; set; }
    }
}
