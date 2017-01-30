namespace DataModel.Entities.RelatedToStore {

    public class Plan : EntityBase<Plan>
    {
        public Plan() { }
        public virtual long Id { get; set; }
        //[StringLength(100)]
        public virtual string Name { get; set; }
        public virtual byte? Period { get; set; }
        public virtual byte? FreeProductNumber { get; set; }
        public virtual byte? FreeMonthNumber { get; set; }
        public virtual int? ProductNumber { get; set; }
        public virtual short? MonthNumber { get; set; }
        public virtual bool? HaveWebsite { get; set; }
        public virtual decimal? Cost { get; set; }
        public virtual decimal? DiscountedCost { get; set; }
        public virtual string Comments { get; set; }
    }
}
