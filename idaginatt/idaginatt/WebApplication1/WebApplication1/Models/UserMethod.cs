﻿using System;
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

        public bool LogIn(string userName, string password, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Idag_Inatt;Integrated Security=True";

            String sqlString = "SELECT Us_Password FROM Tbl_User WHERE Us_UserName = '" + userName + "'";
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
                reader.Close();
                Console.WriteLine(corr_password);
                Console.WriteLine(password);
    
                if (corr_password.Equals(password))
                {
                    return true;
                }
                else
                {
                    errormsg = "The password is incorrect.";
                    return false;
                }
            }
            catch (Exception e)
            {
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
            string sqlstring = "UPDATE Tbl_User SET Us_FirstName = @firstName , Us_LastName = @lastName , Us_Class = @class, WHERE Us_UserName = @userName";
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
    }
}
