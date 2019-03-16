using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AttendingMethod
    {
        public AttendingMethod() {}

        public int InsertAttending(AttendingDetail ad, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "INSERT INTO Tbl_Attending (At_User, At_Foodpref, At_Year) VALUES (@UAttending_User, @Attending_Foodpref, @Attending_Year)";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            dbCommand.Parameters.Add("Attending_User", SqlDbType.NChar, 8).Value = ad.Attending_User;
            dbCommand.Parameters.Add("Attending_Foodpref", SqlDbType.NVarChar).Value = ad.Attending_Foodpref;
            dbCommand.Parameters.Add("Attending_Year", SqlDbType.Int).Value = ad.Attending_Year;


            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Attendee not added to database."; }
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
