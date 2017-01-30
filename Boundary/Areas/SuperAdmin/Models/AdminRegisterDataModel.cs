using DataModel.Entities;
using DataModel.Models.DataModel;

namespace Boundary.Areas.SuperAdmin.Models
{
    public class AdminRegisterDataModel
    {
        public RegisterMemberDataModel RegisterMemberDataModel { get; set; }
        public HBAdmin HbAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}