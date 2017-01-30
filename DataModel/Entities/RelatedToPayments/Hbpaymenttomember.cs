namespace DataModel.Entities.RelatedToPayments
{

    public class HBPaymentToMember : EntityBase<HBPaymentToMember>
    {
        public long Id { get; set; }
        public long AdminCode { get; set; }
        public int Date { get; set; }
        public int Money { get; set; }
        public string TrackingCode { get; set; }
        public long MemberCode { get; set; }
    }
}
