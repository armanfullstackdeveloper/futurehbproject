using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToProduct;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class AttributeValueBL : GenericRepository<AttributeValue, long>
    {
        public List<AttributeValue> GetByAttributeCode(long attributeCode)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    List<AttributeValue> result =
                        session.Query<AttributeValue>().Where(a => a.AttributeCode == attributeCode).ToList();
                    transaction.Commit();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, attributeCode.ToString());
            }
        }
    }
}
