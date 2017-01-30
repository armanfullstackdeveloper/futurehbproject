using System;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToPayments;

namespace BusinessLogic.BussinesLogics.RelatedToPayments
{
    public class PaymentRequestStatusBL : GenericRepository<PaymentRequestStatus, byte>
    {
        private IDbConnection _db;
        public byte? GetCodeByName(string name)
        {
            try
            {
                _db = EnsureOpenConnection();
                byte? code = _db.Query<byte>("SELECT Id FROM [dbo].[PaymentRequestStatus] Name=@name", new { name }).SingleOrDefault();
                EnsureCloseConnection(_db);
                return code;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, name);
            }
        }
    }
}
