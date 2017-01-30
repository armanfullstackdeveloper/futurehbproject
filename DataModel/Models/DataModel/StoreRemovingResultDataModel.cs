using System.Collections.Generic;

namespace DataModel.Models.DataModel
{
    public class StoreRemovingResultDataModel
    {
        /// <summary>
        /// عکس هایی که باید حذف شوند
        /// </summary>
        public List<string> ImageAddressList { get; set; } 
        public bool Success { get; set; }
    }
}
