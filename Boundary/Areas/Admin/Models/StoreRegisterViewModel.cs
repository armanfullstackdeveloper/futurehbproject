using DataModel.Enums;

namespace Boundary.Areas.Admin.Models
{
    public class SimpleStoreRegisterViewModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public EStoreType StoreType { get; set; } 
    }
}