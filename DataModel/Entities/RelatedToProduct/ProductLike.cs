using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct
{
    public class ProductLike : EntityBase<ProductLike>
    {
        public virtual long Id { get; set; }
        [StringLength(20)]
        public virtual string Ip { get; set; }
        public virtual long? ProductCode { get; set; }
    }
}
