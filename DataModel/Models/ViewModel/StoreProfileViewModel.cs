namespace DataModel.Models.ViewModel
{
    public class StoreProfileViewModel
    {

        #region for store

        public long StoreId { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string StorePicture { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }

        public string Tell { get; set; } 

        #endregion



        #region for saller

        public string SallerName { get; set; }
        public string SallerPicture { get; set; }

        #endregion

    }
}
