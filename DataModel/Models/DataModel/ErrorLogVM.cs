namespace DataModel.Models.DataModel
{
    public class ErrorLogVM
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Input { get; set; }
        public long? ErrorLogCode { get; set; } 
    }
}
