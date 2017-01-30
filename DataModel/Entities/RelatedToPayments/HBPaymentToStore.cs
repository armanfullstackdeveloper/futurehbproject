namespace DataModel.Entities.RelatedToPayments
{

    public class HBPaymentToStore : EntityBase<HBPaymentToStore>
    {
        public long OrderCode { get; set; }
        public long AdminCode { get; set; }
        public int Date { get; set; }
        public string TrackingCode { get; set; }
        public int Money { get; set; } 
    }
}
