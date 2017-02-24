using DataModel.Enums;

namespace DataModel.Entities {

    public class ErrorLog : EntityBase<ErrorLog>
    {
        public ErrorLog() { }
        public long Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Input { get; set; }
        public long? ErrorLogCode { get; set; } 
        public string UserCode { get; set; }
        public virtual int? Date { get; set; }
        public virtual short? Time { get; set; }
        public virtual ELogBy LogBy { get; set; }
    }
}
