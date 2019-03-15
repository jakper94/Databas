using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class VoteMethod
    {
        public VoteMethod() { }

        public int InsertVote(VoteDetail vd, int nomineeId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            string sqlstring = "INSERT INTO Tbl_Vote (Vo_Motivation,Vo_VotedOn) VALUES (@motivation,@votedOn)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("motivation", SqlDbType.NVarChar, 100).Value = vd.Vote_Motivation;
            dbCommand.Parameters.Add("votedOn", SqlDbType.Int).Value = nomineeId;
            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas inte en röst i databasen"; }
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
        public List<VoteDetail> GetVoteList(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Idag_Inatt; Integrated Security = True;";

            string sqlstring = "Select * From Tbl_Vote";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataReader reader = null;

            List<VoteDetail> VoteList = new List<VoteDetail>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    VoteDetail vote = new VoteDetail();
                    vote.Vote_Id = Convert.ToInt16(reader["Vo_Id"]);
                    vote.Vote_Motivation = reader["Vo_Motivation"].ToString();
                    vote.Vote_Nominee = Convert.ToInt16(reader["Vo_VotedOn"]);
                    VoteList.Add(vote);
                }
                return VoteList;
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
