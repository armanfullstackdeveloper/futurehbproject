using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToOrder
{
    public class OrderCustomerInfo : EntityBase<OrderCustomerInfo>
    {
        public virtual long Id { get; set; }

        [StringLength(50)]
        public virtual string Name { get; set; }
        public virtual long OrderCode { get; set; } 
        public virtual long? CityCode { get; set; }
        [StringLength(200)]
        public virtual string Place { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Comments { get; set; }
    }
}
