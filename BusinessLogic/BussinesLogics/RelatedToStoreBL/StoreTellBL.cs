using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToStore;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToStoreBL
{
    public class StoreTellBL : DapperConfiguration
    {
        private readonly IDbConnection _db;
        public StoreTellBL()
        {
            _db = EnsureOpenConnection();
        }
        public long Save(StoreTell obj)
        {
            try
            {
                using (var txScope = new TransactionScope())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@StoreCode", obj.StoreCode);
                    parameters.Add("@PhoneNumber", obj.PhoneNumber);
                    long result = _db.Execute("StoreTell_Insert", parameters, commandType: CommandType.StoredProcedure);
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

        public void Delete(StoreTell obj)
        {
            try
            {
                using (var txScope = new TransactionScope())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@StoreCode", obj.StoreCode);
                    parameters.Add("@PhoneNumber", obj.PhoneNumber);
                    _db.Execute("StoreTell_Delete", parameters, commandType: CommandType.StoredProcedure);
                    txScope.Complete();
                    EnsureCloseConnection(_db);
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(obj).ToString());
            }
        }

        public List<decimal> GetTellsById(long storeCode)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StoreCode", storeCode);
                List<decimal> lst =
                    _db.Query<decimal>("StoreTell_SelectByStoreCode", parameters,
                        commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }
    }
}
