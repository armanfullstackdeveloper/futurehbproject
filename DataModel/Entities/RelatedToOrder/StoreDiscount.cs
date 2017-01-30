using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToOrder {

    public class StoreDiscount : EntityBase<StoreDiscount>
    {
        public StoreDiscount() { }
        public virtual long Id { get; set; }
        public virtual long? StoreCode { get; set; }

        [Display(Name = "درصد تخفیف")]
        public virtual byte DiscountPercent { get; set; }

        [Display(Name = "کد")]
        public virtual string Code { get; set; }

        [Display(Name = "یکبار مصرف")]
        public virtual bool IsDisposable { get; set; }

        [Display(Name = "وضعیت")]
        public virtual bool IsActive { get; set; }
    }
}
