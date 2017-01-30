using System.Collections.Generic;

namespace DataModel.Models.ViewModel
{
    public class ProductSearchRequireItemsViewModel
    {
        public List<ProductAttributeWithItemsViewModel> ProductAttributeWithItems { get; set; }
        public List<DropDownItemsModel> Brands { get; set; }
        public List<ColorViewModel> Colors { get; set; }
        public List<StateViewModel> States { get; set; }
        public int MaxPrice { get; set; }
    }
}
