using System;
using System.Data;
using System.Transactions;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToStore;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToStoreBL
{
    public class StoreCustomerBL : DapperConfiguration 
    {
        private readonly IDbConnection _db;
        public StoreCustomerBL()
        {
            _db = EnsureOpenConnection();
        }
        public long Save(StoreCustomer obj)
        {
            try
            {
                using (var txScope = new TransactionScope())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@StoreCode", obj.StoreCode);
                    parameters.Add("@MemberCode", obj.MemberCode);
                    long result = _db.Execute("StoreCustomer_Insert", parameters, commandType: CommandType.StoredProcedure);
                    txScope.Complete();
                    EnsureCloseConnection(_db);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(obj).ToString());
            }
        }

        public void Delete(StoreCustomer obj)
        {
            try
            {
                using (var txScope = new TransactionScope())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@StoreCode", obj.StoreCode);
                    parameters.Add("@MemberCode", obj.MemberCode);
                    _db.Execute("StoreCustomer_Delete", parameters, commandType: CommandType.StoredProcedure);
                    txScope.Complete();
                    EnsureCloseConnection(_db);
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(obj).ToString());
            }
        }

    }
}
