namespace DataModel.Entities.RelatedToProduct {

    public class ProductSearchableAttributeValue : EntityBase<ProductSearchableAttributeValue>
    {
        public virtual long ProductCode { get; set; }
        public virtual long AttributeCode { get; set; }
        public virtual long AttributeValueCode { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as ProductSearchableAttributeValue;
            if (t == null) return false;
            if (ProductCode == t.ProductCode
             && AttributeCode == t.AttributeCode
             && AttributeValueCode == t.AttributeValueCode)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ ProductCode.GetHashCode();
            hash = (hash * 397) ^ AttributeCode.GetHashCode();
            hash = (hash * 397) ^ AttributeValueCode.GetHashCode();

            return hash;
        }
        #endregion
    }
}
