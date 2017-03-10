using System.Collections.Generic;
using DataModel.Enums;

namespace DataModel.Models.ViewModel
{
    public class StoreDetailsViewModel
    {
        public  long Id { get; set; }

        //نام فروشگاه
        public  string Name { get; set; }

        //توضیحات فروشگاه
        public  string Comments { get; set; }

        //نام فروشنده
        public  string SallerName { get; set; }


        //نام شهر
        public  string CityName { get; set; }

        //آدرس کامل
        public  string Place { get; set; }

        public  decimal? Latitude { get; set; }
        public  decimal? Longitude { get; set; }

        public  string LogoAddress { get; set; }
        public string ImgAddress { get; set; }

        //نوع فروشگاه
        public  string StoreTypeName { get; set; }
        public string HomePage { get; set; } 
        public  string Website { get; set; }

        public List<string> Tells { get; set; }
        public List<string> Images { get; set; }
        public List<string> Categories { get; set; }

        public EStoreStatus StoreStatus { get; set; }
    }
}
