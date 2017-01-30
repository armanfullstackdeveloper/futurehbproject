using System.Collections.Generic;
namespace DataModel.Models.ViewModel
{
    public class ProductAttrbiutesViewModel
    {
        public string AttributeName { get; set; }
        public List<string> AttributeValues { get; set; }
    }

    public class ProductAttrbiutesDataModel
    {
        public long ProductCode { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}
