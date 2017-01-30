using DataModel.Enums;

namespace DataModel.Models.DataModel
{
    public class StoreSessionDataModel
    {
        public long StoreCode { get; set; }
        public long SellerCode { get; set; }
        public long MemberCode { get; set; } 
        public long CityCode { get; set; }
        public long StateCode { get; set; }
        public string LogoAddress { get; set; }
        public EStoreStatus Status { get; set; } 
    }
}
