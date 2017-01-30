using System;
using System.Data;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;

namespace BusinessLogic.BussinesLogics
{
    public class MobileAppUserSurveyBL : GenericRepository<MobileAppUserSurvey, long>
    {
        private  IDbConnection _db;
     
        public long? SaveWithDapper(long scoreCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ScoreCode", scoreCode);
                parameters.Add("@InsertedId", dbType: DbType.Int64, direction: ParameterDirection.InputOutput);
                _db.Execute("MobileAppUserSurvey_Save", parameters, commandType: CommandType.StoredProcedure);
                long? procResult = parameters.Get<long>("@InsertedId");
                EnsureCloseConnection(_db);
                return procResult;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, scoreCode.ToString());
            }
        }
    }
}
