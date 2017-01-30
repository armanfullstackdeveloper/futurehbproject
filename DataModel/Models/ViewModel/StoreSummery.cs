namespace DataModel.Models.ViewModel
{
    public class StoreSummery
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }

        private string _cityName;
        public string CityName {
            get
            {
                if (_cityName != null)
                    return _cityName;
                return "-";
            }
            set { _cityName = value; }
        }

        public virtual string Place { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
        public virtual decimal? PhoneNumber { get; set; }
        public virtual string LogoAddress { get; set; }
        public virtual string StoreType { get; set; }
    }
}
