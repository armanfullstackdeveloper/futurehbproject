using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities.RelatedToProduct
{
    public class UserProductReport : EntityBase<UserProductReport>
    {
        public long Id { get; set; }
        public long ProductCode { get; set; } 
        public byte ReportCode { get; set; }

        public string ReportText
        {
            get
            {
                if (ReportCode == 1)
                {
                    return "تصویر محصول نامناسب است"; 
                }
                else if (ReportCode == 2)
                {
                    return "توضیحات محصول نامناسب است";
                }
                else if (ReportCode == 3)
                {
                    return "محصول در دسته بندی مناسب قرار نگرفته است";
                }
                return string.Empty;
            }
        }
    }
}
