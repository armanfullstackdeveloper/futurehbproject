using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Helpers;
using DataModel.Entities;

namespace BusinessLogic.BussinesLogics
{
    public class RoleBL : GenericRepository<Role, int>
    {
        public Role GetByName(string name)
        {
            try
            {
                List<Role> lstRole = (List<Role>)new RoleBL().SelectAll();
                return lstRole.SingleOrDefault(r => r.Name == name);
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, name);
            }
        }
    }
}
