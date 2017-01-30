namespace DataModel.Entities.RelatedToOrder {

    public class OrderHistory : EntityBase<OrderHistory>
    {
        public string UserCode { get; set; }
        public virtual long OrderCode { get; set; }
        public virtual byte OrderStatusCode { get; set; }
        public virtual int Date { get; set; }
        public virtual int Time { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as OrderHistory;
			if (t == null) return false;
			if (OrderCode == t.OrderCode
			 && OrderStatusCode == t.OrderStatusCode)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ OrderCode.GetHashCode();
			hash = (hash * 397) ^ OrderStatusCode.GetHashCode();

			return hash;
        }
        #endregion
    }
}
