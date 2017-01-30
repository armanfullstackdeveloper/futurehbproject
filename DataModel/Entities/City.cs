using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities {

    public class City : EntityBase<City>
    {
        public City() { }
        public virtual long Id { get; set; }
        public virtual long? StateCode { get; set; }
        [StringLength(100)]
        public virtual string Name { get; set; }
    }
}
