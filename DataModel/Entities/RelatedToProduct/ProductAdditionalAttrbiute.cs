namespace DataModel.Entities.RelatedToProduct
{
    public class ProductAdditionalAttrbiute : EntityBase<ProductAdditionalAttrbiute>
    {
        public virtual long Id { get; set; }
        public virtual long? ProductCode { get; set; }
        public virtual string Title { get; set; }
        public virtual string Value { get; set; }
    }

    public class ProductAdditionalAttrbiuteDataModel
    {
        public virtual string Title { get; set; }
        public virtual string Value { get; set; }
    }
}
