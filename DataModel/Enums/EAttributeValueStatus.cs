using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Enums
{
    public enum EAttributeValueStatus:byte
    {
        Accepted=0,
        AddBySeller=1,
        AcceptFromSeller=2,
        RejectFromSeller=3
    }
}
