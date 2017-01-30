namespace DataModel.Models.ViewModel
{
    public class PaymentResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string TrackingCode { get; set; }
        public int? MemberProfit { get; set; }
        public bool? IsProfitAddedToBalance { get; set; }
    }
}
