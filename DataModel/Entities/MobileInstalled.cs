namespace DataModel.Entities
{
    public class MobileInstalled : EntityBase<MobileInstalled>
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// decimal(20, 17)
        /// </summary>
        public virtual decimal? Latitude { get; set; }
        /// <summary>
        /// decimal(20, 17)
        /// </summary>
        public virtual decimal? Longitude { get; set; }
        public string UniqKey { get; set; }
    }
}
