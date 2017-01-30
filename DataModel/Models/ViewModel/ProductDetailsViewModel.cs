using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataModel.Entities.RelatedToProduct;
using DataModel.Enums;

namespace DataModel.Models.ViewModel
{
    public class ProductDetailsViewModel
    {
        public long Id { get; set; }
        public long CategoryCode { get; set; }
        public long StoreCode { get; set; }

        [Display(Name = "نام کالا")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "قیمت")]
        public int? Price { get; set; }

        [Display(Name = "قیمت با تخفیف")]
        public int? DiscountedPrice { get; set; }

        [Display(Name = "قابلیت ارسال")]
        public bool CanSend { get; set; }

        public int? PostalCostInCountry { get; set; } 

        public int? PostalCostInTown { get; set; }

        [Display(Name = "قابلیت تعویض")]
        public bool? Changeability { get; set; }

        [Display(Name = "قابلیت پس دادن")]
        public bool? CanGiveBack { get; set; }

        [Display(Name = "موجود")]
        public bool? IsExist { get; set; }

        [Display(Name = "برند")]
        public string Brand { get; set; }

        [Display(Name = "گارانتی")]
        public string Warranty { get; set; }

        [Display(Name = "کشور سازنده")]
        [StringLength(15)]
        public string MadeIn { get; set; }

        /// <summary>
        /// this is for main image 
        /// </summary>
        [StringLength(200)]
        public string ImgAddress { get; set; }

        public EProductStatus Status { get; set; } 

        public decimal? AvgUsersScores { get; set; }

        public int UsersScoresCount { get; set; }

        public List<ImageViewModel> OtherImagesAddress { get; set; }
    }
}
