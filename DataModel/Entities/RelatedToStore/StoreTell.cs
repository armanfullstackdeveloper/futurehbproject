using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities.RelatedToStore
{
    public class StoreTell : EntityBase<StoreTell>
    {
        public long StoreCode { get; set; }
        public decimal PhoneNumber { get; set; }
    }
}
