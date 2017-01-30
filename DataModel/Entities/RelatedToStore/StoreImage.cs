using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToStore {

    /// <summary>
    /// this class dosent used in app,until now
    /// </summary>
    public class StoreImage : EntityBase<StoreImage>
    {
        public virtual long Id { get; set; }
        public virtual long? StoreCode { get; set; }
        [StringLength(150)]
        public virtual string ImgAddress { get; set; }
    }
}
