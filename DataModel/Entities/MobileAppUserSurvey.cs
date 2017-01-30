using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    public class MobileAppUserSurvey : EntityBase<MobileAppUserSurvey>
    {
        public int Id { get; set; }
        public byte ScoreCode { get; set; } 
        public decimal? Date { get; set; }

        public string ScoreText
        {
            get
            {
                if (ScoreCode == 1)
                {
                    return "عالی";
                }
                else if (ScoreCode == 2)
                {
                    return "خوب";
                }
                else if (ScoreCode == 3)
                {
                    return "متوسط";
                }
                else if (ScoreCode == 4)
                {
                    return "ضعیف";
                }
                return string.Empty;
            }
        }

    }
}
