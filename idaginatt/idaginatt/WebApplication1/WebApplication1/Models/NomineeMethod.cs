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
            
            string sqlstring = "INSERT INTO Tbl_Nominee (Nom_FirstName,Nom_LastName,Nom_ImgLink,Nom_Votes,Nom_Year) VALUES (@firstName,@lastName,@imgLink,@votes,@year)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("firstName", SqlDbType.NVarChar, 30).Value = nd.Nominee_FirstName;
            dbCommand.Parameters.Add("lastName", SqlDbType.NVarChar, 30).Value = nd.Nominee_LastName;
            dbCommand.Parameters.Add("imgLink", SqlDbType.Int).Value = nd.Nominee_ImgLink;
            dbCommand.Parameters.Add("votes", SqlDbType.Int).Value = 0;
            dbCommand.Parameters.Add("year", SqlDbType.Int).Value = nd.Nominee_Year;

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
        public List<NomineeDetail> GetNomineeList(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            string sqlstring = "Select * From Tbl_Nominee";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataReader reader = null;

            List<NomineeDetail> NomineeList = new List<NomineeDetail>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    NomineeDetail Nominee = new NomineeDetail();
                    Nominee.Nominee_Id = Convert.ToInt16(reader["Nom_Id"]);
                    Nominee.Nominee_FirstName = reader["Nom_FirstName"].ToString();
                    Nominee.Nominee_LastName = reader["Nom_LastName"].ToString();
                    Nominee.Nominee_ImgLink = reader["Nom_ImgLink"].ToString();
                    Nominee.Nominee_Year = Convert.ToInt16(reader["Nom_Year"]);

                    NomineeList.Add(Nominee);
                }
                return NomineeList;
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
        public NomineeDetail GetNomineeById(int id, out string errormsg)
        {
            //sakapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();
            //Koppling mot SQL  Server
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            //Sqlstring för att hämta alla personer
            string sqlstring = "Select * From Tbl_Nominee Where Nom_Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;
            //skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();


            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "MyNominee");
                int i = 0;
                NomineeDetail nd = new NomineeDetail();
                nd.Nominee_Id = Convert.ToInt16(myDS.Tables["MyNominee"].Rows[i]["Nom_Id"]);
                nd.Nominee_FirstName = myDS.Tables["MyNominee"].Rows[i]["Nom_FirstName"].ToString();
                nd.Nominee_LastName = myDS.Tables["MyNominee"].Rows[i]["Nom_LastName"].ToString();
                nd.Nominee_Votes = Convert.ToInt16(myDS.Tables["MyNominee"].Rows[i]["Nom_Votes"]);
                nd.Nominee_ImgLink =myDS.Tables["MyNominee"].Rows[i]["Nom_ImgLink"].ToString();



                errormsg = "";
                return nd;
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
        public int UpdateNominee(NomineeDetail nd, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "UPDATE Tbl_Nominee SET Nom_FirstName= @firstName , Nom_LastName= @lastName , Nom_Votes = @votes, Nom_ImgLink = @imgLink WHERE Nom_Id = @nomineeId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("nomineeId", SqlDbType.Int).Value = nd.Nominee_Id;
            dbCommand.Parameters.Add("firstName", SqlDbType.NVarChar, 30).Value = nd.Nominee_FirstName;
            dbCommand.Parameters.Add("lastName", SqlDbType.NVarChar, 30).Value = nd.Nominee_LastName;
            dbCommand.Parameters.Add("votes", SqlDbType.Int).Value = nd.Nominee_Votes;
            dbCommand.Parameters.Add("imgLink", SqlDbType.NVarChar, 30).Value = nd.Nominee_ImgLink;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det gick inte att uppdatera en nominee i databasen"; }
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
        public int DeleteNominee(int id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";
            string sqlstring = "DELETE FROM Tbl_Nominee WHERE Nom_Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;
            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det raderas inte en spelare i databasen"; }
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
        public List<NomineeDetail> GetNomineeListByYear(int year,out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            string sqlstring = "Select * From Tbl_Nominee Where Nom_Year = @year";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("year", SqlDbType.Int).Value = year;

            SqlDataReader reader = null;

            List<NomineeDetail> NomineeList = new List<NomineeDetail>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    NomineeDetail Nominee = new NomineeDetail();
                    Nominee.Nominee_Id = Convert.ToInt16(reader["Nom_Id"]);
                    Nominee.Nominee_FirstName = reader["Nom_FirstName"].ToString();
                    Nominee.Nominee_LastName = reader["Nom_LastName"].ToString();
                    Nominee.Nominee_ImgLink = reader["Nom_ImgLink"].ToString();
                    Nominee.Nominee_Year = Convert.ToInt16(reader["Nom_Year"]);

                    NomineeList.Add(Nominee);
                }
                return NomineeList;
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
