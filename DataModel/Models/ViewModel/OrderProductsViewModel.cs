namespace DataModel.Models.ViewModel
{
    public class OrderProductsViewModel
    {
        public long ProductCode { get; set; }
        public string ProductSummery { get; set; } //شرح کالا
        public short Count { get; set; }
        public int UnitPrice { get; set; } //قیمت واحد
        public string Color { get; set; }
        public string Size { get; set; }

        public int OverallPrice //قیمت کل
        {
            get { return Count * UnitPrice; }
        }
    }
}
