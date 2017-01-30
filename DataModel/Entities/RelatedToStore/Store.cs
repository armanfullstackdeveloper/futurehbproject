using System.ComponentModel.DataAnnotations;
using DataModel.Enums;

namespace DataModel.Entities.RelatedToStore
{

    public class Store : EntityBase<Store>
    {
        public Store() { }
        public virtual long Id { get; set; }

        [StringLength(100)]
        [Display(Name = "نام فروشگاه")]
        public virtual string Name { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات فروشگاه")]
        public virtual string Comments { get; set; }

        public virtual long SallerCode { get; set; }

        [Display(Name = "کد شناسه صنفی")]
        public virtual int? CommercialCode { get; set; }

        [Display(Name = "شماره تلفن معرف")]
        public virtual decimal? ReagentPhoneNumber { get; set; }

        [StringLength(128)]
        public virtual string UserCode { get; set; }

        [Display(Name = "نام شهر")]
        public virtual long? CityCode { get; set; }

        [StringLength(200)]
        [Display(Name = "آدرس کامل")]
        public virtual string Place { get; set; }

        /// <summary>
        /// decimal(20, 17)
        /// </summary>
        public virtual decimal? Latitude { get; set; }
        /// <summary>
        /// decimal(20, 17)
        /// </summary>
        public virtual decimal? Longitude { get; set; }

        /// <summary>
        /// this is for main image 
        /// </summary>
        [StringLength(200)]
        public virtual string ImgAddress { get; set; }

        [StringLength(200)]
        public virtual string LogoAddress { get; set; }
        
        public byte StoreTypeCode { get; set; } 

        /// <summary>
        /// it'n in our app, it's for saller
        /// </summary>
        [StringLength(100)]
        public virtual string Website { get; set; }

        public virtual decimal? RegisterDate { get; set; }

        public string HomePage { get; set; }

        public EStoreStatus StoreStatus { get; set; }
    }
}
