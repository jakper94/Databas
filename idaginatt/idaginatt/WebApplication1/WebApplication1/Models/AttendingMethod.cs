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
        public AttendingMethod() { }

        public int InsertAttending(AttendingDetail ad, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "INSERT INTO Tbl_Attending (At_User, At_Foodpref, At_Year) VALUES (@Attending_User, @Attending_Foodpref, @Attending_Year)";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            dbCommand.Parameters.Add("Attending_User", SqlDbType.NChar, 8).Value = ad.Attending_User;
            string foodpref;
            if(ad.Attending_Foodpref == null)
            {
                foodpref = "";
            } else
            {
                foodpref = ad.Attending_Foodpref;
            }
            dbCommand.Parameters.Add("Attending_Foodpref", SqlDbType.NVarChar).Value = foodpref;
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

        public List<AttendingDetail> GetAttendingList(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            string sqlstring = "Select At_User, Us_Firstname, Us_Lastname, Us_Class, At_Foodpref, At_Year From Tbl_Attending, Tbl_User Where Tbl_User.Us_UserName = Tbl_Attending.At_User";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataReader reader = null;

            List<AttendingDetail> AttendingList = new List<AttendingDetail>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    AttendingDetail Attending = new AttendingDetail();
                    Attending.Attending_User = reader["At_User"].ToString();
                    Attending.Attending_Firstname = reader["Us_FirstName"].ToString();
                    Attending.Attending_Lastname = reader["Us_LastName"].ToString();
                    Attending.Attending_Class = reader["Us_Class"].ToString();
                    Attending.Attending_Foodpref = reader["At_Foodpref"].ToString();
                    Attending.Attending_Year = Convert.ToInt16(reader["At_Year"]);

                    AttendingList.Add(Attending);
                }
                return AttendingList;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        
    }
}