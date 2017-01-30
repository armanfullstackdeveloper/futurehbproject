namespace DataModel.Entities.RelatedToProduct {

    public class ProductNonSearchableAttributeValue : EntityBase<ProductNonSearchableAttributeValue>
    {
        public virtual long ProductCode { get; set; }
        public virtual long AttributeCode { get; set; }
        public virtual string Value { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as ProductNonSearchableAttributeValue;
			if (t == null) return false;
			if (ProductCode == t.ProductCode
			 && AttributeCode == t.AttributeCode)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ ProductCode.GetHashCode();
			hash = (hash * 397) ^ AttributeCode.GetHashCode();

			return hash;
        }
        #endregion
    }
}
