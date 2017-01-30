namespace DataModel.Entities.FirstPage
{
    public class FirstPage_Slider : EntityBase<FirstPage_Slider>
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string ImgAddress { get; set; }
        public virtual int? ImgNumber { get; set; }
        public virtual int? StartDate { get; set; }
        public virtual int? EndDate { get; set; }
        public virtual short? StartTime { get; set; }
        public virtual short? EndTime { get; set; }
        public virtual string Link { get; set; }
    }

    public class FirstPage_SliderDataModel
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string ImgAddress { get; set; }
        public virtual int? ImgNumber { get; set; }
        public virtual string Link { get; set; }
    }
}
