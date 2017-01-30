namespace DataModel.Models.ViewModel
{
    public class VerifyDiscountedCodeAndUsingBalanceViewModel
    {
        public bool DiscountedCodeAccept { get; set; }
        public bool MemberUsedBalanceAccept { get; set; }
        public int? OverallCostWithConsideringDiscountedCode { get; set; }
        public int? OverallCostWithConsideringMemberUsedBalance { get; set; }
        public int? OverallCost { get; set; }
    }
}
