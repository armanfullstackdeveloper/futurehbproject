namespace DataModel.Entities.RelatedToProduct {

    public class Attribute : EntityBase<Attribute>
    {
        public Attribute() { }
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long CategoryCode { get; set; }
        public virtual byte? AttributeTypeCode { get; set; }
    }
}
