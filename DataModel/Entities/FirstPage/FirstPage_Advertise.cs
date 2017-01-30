namespace DataModel.Entities.FirstPage
{
    public class FirstPage_Advertise : EntityBase<FirstPage_Advertise>
    {
        public virtual long Id { get; set; }
        public virtual string ImgAddress { get; set; }
        public virtual int? StartDate { get; set; }
        public virtual int? EndDate { get; set; }
        public virtual short? StartTime { get; set; }
        public virtual short? EndTime { get; set; }
        public virtual string Link { get; set; }
        
        /// <summary>
        /// یه سری مکان ها مشخص می کنیم و  بعد با استفاده از کد مکانشون عکس هاشونو تو جای مناسب قرار میدیم
        /// </summary>
        public virtual string PlaceCode { get; set; }
    }
}
