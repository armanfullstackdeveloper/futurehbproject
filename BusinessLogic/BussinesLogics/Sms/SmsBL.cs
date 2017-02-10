using System;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Enums;

namespace BusinessLogic.BussinesLogics.Sms
{
    public class SmsBL : GenericRepository<DataModel.Entities.Sms.Sms, long>
    {
        public int TodayAttempt(long phoneNumber, ESmsType type)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@phoneNumber", phoneNumber);
                parameters.Add("@smsType", type);
                int result = db.Query<int>("Sms_TodayAttempt", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(db);
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, phoneNumber.ToString());
            }
        }

        public string VerificationCode(long phoneNumber)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@phoneNumber", phoneNumber);
                string result = db.Query<string>("Sms_GetVerificationCodeByPhoneNumber", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(db);
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, phoneNumber.ToString());
            }
        }
    }
}
