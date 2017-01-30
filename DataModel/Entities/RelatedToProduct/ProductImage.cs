using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct {

    public class ProductImage : EntityBase<ProductImage>
    {
        public ProductImage() { }
        public virtual long Id { get; set; }
        public virtual long? ProductCode { get; set; }
        [StringLength(200)]
        public virtual string ImgAddress { get; set; }
    }

    public class ImageViewModel
    {
        public long ImgCode { get; set; } 
        public string ImgAddress { get; set; }
    }

}
