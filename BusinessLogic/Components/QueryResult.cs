using System.Collections.Generic;

namespace BusinessLogic.Components
{
    /// <summary>
    /// use this class for manage procedure message with return value or values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T>
    {
        public QueryResult(T obj,List<T> objLst,int messageId)
        {
            Obj = obj;
            ObjList = objLst;
            DbMessage = new DbMessage(messageId);
        }
        public T Obj { get; set; }
        public List<T> ObjList { get; set; }
        public DbMessage DbMessage { get; set; }  
    }
}
