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

            String sqlString = "INSERT INTO Tbl_User (Us_UserName) VALUES (@User_UserName)";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            dbCommand.Parameters.Add("User_UserName", SqlDbType.NChar,8).Value = ud.User_UserName;

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

        public bool LogIn(string userName, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "SELECT Us_UserName FROM Tbl_User WHERE Us_UserName = '" + userName + "'";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        //    dbCommand.Parameters.Add("userName", SqlDbType.NChar,8).Value = userName;

            SqlDataReader reader = null;
            errormsg = "";
            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();
                reader.Read();
                string obj = reader["Us_UserName"].ToString();
                reader.Close();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                errormsg = "The username does not exist.";
                return false;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public bool AdminLogIn(string userName, string password, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "SELECT Us_Password, Us_IsAdmin FROM Tbl_User WHERE Us_UserName = '" + userName + "'";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            //    dbCommand.Parameters.Add("userName", SqlDbType.NChar,8).Value = userName;

            SqlDataReader reader = null;
            errormsg = "";
            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();
                reader.Read();
                string corr_password = reader["Us_Password"].ToString();
                bool isAdmin = reader["Us_IsAdmin"] as bool? ?? false;
                reader.Close();

                if (isAdmin)
                {
                    if (corr_password.Equals(password))
                    {
                        return true;
                    }
                    errormsg = "The password is incorrect.";
                    return false;
                }
                else
                {
                    errormsg = "You do not have admin access.";
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                errormsg = "The username does not exist.";
                return false;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int UpdateUserInfo(UserDetail ud, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "UPDATE Tbl_User SET Us_FirstName = @firstName, Us_LastName = @lastName, Us_Class = @class WHERE Us_UserName = @userName";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("userName", SqlDbType.NChar, 8).Value = ud.User_UserName;
            dbCommand.Parameters.Add("firstName", SqlDbType.NVarChar, 20).Value = ud.User_FirstName;
            dbCommand.Parameters.Add("lastName", SqlDbType.NVarChar, 50).Value = ud.User_LastName;
            dbCommand.Parameters.Add("class", SqlDbType.NChar, 4).Value = ud.User_Class;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "User not updated in database"; }
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

        public int DeleteUser(string userName, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "DELETE FROM Tbl_User WHERE Us_UserName = @userName";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("userName", SqlDbType.NChar,8).Value = userName;
            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "User not removed from database"; }
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

        public List<UserDetail> SelectUsers(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "SELECT Us_UserName, Us_FirstName, Us_LastName, Us_IsAdmin, Us_Class FROM Tbl_User;";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            SqlDataReader reader = null;

            List<UserDetail> userList = new List<UserDetail>();

            errormsg = ""; 

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    UserDetail user = new UserDetail();

                    user.User_UserName = reader["Us_UserName"].ToString();
                    user.User_FirstName = reader["Us_FirstName"].ToString();
                    user.User_LastName = reader["Us_LastName"].ToString();
                    user.User_Class = reader["Us_Class"].ToString();
                    user.User_IsAdmin = Convert.ToBoolean(reader["Us_IsAdmin"]);

                    userList.Add(user);
                }
                reader.Close();

                return userList; 

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

        public void MakeUserAdmin(string username, string password, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "UPDATE Tbl_User SET Us_IsAdmin = 1, Us_Password = @password WHERE Us_UserName = @username";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("password", SqlDbType.NVarChar, 20).Value = password;
            dbCommand.Parameters.Add("username", SqlDbType.NChar, 8).Value = username;


            errormsg = "";

            try
            {
                dbConnection.Open();
                dbCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void DeleteAdmin(string username, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "UPDATE Tbl_User SET Us_IsAdmin = 0 WHERE Us_UserName = '" + username + "';";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            errormsg = "";

            try
            {
                dbConnection.Open();
                dbCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public UserDetail GetUserByUserName(string username, out string errormsg)
        {
            //sakapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();
            //Koppling mot SQL  Server
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            //Sqlstring för att hämta alla personer
            string sqlstring = "Select * From Tbl_User Where Us_UserName = @user";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("user", SqlDbType.Int).Value = username;
            //skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();


            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "MyUser");
                int i = 0;
                UserDetail ud = new UserDetail();
                ud.User_UserName = myDS.Tables["MyUser"].Rows[i]["Us_UserName"].ToString();
                ud.User_FirstName = myDS.Tables["MyUser"].Rows[i]["Us_FirstName"].ToString();
                ud.User_LastName = myDS.Tables["MyUser"].Rows[i]["Us_LastName"].ToString();
                ud.User_HasVoted = Convert.ToBoolean(myDS.Tables["MyUser"].Rows[i]["Us_HasVoted"]);
             
                errormsg = "";
                return ud;
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
        public int SetHasVotedToTrue(string userNameId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "UPDATE Tbl_User SET Us_HasVoted = '1' WHERE Us_UserName = @userName";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("userName", SqlDbType.NChar, 8).Value = userNameId;
          

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "User not updated in database"; }
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
