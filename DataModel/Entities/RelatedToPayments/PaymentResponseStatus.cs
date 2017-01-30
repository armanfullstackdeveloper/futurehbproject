namespace DataModel.Entities.RelatedToPayments {

    public class PaymentResponseStatus : EntityBase<PaymentResponseStatus>
    {
        public PaymentResponseStatus() { }
        public virtual byte Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Comments { get; set; }
    }
}
