using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct
{
    public class ProductVote : EntityBase<ProductVote>
    {
        public virtual long Id { get; set; }
        [StringLength(50)]
        public virtual string Ip { get; set; }
        public virtual long? ProductCode { get; set; }
        public virtual decimal? Score { get; set; }
    }
}
