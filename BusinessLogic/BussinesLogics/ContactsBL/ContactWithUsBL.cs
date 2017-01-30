using BusinessLogic.Helpers;
using DataModel.Entities.Contacts;

namespace BusinessLogic.BussinesLogics.ContactsBL
{
    public class ContactWithUsBL : GenericRepository<ContactWithUs, long>
    {

        //todo: niyaz be zakhire to database nist hamon be email bere kafiye
        //public long? SaveWithDapper(ContactWithUs contactCustomersWithAdmin)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Text", contactCustomersWithAdmin.Text);
        //    parameters.Add("@SenderName", contactCustomersWithAdmin.SenderName);
        //    parameters.Add("@Email", contactCustomersWithAdmin.Email);
        //    parameters.Add("@InsertedId", dbType: DbType.Int64, direction: ParameterDirection.InputOutput);
        //    this.DbConnection.Execute("ContactWithUs_Save", parameters, commandType: CommandType.StoredProcedure);
        //    var procResult = parameters.Get<long>("@InsertedId");
        //    EnsureCloseConnection();
        //    return procResult;
        //}

    }
}
