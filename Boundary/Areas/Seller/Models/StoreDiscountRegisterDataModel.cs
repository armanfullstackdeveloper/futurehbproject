using System.ComponentModel.DataAnnotations;

namespace Boundary.Areas.Seller.Models
{
    public class StoreDiscountRegisterDataModel
    {
        [Display(Name = "کد")]
        public virtual string Code { get; set; }

        [Display(Name = "درصد تخفیف")]
        public virtual byte DiscountPercent { get; set; }

        [Display(Name = "یکبار مصرف")]
        public virtual bool IsDisposable { get; set; }
    }
}
