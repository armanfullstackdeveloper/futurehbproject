namespace DataModel.Models.ViewModel
{
    public class JsonResultViewModel
    {
        public bool Success { get; set; }
        public string TrackingCode { get; set; }
        public dynamic Response { get; set; }
    }
}
