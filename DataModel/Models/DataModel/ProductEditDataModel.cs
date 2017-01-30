using System;
namespace DataModel.Models.DataModel
{
    public class ProductEditDataModel
    {
        public long AttributeCode { get; set; }
        public long AttributeValueCode { get; set; }
        public bool? IsMultiDropdown { get; set; }
    }
}
