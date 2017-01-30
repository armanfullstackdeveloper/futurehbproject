namespace Boundary.Areas.SuperAdmin.Models
{
    public class AdminViewModel
    {
        public long Id { get; set; }
        public string UserCode { get; set; }
        public string Name { get; set; }
        public virtual string ImgAddress { get; set; }
        public string RoleName { get; set; }
    }
}