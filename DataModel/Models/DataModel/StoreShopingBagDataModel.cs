using System.Collections.Generic;
using DataModel.Entities.RelatedToStore;

namespace DataModel.Models.DataModel
{
    public class StoreShopingBagDataModel
    {
        public StoreShopingBagDataModel(Store store)
        {
            Products = new List<ProductInShopingBag>();
            Store = store;
        }
        public Store Store { get; set; }
        public List<ProductInShopingBag> Products { get; set; }
        public long? DiscountCode { get; set; }
    }

    public class ProductInShopingBag
    {
        public long ProductCode { get; set; }
        public int RealPrice { get; set; }
        public int ProductCostConsideringDiscountCodeAndCount { get; set; } 
        public int PostalCostInCountry { get; set; }
        public int PostalCostInTown { get; set; } 
        public short Count { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
