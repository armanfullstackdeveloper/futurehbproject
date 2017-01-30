namespace DataModel.Models.DataModel
{
    public class ProductToShoppingBagDataModel
    {
        public long ProductCode { get; set; }
        public string ProductName { get; set; }

        public long StoreCityCode { get; set; } 
        public string StoreName { get; set; }
        public long  StoreCode { get; set; } 

        /// <summary>
        /// اگه محصول قیمت با تخفیف داشت
        /// RealPrice 
        /// را برابر قیمت با تخفیف آن قرار دهید و در غیر این صورت برابر قیمت آن
        /// </summary>
        public int RealPrice { get; set; }
        public string ImgAddress { get; set; }
        public int? PostalCostInCountry { get; set; }
        public int? PostalCostInTown { get; set; }
    }
}
