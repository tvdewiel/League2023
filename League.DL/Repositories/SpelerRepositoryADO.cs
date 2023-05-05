using League.BL.Interfaces;
using League.BL.Model;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
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
    }
}
