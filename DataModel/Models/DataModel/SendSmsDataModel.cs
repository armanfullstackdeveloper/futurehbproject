using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models.DataModel
{
    public class SendSmsDataModel
    {
        public string Username { get; set; }
        public string PassWord { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Text { get; set; } 
    }
}
