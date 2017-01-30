using System.Collections.Generic;
using DataModel.Enums;

namespace DataModel.Models.ViewModel
{
    public class ProductSummary
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public string ImgAddress { get; set; }
        public bool IsExist { get; set; }
        public EProductStatus Status { get; set; }  
        public List<ProductAttrbiutesViewModel> ProductAttrbiutesViewModels { get; set; }


        public long? StoreCode { get; set; }
        public string StoreName { get; set; }
        public string CityName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
