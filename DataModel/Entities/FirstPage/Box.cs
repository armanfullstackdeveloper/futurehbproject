using DataModel.Enums;

namespace DataModel.Entities.FirstPage
{
    public class Box : EntityBase<Box>
    {
        public virtual long Id { get; set; }
        public virtual string ImgAddress { get; set; }
        public virtual int? StartDate { get; set; }
        public virtual int? EndDate { get; set; }
        public virtual short? StartTime { get; set; }
        public virtual short? EndTime { get; set; }
        public virtual string Link { get; set; }
        public virtual int OrderNumber { get; set; } 
        public virtual string Position { get; set; }
        public virtual string Title { get; set; } 
        public virtual string Description { get; set; }
        public virtual bool IsForApp { get; set; } 
    }
}
