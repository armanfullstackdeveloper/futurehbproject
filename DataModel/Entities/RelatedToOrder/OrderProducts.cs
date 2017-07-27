namespace DataModel.Entities.RelatedToOrder {

    public class OrderProducts : EntityBase<OrderProducts>
    {
        public virtual long OrderCode { get; set; }
        public virtual long ProductCode { get; set; }
        public virtual short Count { get; set; }
        public int CurrentPrice { get; set; }
        public string Color { get; set; }
        public string Size { get; set; } 
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as OrderProducts;
			if (t == null) return false;
			if (OrderCode == t.OrderCode
			 && ProductCode == t.ProductCode)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ OrderCode.GetHashCode();
			hash = (hash * 397) ^ ProductCode.GetHashCode();

			return hash;
        }
        #endregion
    }
}
