using DataModel.Enums;

namespace DataModel.Entities.RelatedToOrder {
    
    public class Order:EntityBase<Order>
    {
        public Order() { }
        public virtual long Id { get; set; }
        public virtual EOrderType OrderType { get; set; } 
        public virtual long MemberCode { get; set; }
        public virtual long? StoreCode { get; set; }


        /// <summary>
        /// چون ممکنه اصلا نیاز به پرداخت آنلاین نباشه
        /// </summary>
        public virtual long? PaymentRequestCode { get; set; }

        /// <summary>
        /// مبلغ پرداختی در درگاه می باشد و شامل هزینه ی پستی هم میشود
        /// </summary>
        public virtual int OverallPayment { get; set; }
        public virtual int SendingCost { get; set; } 
        public virtual long? StoreDiscountCode { get; set; }
        public virtual byte? OrderSendingTypeCode { get; set; } 
        public virtual string TrackingCode { get; set; }
    }
}
