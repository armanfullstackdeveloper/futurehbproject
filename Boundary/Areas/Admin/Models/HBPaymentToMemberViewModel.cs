namespace Boundary.Areas.Admin.Models
{
    public class HBPaymentToMemberViewModel
    {
        public long Id { get; set; }
        public string AdminName { get; set; }
        public int Date { get; set; }
        public int Money { get; set; }
        public string TrackingCode { get; set; }
        public long MemberCode { get; set; } 
        public string MemberName { get; set; }
    }
}