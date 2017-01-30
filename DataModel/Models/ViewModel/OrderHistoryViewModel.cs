using DataModel.Enums;

namespace DataModel.Models.ViewModel
{
    public class OrderHistoryViewModel
    {
        public long OrderCode { get; set; }
        public EOrderStatus OrderStatus { get; set; } 
        public string UserCode { get; set; }
        public string UserRoleName { get; set; } 
        public int Date { get; set; }
        public int Time { get; set; }
    }
}