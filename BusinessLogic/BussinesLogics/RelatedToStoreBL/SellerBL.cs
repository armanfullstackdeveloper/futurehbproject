using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToStore;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.BussinesLogics.RelatedToStoreBL
{
    public class SellerBL : GenericRepository<Seller, long>
    {
        private IDbConnection _db;

        public string GetSellerPhotoAddres(long sellerCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SallerCode", sellerCode);

                string st = _db.Query<string>("Saller_GetPhotoAddress", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return st;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, sellerCode.ToString());
            }
        }

        public string GetPhotoAddresByUserCode(string userCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@userCode", userCode);
                string address = this._db.Query<string>("Saller_SelectPhotoAddresByUserCode", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return address;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userCode);
            }
        }

        public bool EditSallerPhotoAddress(long sallerCode, string rootPath)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@newAddress", rootPath);
                parameters.Add("@SallerCode", sallerCode);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                this._db.Execute("Saller_EditPhotoAddress", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");
                EnsureCloseConnection(_db);
                return (procResult == 111);
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => sallerCode),
                        Value = sallerCode.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => rootPath),
                        Value = rootPath
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }
    }
}
