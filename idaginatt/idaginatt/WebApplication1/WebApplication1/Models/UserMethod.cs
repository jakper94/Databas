using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UserMethod
    {
        public UserMethod() {}

        public int InsertUser(UserDetail ud, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "INSERT INTO Tbl_User (Us_UserName, Us_Password) VALUES (@User_UserName, @User_Password)";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            dbCommand.Parameters.Add("User_UserName", SqlDbType.NChar,8).Value = ud.User_UserName;
            dbCommand.Parameters.Add("User_Password", SqlDbType.NChar, 10).Value = ud.User_Password;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "User not added to database."; }
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

        public int LogIn(string userName, string password, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "SELECT FROM Tbl_User WHERE Us_UserName = @userName";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "User not added to database."; }
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
        public  GetPlayeById(int id, out string errormsg)
        {
            //sakapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();
            //Koppling mot SQL  Server
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Team; Integrated Security = True;";

            //Sqlstring för att hämta alla personer
            string sqlstring = "Select * From Tbl_Player Where Pl_Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;
            //skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();


            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "myPlayer");
                int i = 0;
                PlayerDetalj pd = new PlayerDetalj();
                pd.PlayerId = Convert.ToInt16(myDS.Tables["myPlayer"].Rows[i]["Pl_Id"]);
                pd.FirstName = myDS.Tables["myPlayer"].Rows[i]["Pl_FirstName"].ToString();
                pd.LastName = myDS.Tables["myPlayer"].Rows[i]["Pl_LastName"].ToString();
                pd.TeamId = Convert.ToInt16(myDS.Tables["myPlayer"].Rows[i]["Pl_Plays"]);

                errormsg = "";
                return pd;
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
