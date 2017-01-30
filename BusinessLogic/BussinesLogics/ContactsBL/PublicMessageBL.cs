using System;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.Contacts;

namespace BusinessLogic.BussinesLogics.ContactsBL
{
    public class PublicMessageBL : GenericRepository<PublicMessage, long>
    {
        private IDbConnection _db;
        public PublicMessageViewModel GetNewest(bool isForStore)
        {
            try
            {
                _db = EnsureOpenConnection();
                PublicMessageViewModel message;
                var parameters = new DynamicParameters();
                parameters.Add("@isForStore", isForStore);
                message = _db.Query<PublicMessageViewModel>("PublicMessage_GetNewest", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return message;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, isForStore.ToString());
            }
        }
    }
}
