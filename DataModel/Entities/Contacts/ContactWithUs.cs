using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.Contacts {

    public class ContactWithUs : EntityBase<ContactWithUs>
    {
        public virtual long Id { get; set; }
        [StringLength(500)]
        public virtual string Text { get; set; }
        [StringLength(50)]
        public virtual string SenderName { get; set; }
        [StringLength(100)]
        public virtual string Email { get; set; }
        public virtual decimal? Date { get; set; }
        public virtual decimal? Time { get; set; }
    }
}
