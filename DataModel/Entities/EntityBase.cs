using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DataModel.Models;

namespace DataModel.Entities
{
    public class EntityBase<T> where T : new()
    {
        //public T2 GetAttributeFrom<T2>(object instance, string propertyName) where T2 : Attribute
        //{
        //    var attrType = typeof(T2);
        //    var property = instance.GetType().GetProperty(propertyName);
        //    return (T2)property.GetCustomAttributes(attrType, false).First();
        //}
        //public List<DynamicProductProperty> CreateModelForShow(T obj)
        //{
        //    List<DynamicProductProperty> lst = new List<DynamicProductProperty>();
        //    //PropertyInfo[] properties = typeof(T).GetProperties();
        //    PropertyInfo[] properties = obj.GetType().GetProperties();
        //    foreach (PropertyInfo property in properties)
        //    {
        //        if (property.Name == "Id" || property.Name == "ProductCode")// || property.Name.Contains("Code")
        //            continue;
        //        var temp = property.GetValue(obj, null);

        //        string header;
        //        try
        //        {
        //            header = new EntityBase<T>().GetAttributeFrom<DisplayAttribute>(new T(), property.Name).Name;
        //        }
        //        catch (Exception)
        //        {
        //            header = null;
        //        }
                
        //        DynamicProductProperty dynamicProductProperty = new DynamicProductProperty()
        //        {
        //            Header = header,
        //            Value = (temp != null) ? temp.ToString() : string.Empty,
        //            Name = property.Name
        //        };
        //        lst.Add(dynamicProductProperty);
        //    }
        //    return lst;
        //}

        //public List<DynamicProductAttrbiueProperty> CreateModelForManage(T obj,long subCat2Code)
        //{
        //    List<DynamicProductAttrbiueProperty> lst = new List<DynamicProductAttrbiueProperty>();
        //    PropertyInfo[] properties = obj.GetType().GetProperties();
        //    foreach (PropertyInfo property in properties)
        //    {
        //        if (property.Name.Contains("Code_OT") || property.Name.Contains("_IV"))
        //        {
        //            DynamicProductAttrbiueProperty dynamicProductAttrbiueProperty = new DynamicProductAttrbiueProperty()
        //            {
        //                ColumnName = property.Name,
        //                Header = new EntityBase<T>().GetAttributeFrom<DisplayAttribute>(new T(), property.Name).Name,
        //                SubCat2Code = subCat2Code
        //            };
        //            lst.Add(dynamicProductAttrbiueProperty);
        //        }
        //    }
        //    return lst;
        //}
    }
}
