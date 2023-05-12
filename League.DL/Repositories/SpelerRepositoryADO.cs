using League.BL.DTO;
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
    public class SpelerRepositoryADO : ISpelerRepository
    {
        private string connectieString;

        public SpelerRepositoryADO(string connectieString)
        {
            this.connectieString = connectieString;
        }
        public bool BestaatSpeler(Speler s)
        {
            string sql = "select count(*) from dbo.speler where naam=@naam";
            using(SqlConnection conn=new SqlConnection(connectieString))
            using(SqlCommand cmd=conn.CreateCommand())
            {                
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@naam", s.Naam);
                    int n=(int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch(Exception ex)
                {
                    throw new SpelerRepositoryException("bestaatspeler", ex);
                }
            }
        }
        public bool BestaatSpeler(int id)
        {
            string sql = "select count(*) from dbo.speler where id=@id";
            using (SqlConnection conn = new SqlConnection(connectieString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryException("bestaatspeler", ex);
                }
            }
        }
        public Speler SchrijfSpelerInDB(Speler s)
        {
            string sql = "insert into dbo.speler(naam,lengte,gewicht) output INSERTED.ID VALUES(@naam,@lengte,@gewicht)";
            using (SqlConnection conn = new SqlConnection(connectieString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@naam", s.Naam);
                    if (s.Lengte == null) cmd.Parameters.AddWithValue("@lengte", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@lengte", s.Lengte);
                    if (s.Gewicht == null) cmd.Parameters.AddWithValue("@gewicht", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@gewicht", s.Gewicht);
                    int newID=(int)cmd.ExecuteScalar();
                    s.ZetId(newID); return s;
                }
                catch (Exception ex)
                {
                    throw new SpelerRepositoryException("bestaatspeler", ex);
                }
            }
        }
        public Speler SelecteerSpeler(int id)
        {
            string sql = "select ts.id spelerid,ts.naam spelernaam,ts.rugnummer spelerrugnummer,ts.lengte spelerlengte,ts.gewicht spelergewicht,ts.teamid spelerteamid,tt.* from speler ts left join (select t1.stamnummer,t1.naam ploegnaam,t1.bijnaam,t2.* from team t1 left join speler t2 on t1.stamnummer=t2.teamid) tt on tt.stamnummer=ts.teamid where ts.id=@id";
            using(SqlConnection connection=new SqlConnection(connectieString))
            using(SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", id);
                    Team team = null;
                    Speler speler = null;
                    IDataReader reader= cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (speler== null)
                        {
                            int? lengte = null;
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("spelerlengte"))) lengte = (int?)reader["spelerlengte"];
                            if (!reader.IsDBNull(reader.GetOrdinal("spelergewicht"))) gewicht = (int?)reader["spelergewicht"];
                            speler = new Speler(id, (string)reader["spelernaam"], lengte, gewicht);
                            if (!reader.IsDBNull(reader.GetOrdinal("spelerrugnummer")))
                                speler.ZetRugnummer((int)reader["rugnummer"]);
                            if (reader.IsDBNull(reader.GetOrdinal("spelerteamid"))) return speler;
                        }
                        if (team==null)
                        {
                            string naam = (string)reader["ploegnaam"];
                            string bijnaam = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("bijnaam"))) bijnaam = (string)reader["bijnaam"];
                            team = new Team((int)reader["stamnummer"], naam);
                            if (bijnaam!=null) { team.ZetBijnaam(bijnaam); }
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        {
                            int? lengte = null;
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int?)reader["lengte"];
                            if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int?)reader["gewicht"];
                            int sid = (int)reader["id"];
                            Speler s = new Speler(sid, (string)reader["naam"], lengte, gewicht);
                            s.ZetTeam(team);
                            if (!reader.IsDBNull(reader.GetOrdinal("rugnummer")))
                                speler.ZetRugnummer((int)reader["rugnummer"]);
                            if (sid == id) speler = s;
                        }
                    }
                    return speler;
                }
                catch(Exception ex) { throw new SpelerRepositoryException("selecteerspeler", ex); }
            }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            string sql = "SELECT t1.id,t1.naam,t1.rugnummer,t1.gewicht,t1.lengte,  case when t2.stamnummer is null then null  else concat(t2.naam,' (',t2.bijnaam,') - ',t2.stamnummer)  end teamnaam  FROM speler t1 left join team t2 on t1.teamid=t2.stamnummer ";
            if (id.HasValue) sql += " WHERE t1.id=@id";
            else sql += " WHERE t1.naam=@naam";
            List<SpelerInfo> spelers= new List<SpelerInfo>();
            using(SqlConnection connection=new SqlConnection(connectieString))
            using(SqlCommand command= connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = sql;
                    if (id.HasValue)
                    {
                        command.Parameters.AddWithValue("@id", id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@naam", naam);
                    }
                    IDataReader reader=command.ExecuteReader();
                    while (reader.Read())
                    {
                        string teamnaam = null;
                        int? lengte = null;
                        int? gewicht = null;
                        int? rugnummer= null;
                        if (!reader.IsDBNull(reader.GetOrdinal("teamnaam"))) { teamnaam = (string)reader["teamnaam"]; }
                        if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) { lengte= (int)reader["lengte"]; }
                        if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) { gewicht = (int)reader["gewicht"]; }
                        if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) { rugnummer = (int)reader["rugnummer"]; }
                        SpelerInfo speler=new SpelerInfo((int)reader["id"],(string)reader["naam"],rugnummer,gewicht,lengte,teamnaam);
                        spelers.Add(speler);
                    }
                    reader.Close();
                    return spelers;
                }
                catch(Exception ex) { throw new SpelerRepositoryException("SelecteerSpelers"); }
            }
        }
        public void UpdateSpeler(Speler speler)
        {
            string sql = "UPDATE speler SET naam=@naam, rugnummer=@rugnummer,lengte=@lengte,gewicht=@gewicht WHERE id=@id";
            using(SqlConnection connection= new SqlConnection(connectieString))
            using(SqlCommand command= connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@id", speler.Id);
                    command.Parameters.AddWithValue("@naam", speler.Naam);
                    if (speler.Rugnummer == null)
                        command.Parameters.AddWithValue("@rugnummer", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@rugnummer", speler.Rugnummer);
                    if (speler.Lengte == null)
                        command.Parameters.AddWithValue("@lengte", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@lengte", speler.Lengte);
                    if (speler.Gewicht == null)
                        command.Parameters.AddWithValue("@gewicht", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@gewicht", speler.Gewicht);
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new SpelerRepositoryException("updatespeler", ex);
                }
            }
        }
    }
}
