namespace DataModel.Entities.RelatedToStore {

    public class SalesPlan : EntityBase<SalesPlan>
    {
        public virtual long Id { get; set; }
        public virtual long? PlanCode { get; set; }
        public virtual decimal? StartDate { get; set; }
        public virtual long? TransactionCode { get; set; }
    }
}
