namespace DataModel.Entities.RelatedToStore {

    public class CatsOfStore : EntityBase<CatsOfStore>
    {
        public virtual long CatCode { get; set; }
        public virtual long StoreCode { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as CatsOfStore;
            if (t == null) return false;
            if (CatCode == t.CatCode
             && StoreCode == t.StoreCode)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ CatCode.GetHashCode();
            hash = (hash * 397) ^ StoreCode.GetHashCode();

            return hash;
        }
        #endregion
    }
}
