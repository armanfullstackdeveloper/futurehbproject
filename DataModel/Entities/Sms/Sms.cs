using DataModel.Enums;

namespace DataModel.Entities.Sms
{
    public class Sms : EntityBase<Sms>
    {
        public virtual long Id { get; set; }
        public string Text { get; set; }
        public virtual ESmsType SmsType { get; set; }
        public virtual long Reciver { get; set; } 
        /// <summary>
        /// RecId
        /// </summary>
        public virtual long TrackingCode { get; set; } 
        public virtual int CreationDate { get; set; }    
        public virtual short CreationTime { get; set; }      
    }
}
