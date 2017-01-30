using System;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;

namespace BusinessLogic.BussinesLogics
{
    public class HBAdminBL : GenericRepository<HBAdmin,long>
    {
        private IDbConnection _db;

        public HBAdmin GetSummaryForSession(string userCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                HBAdmin item = _db.Query<HBAdmin>("select Id,Name,ImgAddress from HBAdmin where UserCode=@UserCode", parameters).SingleOrDefault();
                EnsureCloseConnection(_db);
                return item;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userCode);
            }
        }

    }
}
