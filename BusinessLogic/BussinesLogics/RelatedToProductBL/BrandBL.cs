using System;
using System.Collections.Generic;
using System.Data;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities.RelatedToProduct;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class BrandBL : GenericRepository<Brand, long>
    {
        private IDbConnection _db;
       
        public List<Brand> GetAllForSpecificSubCat(long subCatCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CatCode", subCatCode);
                IEnumerable<Brand> lstBrand = _db.Query<Brand>("Brand_SelectAllForSpecificCategory", parameters, commandType: CommandType.StoredProcedure);
                EnsureCloseConnection(_db);
                return (List<Brand>)lstBrand;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, subCatCode.ToString());
            }
        }

    }
}
