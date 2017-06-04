using System.Collections.Generic;

namespace DataModel.Models.DataModel
{
    public class SearchParametersDataModel
    {
        private long? _catCode;
        public long? CategoryCode
        {
            get
            {
                return _catCode == 0 ? null : _catCode;
            }
            set { _catCode = value; }
        }

        private string _name;
        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(_name) ? null : _name;
            }
            set { _name = value; }
        }

        private long? _storeCode;
        public long? StoreCode
        {
            get
            {
                return _storeCode == 0 ? null : _storeCode;
            }
            set { _storeCode = value; }
        }

        private int? _pageNumber;
        public int? PageNumber
        {
            get
            {
                return _pageNumber == 0 ? null : _pageNumber;
            }
            set { _pageNumber = value; }
        }

        private int? _rowsPage;
        public int? RowsPage
        {
            get
            {
                return _rowsPage == 0 ? null : _rowsPage;
            }
            set { _rowsPage = value; }
        }

        public bool? TileShow { get; set; }

        private int? _minPrice;
        public int? MinPrice
        {
            get
            {
                return _minPrice == 0 ? null : _minPrice;
            }
            set { _minPrice = value; }
        }

        private int? _maxPrice;
        public int? MaxPrice
        {
            get
            {
                return _maxPrice == 0 ? null : _maxPrice;
            }
            set { _maxPrice = value; }
        }


        public bool? JustExsisted { get; set; }

        private int? _sortBy;
        public int? SortBy
        {
            get
            {
                return _sortBy == 0 ? null : _sortBy;
            }
            set { _sortBy = value; }
        }


        public bool? Ascending { get; set; }

        private long? _state;
        public long? State
        {
            get
            {
                return _state == 0 ? null : _state;
            }
            set { _state = value; }
        }

        private long? _city;
        public long? City
        {
            get
            {
                return _city == 0 ? null : _city;
            }
            set { _city = value; }
        }


        public List<long> Brands { get; set; }
        public List<long> Colors { get; set; }

        private decimal? _latitude;
        public decimal? Latitude
        {
            get
            {
                return _latitude == 0 ? null : _latitude;
            }
            set { _latitude = value; }
        }

        private decimal? _longitude;
        public decimal? Longitude
        {
            get
            {
                return _longitude == 0 ? null : _longitude;
            }
            set { _longitude = value; }
        }

        public SearchPlace SearchPlace { get; set; }

        public List<ProductAttributeWithItemsDataModel> Attributes { get; set; }
    }

    public class AttributeFilterDataModel
    {
        public long AttributeCode { get; set; }
        public long AttributeValueCode { get; set; }
    }

    public enum SearchPlace
    {
        Non = 0,
        FirstPage = 1,
        SearchPage = 2,
        ProductDetails = 3,
        StoreHomePage = 4
    }
}
