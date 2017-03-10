using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.FirstPage;

namespace BusinessLogic.BussinesLogics.FirstPageBL
{
    public class BoxBL : GenericRepository<Box, long>
    {
        private IDbConnection _db;

        public string GetImgAddressById(long boxId)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", boxId);
                string item = _db.Query<string>("Box_GetImgAddressById", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return item;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, boxId.ToString());
            }
        }

        public IEnumerable<Box> GetActiveBox(string position)  
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@persianToday", PersianDateTime.Now.Date.ToInt());
                parameters.Add("@position", position);
                IEnumerable<Box> result = _db.QueryAsync<Box>("Box_GetActive", parameters,
                    commandType: CommandType.StoredProcedure).Result;
                EnsureCloseConnection(_db);
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, position);
            }
        }
    }
}
