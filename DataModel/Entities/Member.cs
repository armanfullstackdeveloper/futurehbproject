using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities {

    public class Member : EntityBase<Member>
    {
        public Member() { }
        public virtual long Id { get; set; }

        [StringLength(60)]
        public virtual string Name { get; set; }

        public virtual string UserCode { get; set; }

        [StringLength(150)]
        public virtual string PicAddress { get; set; }
        public virtual long? CityCode { get; set; }

        [StringLength(200)]
        public virtual string Place { get; set; }

        //[RegularExpression("^[0-9]*$")]  [RegularExpression("[0-9]{10}")]
        public virtual string PostalCode { get; set; }

        //[RegularExpression("^[0-9]*$")]
        public virtual string MobileNumber { get; set; }

        //[RegularExpression("^[0-9]*$")]
        public virtual string PhoneNumber { get; set; }

        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }

        public int Balance { get; set; }
    }
}
