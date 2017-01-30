using System.Collections.Generic;
using DataModel.Entities.RelatedToProduct;

namespace DataModel.Models.ViewModel
{
    public class CompleteProductForOne
    {
        public ProductDetailsViewModel Product { get; set; }
        public StoreSummery StoreSummery { get; set; }
        public List<Color> Colors { get; set; }
        public List<ProductAttrbiutesViewModel> ProductAttrbiutesViewModels { get; set; }
    }

    public class SearchResultViewModel
    {
        public List<ProductSummary> ProductsSummery { get; set; } 
        public int? ProductsCount { get; set; }
    }

}
