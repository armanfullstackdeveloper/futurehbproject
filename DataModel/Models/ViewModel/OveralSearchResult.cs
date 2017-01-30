using System.Collections.Generic;

namespace DataModel.Models.ViewModel
{
    public class OveralSearchResultDataModel
    {
        public List<ProductsResultDataModel> ProductsResult { get; set; }
        public List<StoresResultDataModel> StoresResult { get; set; }
    }

    public class ProductsResultDataModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } 
        public string ProductImgAddress { get; set; }
        public string StoreName { get; set; }
        public string CityName { get; set; }
    }

    public class StoresResultDataModel
    {
        public long Id { get; set; }
        public string Name { get; set; } 
        public string ImgAddress { get; set; }
        public string CityName { get; set; }
    }
    
}
