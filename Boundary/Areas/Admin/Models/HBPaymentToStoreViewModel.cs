using DataModel.Entities.RelatedToPayments;

namespace Boundary.Areas.Admin.Models
{
    public class HBPaymentToStoreViewModel
    {
        public long OrderCode { get; set; }
        public int Date { get; set; }
        public string TrackingCode { get; set; }
        public int Money { get; set; } 
        public string AdminName { get; set; }
    }
}