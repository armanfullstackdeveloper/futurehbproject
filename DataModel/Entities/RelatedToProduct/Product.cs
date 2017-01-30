using System.ComponentModel.DataAnnotations;
using DataModel.Enums;

namespace DataModel.Entities.RelatedToProduct
{

    public class Product : EntityBase<Product>
    {
        public long Id { get; set; }
        public long? CategoryCode { get; set; }
        public long StoreCode { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }


        [Display(Name = "قیمت با تخفیف")]
        public int? DiscountedPrice { get; set; }

        public int? RegisterDate { get; set; }


        [Display(Name = "قابلیت ارسال")]
        public bool CanSend { get; set; }


        [Display(Name = "هزینه پستی برون شهری")]
        public int? PostalCostInCountry { get; set; }

        [Display(Name = "هزینه پستی درون شهری")]
        public int? PostalCostInTown { get; set; }


        [Display(Name = "قابلیت تعویض")]
        public bool Changeability { get; set; }


        [Display(Name = "قابلیت پس دادن")]
        public bool CanGiveBack { get; set; }


        [Display(Name = "موجود")]
        public bool IsExist { get; set; }


        [Display(Name = "برند")]
        public long? BrandCode { get; set; }


        [Display(Name = "گارانتی")]
        public string Warranty { get; set; }


        [Display(Name = "کشور سازنده")]
        [StringLength(15)]
        public string MadeIn { get; set; }


        [Display(Name = "نام کالا")]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// this is for main image 
        /// </summary>
        [StringLength(200)]
        public string ImgAddress { get; set; }

        public EProductStatus Status { get; set; }
    }

    
}
