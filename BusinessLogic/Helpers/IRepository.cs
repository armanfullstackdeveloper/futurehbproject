using System.Collections.Generic;
using DataModel.Entities;

namespace BusinessLogic.Helpers
{
    public interface IRepository<T, TPkT> where T : EntityBase<T>, new()
    {
        TPkT Insert(T pModel);
        bool Update(T pModel);
        bool Delete(TPkT pId);
        IList<T> SelectAll();
        T SelectOne(TPkT pId);
    }
}
