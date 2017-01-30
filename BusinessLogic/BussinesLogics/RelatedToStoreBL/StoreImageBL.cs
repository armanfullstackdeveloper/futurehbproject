using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToStore;

namespace BusinessLogic.BussinesLogics.RelatedToStoreBL
{
    public class StoreImageBL : GenericRepository<StoreImage,long>
    {
        public List<StoreImage> GetAllByStoreCode(long storeCode) 
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@storeCode", storeCode);

                List<StoreImage> result = db.Query<StoreImage>(@"select * from [dbo].[StoreImage] where 
                        (@storeCode is null or [StoreCode]=@storeCode)", parameters).ToList();
                EnsureCloseConnection(db);
                return result;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, storeCode.ToString());
            }
        }
    }
}
