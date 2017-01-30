using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Models.ViewModel;
using Attribute = DataModel.Entities.RelatedToProduct.Attribute;

namespace BusinessLogic.BussinesLogics.RelatedToProductBL
{
    public class AttributeBL : GenericRepository<Attribute, long>
    {
        private IDbConnection _db;
      
        public List<Attribute> GetByCategoryCode(long? catCode)
        {
            try
            {
                _db = EnsureOpenConnection();
                if (catCode == null)
                    return null;
                List<Attribute> lst = _db.Query<Attribute>("SELECT * FROM dbo.Attribute_GetByCategoryCode(@catCode)", new { catCode }).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, catCode.ToString());
            }
        }

        public List<AttributeViewModel> GetByCategoryCodeForShow(long? catCode) 
        {
            try
            {
                _db = EnsureOpenConnection();                
                List<AttributeViewModel> lst = _db.Query<AttributeViewModel>("Attribute_GetAllForShow", new { catCode }, commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                return lst;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, catCode.ToString());
            }
        }
    }
}
