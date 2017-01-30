using System.Collections.Generic;

namespace DataModel.Models.ViewModel
{
    public class ProductRegisterRequireItemsViewModel
    {
        public List<ProductAttributeWithoutItemsViewModel> ProductAttributeWithoutItems { get; set; }
        public List<ProductAttributeWithItemsViewModel> ProductSelectAttributeWithItems { get; set; }
        public List<ProductAttributeWithItemsViewModel> ProductMultiSelectAttributeWithItems { get; set; }

        public List<DropDownItemsModel> Brands { get; set; }
        public List<ColorViewModel> Colors { get; set; } 
    }

    public class ProductAttributeWithoutItemsViewModel
    {
        public long AttributeCode { get; set; }
        public string AttributeName { get; set; }
    }

    public class ProductAttributeWithItemsViewModel 
    {
        public long AttributeCode { get; set; }
        public string AttributeName { get; set; }
        public List<DropDownItemsModel> AttributeItems { get; set; }
    }
}
