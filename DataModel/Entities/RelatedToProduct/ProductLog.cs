using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct {

    public class ProductLog : EntityBase<ProductLog>
    {
        public virtual long Id { get; set; }
        [StringLength(20)]
        public virtual string Ip { get; set; }
        public virtual long? ProductCode { get; set; }
        public virtual decimal? LogDate { get; set; }
        public virtual decimal? LogTime { get; set; }
    }
}
