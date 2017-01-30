namespace DataModel.Models.ViewModel { 

    public class MemberViewModel 
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string UserCode { get; set; }

        public virtual string PicAddress { get; set; }

        public virtual long? CityCode { get; set; }
        public virtual string City { get; set; } 

        public virtual string Place { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string MobileNumber { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }

        public int Balance { get; set; }
    }
}
