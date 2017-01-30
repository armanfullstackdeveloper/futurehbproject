using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Helpers;
using DataModel.Entities.RelatedToPayments;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToPayments
{
    public class HBPaymentToMemberBL : GenericRepository<HBPaymentToMember, long>
    {
        public IEnumerable<HBPaymentToMember> GetAll_OrderByLast(int pageNumer)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var result = session.Query<HBPaymentToMember>().OrderByDescending(p => p.Date)
                        .Skip((pageNumer - 1) * StaticNembericInBL.CountOfItemsInAdminPages)
                        .Take(StaticNembericInBL.CountOfItemsInAdminPages)
                        .ToList();
                    transaction.Commit();
                    return result;
                }
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                    {
                        new ActionInputViewModel()
                        {
                            Name = HelperFunctionInBL.GetVariableName(() => pageNumer),
                            Value = pageNumer.ToString()
                        },
                    };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }
    }
}
