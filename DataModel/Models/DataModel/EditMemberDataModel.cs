using System.ComponentModel.DataAnnotations;

namespace DataModel.Models.DataModel
{
    public class EditMemberDataModel
    {
        public long Id { get; set; }

        [StringLength(60)]
        public string Name { get; set; }

        [StringLength(60)]
        public string Email { get; set; }

        public long? CityCode { get; set; }

        [StringLength(200)]
        public string Place { get; set; }

        [RegularExpression("^[0-9]*$")]
        public string PostalCode { get; set; }

        [RegularExpression("^[0-9]*$")]
        public string MobileNumber { get; set; }

        [RegularExpression("^[0-9]*$")]
        public string PhoneNumber { get; set; }

        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
    }
}
