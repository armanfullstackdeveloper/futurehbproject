using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic.Helpers
{
    public class DapperConfiguration
    {
        public IDbConnection EnsureOpenConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["ShopFinderForDapper"].ConnectionString);
        }

        public void EnsureCloseConnection(IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }


    }
}
