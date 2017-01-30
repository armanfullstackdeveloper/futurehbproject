namespace DataModel.Entities.RelatedToProduct {

    public class AttributeType : EntityBase<AttributeType>
    {
        public AttributeType() { }
        public virtual byte Id { get; set; }
        public virtual string Name { get; set; }
    }
}
