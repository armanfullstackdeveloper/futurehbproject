using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToStore {

    public class Transaction : EntityBase<Transaction>
    {
        public Transaction() { }
        public virtual long Id { get; set; }
        public virtual long? StoreCode { get; set; }
        public virtual decimal? TransactionDate { get; set; }
        public virtual decimal? TransactionTime { get; set; }
        public virtual decimal? Cost { get; set; }
        [StringLength(100)]
        public virtual string TrackingCode { get; set; }
        public virtual bool? IsChecked { get; set; }
        public virtual bool? IsSuccessful { get; set; }
    }
}
