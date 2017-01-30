using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct {

    public class ProductComment : EntityBase<ProductComment>
    {
        public virtual long Id { get; set; }
        public virtual long? ProductCode { get; set; }
        [StringLength(100)]
        public virtual string SenderName { get; set; }
        [StringLength(500)]
        public virtual string Text { get; set; }
        public virtual bool? IsPass { get; set; }
        public virtual decimal? Date { get; set; }
        public virtual decimal? Time { get; set; }
        public virtual long InReplyTo { get; set; } 
    }
}
