using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct {

    public class Brand : EntityBase<Brand>
    {
        public Brand() { }
        public virtual long Id { get; set; }
        [StringLength(50)]
        public virtual string Name { get; set; }
        public virtual long? CategoryCode { get; set; }
        public virtual string EngName { get; set; } 
    }
}
