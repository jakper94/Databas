using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class NomineeMethod
    {
        public NomineeMethod() { }
        public int InsertNominee(NomineeDetail nd, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            string sqlstring = "INSERT INTO Tbl_Nominee (Nom_FirstName,Nom_LastName,Nom_ImgLink) VALUES (@firstName,@lastName,@imgLink)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("firstName", SqlDbType.NVarChar, 30).Value = nd.Nominee_FirstName;
            dbCommand.Parameters.Add("lastName", SqlDbType.NVarChar, 30).Value = nd.Nominee_LastName;
            dbCommand.Parameters.Add("imgLink", SqlDbType.Int).Value = nd.Nominee_ImgLink;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas inte en nominerad i databasen"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }
       
    }
}
