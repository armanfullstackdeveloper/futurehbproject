using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataModel.Entities.RelatedToProduct;

namespace DataModel.Models.DataModel
{
    public class ProductRegisterDataModel
    {
        public ProductMainAttributeDataModel ProductMainAttributeDataModels { get; set; }  
        public List<ProductAttributeWithoutItemsDataModel> ProductAttributeWithoutItemsDataModels { get; set; }
        public List<ProductAttributeWithItemsForSaveAndUpdate> ProductAttributeWithItemsDataModels { get; set; }
        public List<ProductAdditionalAttrbiuteDataModel> ProductAdditionalAttrbiutes { get; set; }
    }

    public class ProductAttributeWithoutItemsDataModel
    {
        public long AttributeCode { get; set; }
        public string Value { get; set; }
    }

    public class ProductAttributeWithItemsDataModel
    {
        public long Code { get; set; }
        public List<long> Values { get; set; }
    }

    public class ProductAttributeWithItemsForSaveAndUpdate
    {
        public long Code { get; set; }
        public List<long> Values { get; set; }
        public List<string> TextValues { get; set; } 
    }

    public class ProductMainAttributeDataModel
    {
        public virtual long Id { get; set; }
        public virtual long? CategoryCode { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }


        [Display(Name = "قیمت با تخفیف")]
        public int? DiscountedPrice { get; set; }


        [Display(Name = "قابلیت ارسال")]
        public bool CanSend { get; set; }


        [Display(Name = "هزینه پستی برون شهری")]
        public int? PostalCostInCountry { get; set; }

        [Display(Name = "هزینه پستی درون شهری")]
        public int? PostalCostInTown { get; set; }


        [Display(Name = "قابلیت تعویض")]
        public bool? Changeability { get; set; }


        [Display(Name = "قابلیت پس دادن")]
        public bool? CanGiveBack { get; set; }


        [Display(Name = "موجود")]
        public bool? IsExist { get; set; }


        [Display(Name = "برند")]
        public long? BrandCode { get; set; }


        [Display(Name = "گارانتی")]
        public string Warranty { get; set; }


        [Display(Name = "کشور سازنده")]
        public string MadeIn { get; set; }


        [Display(Name = "نام کالا")]
        public string Name { get; set; }

        public List<long> Colors { get; set; }

        /// <summary>
        /// فقط برای ویرایش محصول استفاده شده
        /// </summary>
        public List<ImageViewModel> OtherImagesAddress { get; set; }
    }
}
