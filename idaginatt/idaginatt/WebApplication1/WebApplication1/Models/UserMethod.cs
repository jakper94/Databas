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

            String sqlString = "SELECT Us_Password FROM Tbl_User WHERE Us_UserName = @userName";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            SqlDataReader reader = null;
            errormsg = "";
            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();
                string corr_password = reader["Us_Password"].ToString();
                if (corr_password == null)
                {
                    errormsg = "Username does not exist";
                    return false;
                }
                if (corr_password.Equals(password))
                {
                    return true;
                }
                else
                {
                    errormsg = "Incorrect password";
                    return false;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return false;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
