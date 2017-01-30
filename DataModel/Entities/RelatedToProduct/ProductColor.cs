namespace DataModel.Entities.RelatedToProduct {

    public class ProductColor : EntityBase<ProductColor>
    {
        public virtual long ProductCode { get; set; }
        public virtual long ColorCode { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as ProductColor;
			if (t == null) return false;
			if (ProductCode == t.ProductCode
			 && ColorCode == t.ColorCode)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ ProductCode.GetHashCode();
			hash = (hash * 397) ^ ColorCode.GetHashCode();

			return hash;
        }
        #endregion
    }
}
