using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities.RelatedToProduct
{
    public class Color
    {
        public Color() { } 
        public virtual long Id { get; set; }
        public virtual string ColorTitle { get; set; }
        public string ColorHex { get; set; }
    }
}
