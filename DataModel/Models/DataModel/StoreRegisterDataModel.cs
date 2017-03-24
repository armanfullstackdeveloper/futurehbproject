using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataModel.Enums;

namespace DataModel.Models.DataModel
{
    /// <summary>
    /// این مدل برای ثبت فروشگاه در حالت عادی است
    /// </summary>
    public class StoreRegisterDataModel : StoreRegisterForUpgradeMemberToSallerDataModel
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// این مدل برای ثبت فروشگاه در حالت ارتقاء از کاربر به فروشنده است
    /// </summary>
    public class StoreRegisterForUpgradeMemberToSallerDataModel
    {
        #region فروشگاه

        [StringLength(100)]
        [Display(Name = "نام فروشگاه")]
        public string StoreName { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات فروشگاه")]
        public string StoreComments { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "کد شناسه صنفی")]
        public int? CommercialCode { get; set; }

        [Display(Name = "شماره تلفن معرف")]
        public decimal? ReagentPhoneNumber { get; set; }

        [Display(Name = "نام شهر")]
        public long? CityCode { get; set; }

        [StringLength(200)]
        [Display(Name = "آدرس کامل")]
        public string Place { get; set; }

        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }

        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }

        [Display(Name = "نوع محصولات فروشگاه")]
        public List<long> ListCategoryCode { get; set; }

        /// <summary>
        /// it's not required! just because javad cant sent list |:
        /// </summary>
        public string CategoryCodes { get; set; }

        public EStoreType StoreTypeCode { get; set; }
        
        public string Website { get; set; }

        #endregion



        #region فروشنده

        [StringLength(100)]
        [Display(Name = "نام فروشنده")]
        public string SallerName { get; set; }

        //[StringLength(10, MinimumLength = 10)]
        [Display(Name = "کد ملی ")]
        public decimal? NationalCode { get; set; }

        [Display(Name = "توضیحات فروشنده")]
        [StringLength(1000)]
        public string SallerComments { get; set; }
        public bool? IsMale { get; set; }

        #endregion

    }

    public class StoreEditDataModel
    {
        public long StoreCode { get; set; }

        [StringLength(100)]
        [Display(Name = "نام فروشگاه")]
        public string StoreName { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات فروشگاه")]
        public string StoreComments { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "کد شناسه صنفی")]
        public int? CommercialCode { get; set; }

        [Display(Name = "شماره تلفن معرف")]
        public decimal? ReagentPhoneNumber { get; set; }

        [Display(Name = "نام شهر")]
        public long? CityCode { get; set; }

        [StringLength(200)]
        [Display(Name = "آدرس کامل")]
        public string Place { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        [Display(Name = "شماره تماس")]
        public List<string> PhoneNumbers { get; set; } 

        [Display(Name = "نوع محصولات فروشگاه")]
        public List<long> ListCategoryCode { get; set; }

        public byte StoreTypeCode { get; set; }

        public string Website { get; set; }
        public string HomePage { get; set; }
        public string Email { get; set; }
    }
}
