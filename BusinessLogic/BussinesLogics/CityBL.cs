using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.BussinesLogics
{
    public class CityBL : GenericRepository<City, long>
    {
        private IDbConnection _db;

        public List<City> GetStatesCityThatHaveStore(long? stateCode, long subCat2Code)
        {
            try
            {
                _db = EnsureOpenConnection();
                List<City> lst;
                var parameters = new DynamicParameters();
                parameters.Add("@StateCode", stateCode);
                parameters.Add("@SubCat2Code", subCat2Code);
                using (var multipleResults = _db.QueryMultiple("City_GetValidStateAndCities", parameters, commandType: CommandType.StoredProcedure))
                {
                    lst = multipleResults.Read<City>().ToList();
                }
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => stateCode),
                        Value = stateCode.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => subCat2Code),
                        Value = subCat2Code.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }
    }
}
