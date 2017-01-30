using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToStore {

    public class Seller : EntityBase<Seller>
    {
        public Seller() { }
        public virtual long Id { get; set; }
        [StringLength(100)]
        public virtual string Name { get; set; }
        public virtual decimal? NationalCode { get; set; }
        [StringLength(1000)]
        public virtual string Comments { get; set; }
        [StringLength(150)]
        public virtual string PicAddress { get; set; }
        public virtual bool? IsMale { get; set; }
    }
}
