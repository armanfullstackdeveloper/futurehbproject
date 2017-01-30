using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities.RelatedToStore
{
    public class UserStoreReport : EntityBase<UserStoreReport>
    {
        public long Id { get; set; }
        public long StoreCode { get; set; }
        public byte ReportCode { get; set; }

        public string ReportText
        {
            get
            {
                if (ReportCode == 1)
                {
                    return "تصویر فروشگاه نامناسب است";
                }
                else if (ReportCode == 2)
                {
                    return "توضیحات فروشگاه نامناسب است";
                }
                else if (ReportCode == 3)
                {
                    return "فروشگاه در دسته بندی مناسب قرار نگرفته است";
                }
                return string.Empty;
            }
        }
    }
}
