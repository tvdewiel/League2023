using League.BL.Interfaces;
using League.BL.Model;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Repositories
{
    public class TeamRepositoryADO : ITeamRepository
    {
        private string connectionString;
        public TeamRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatTeam(Team t)
        {
            string sql = "select count(*) from team where stamnummer=@stamnummer";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@stamnummer", t.Stamnummer);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex) { throw new TeamRepositoryException("BestaatTeam", ex); }
            }
        }
        public void SchrijfTeamInDB(Team t)
        {
            string sql = "insert into team(stamnummer,naam,bijnaam) values(@stamnummer,@naam,@bijnaam)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@stamnummer", t.Stamnummer);
                    cmd.Parameters.AddWithValue("@naam", t.Naam);
                    if (t.Bijnaam==null)
                        cmd.Parameters.AddWithValue("@bijnaam",DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@bijnaam", t.Bijnaam);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new TeamRepositoryException("BestaatTeam", ex); }
            }
        }
        public Team SelecteerTeam(int stamnummer)
        {
            string sql = "select stamnummer,t1.naam as ploegnaam,t1.bijnaam,t2.* from team t1 left join speler t2 on t1.stamnummer=t2.teamid where t1.stamnummer=@stamnummer";
            using(SqlConnection connection=new SqlConnection(connectionString))
            using(SqlCommand command= connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@stamnummer", stamnummer);
                    IDataReader reader=command.ExecuteReader();
                    Team team = null;                   
                    while(reader.Read())
                    {
                        if (team == null)
                        {
                            string ploegnaam = (string)reader["ploegnaam"];
                            string bijnaam = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam=(string)reader["bijnaam"];      
                            team = new Team(stamnummer, ploegnaam);
                            if (!string.IsNullOrWhiteSpace(bijnaam)) team.ZetBijnaam(bijnaam);
                        }
                        //if (reader["id"]!=null)
                        if (!reader.IsDBNull(reader.GetOrdinal("id"))) 
                        {
                            string naam = (string)reader["naam"];
                            int id = (int)reader["id"];
                            int? rugnummer = null;
                            int? gewicht = null;
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) rugnummer = (int?)reader["rugnummer"];
                            if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                            if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                            Speler speler = new Speler(id, naam, lengte, gewicht);
                            if (rugnummer != null) speler.ZetRugnummer((int)rugnummer);
                            speler.ZetTeam(team);
                        }
                    }
                    reader.Close();
                    return team;
                }
                catch(Exception ex)
                {
                    throw new TeamRepositoryException("SelecteerTeam",ex);
                }
            }
        }
    }
}
