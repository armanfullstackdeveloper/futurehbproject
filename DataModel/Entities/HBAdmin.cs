namespace DataModel.Entities
{
    public class HBAdmin : EntityBase<HBAdmin>
    {
        public long Id { get; set; }
        public string UserCode { get; set; }
        public string Name { get; set; }
        public virtual string ImgAddress { get; set; } 
    }
}
