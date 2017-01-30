using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.Contacts
{

    public class PublicMessage : EntityBase<PublicMessage>
    {
        public virtual long Id { get; set; }
        [StringLength(500)]
        public virtual string Text { get; set; }
        [StringLength(500)]
        public virtual string Link { get; set; }
        public virtual decimal? Date { get; set; }
        public virtual decimal? Time { get; set; }
        public virtual bool IsForStore { get; set; }
    }

    public class PublicMessageViewModel
    {
        public virtual long Id { get; set; }
        [StringLength(500)]
        public virtual string Text { get; set; }
        [StringLength(500)]
        public virtual string Link { get; set; }
    }
}
