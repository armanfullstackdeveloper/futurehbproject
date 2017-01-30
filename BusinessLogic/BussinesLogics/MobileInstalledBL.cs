using System;
using System.Linq;
using BusinessLogic.Helpers;
using DataModel.Entities;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics
{
    public class MobileInstalledBL : GenericRepository<MobileInstalled, long>
    {
        public bool IsInstalledBefore(string uniqeKey)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    var mobile = session.Query<MobileInstalled>().Where(x => x.UniqKey == uniqeKey).ToList();
                    if (mobile.Count != 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, uniqeKey);
            }
        }
    }
}
