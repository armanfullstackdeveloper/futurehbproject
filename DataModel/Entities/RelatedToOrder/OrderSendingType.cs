using System;
namespace DataModel.Entities.RelatedToOrder
{
    public class OrderSendingType : EntityBase<OrderSendingType>
    {
        public virtual byte Id { get; set; }
        public virtual string Name { get; set; }
    }
}
