using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;

namespace BusinessLogic.BussinesLogics
{
    public class StateBL : GenericRepository<State, long>
    {
        private IDbConnection _db;

        public List<State> GetAllStatesCities()
        {
            _db = EnsureOpenConnection();
            List<State> lstStates;
            List<City> lstCities;
            var parameters = new DynamicParameters();
            using (var multipleResults = _db.QueryMultiple("State_GetAllStatesCities", parameters, commandType: CommandType.StoredProcedure))
            {
                lstStates = multipleResults.Read<State>().ToList();
                lstCities = multipleResults.Read<City>().ToList();
            }
            foreach (State state in lstStates)
            {
                foreach (City city in lstCities)
                {
                    if (state.Id == city.StateCode)
                    {
                        state.Cities = state.Cities ?? new List<City>();
                        state.Cities.Add(city);
                    }
                }
            }
            EnsureCloseConnection(_db);
            return lstStates;
        }
    }
}
