using System;
using System.Text;
using System.Collections.Generic;
using DataModel.Enums;


namespace DataModel.Entities.RelatedToProduct {

    public class AttributeValue : EntityBase<AttributeValue>
    {
        public AttributeValue() { }
        public virtual long Id { get; set; }
        public virtual long AttributeCode { get; set; }
        public virtual string Value { get; set; }
        public virtual EAttributeValueStatus Status { get; set; }
    }
}
