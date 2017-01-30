using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct
{

    public class Category : EntityBase<Category>
    {
        public Category() { }
        public virtual long Id { get; set; }
        [StringLength(50)]
        public virtual string Name { get; set; }
        public virtual string ImgAddress { get; set; }
        public virtual long? BaseCategoryCode { get; set; }

        public List<Category> SubCategories { get; set; }
    }
}
