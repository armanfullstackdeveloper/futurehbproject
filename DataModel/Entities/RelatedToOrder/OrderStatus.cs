namespace DataModel.Entities.RelatedToOrder {

    public class OrderStatus : EntityBase<OrderStatus>
    {
        public OrderStatus() { }
        public virtual byte Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Comments { get; set; }
    }
}
