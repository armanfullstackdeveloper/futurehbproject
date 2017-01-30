using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.FirstPage;
using DataModel.Models.ViewModel;

namespace BusinessLogic.BussinesLogics.FirstPageBL
{
    public class FirstPage_AdvertiseBL : GenericRepository<FirstPage_Advertise, long>
    {
        private IDbConnection _db;

        public string GetImgAddressById(long sliderId)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", sliderId);
                string item = _db.Query<string>("FirstPage_Advertise_GetImgAddressById", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return item;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, sliderId.ToString());
            }
        }

        public List<FirstPage_AdvertiseViewModel> GetActiveAdvertise() 
        {
            _db = EnsureOpenConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@persianToday", PersianDateTime.Now.Date.ToInt());
            List<FirstPage_AdvertiseViewModel> firstPageSliders = _db.Query<FirstPage_AdvertiseViewModel>("FirstPage_Advertise_GetActiveAdvertise", parameters, commandType: CommandType.StoredProcedure).ToList();
            EnsureCloseConnection(_db);
            return firstPageSliders;
        }
    }
}
