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
    public class TransferRepositoryADO : ITransferRepository
    {
        private string connectionString;
        public TransferRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public Transfer SchrijfTransferInDB(Transfer transfer)
        {
            string sqlTransfer = "INSERT INTO transfer(spelerid,prijs,oudteamid,nieuwteamid) output INSERTED.ID VALUES(@spelerid,@prijs,@oudteamid,@nieuwteamid)";
            string sqlSpeler = "UPDATE speler SET teamid=@teamid WHERE id=@id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (SqlCommand commandSpeler = connection.CreateCommand())
                using (SqlCommand commandTransfer = connection.CreateCommand())
                {
                    try
                    {
                        commandTransfer.Transaction = transaction;
                        commandSpeler.Transaction = transaction;
                        //transfer
                        commandTransfer.CommandText = sqlTransfer;
                        commandTransfer.Parameters.AddWithValue("@spelerid", transfer.Speler.Id);
                        commandTransfer.Parameters.AddWithValue("@prijs", transfer.Prijs);
                        if (transfer.OudTeam == null)
                            commandTransfer.Parameters.AddWithValue("@oudteamid", DBNull.Value);
                        else
                            commandTransfer.Parameters.AddWithValue("@oudteamid", transfer.OudTeam.Stamnummer);
                        if (transfer.NieuwTeam == null)
                            commandTransfer.Parameters.AddWithValue("@nieuwteamid", DBNull.Value);
                        else
                            commandTransfer.Parameters.AddWithValue("@nieuwteamid", transfer.NieuwTeam.Stamnummer);
                        int newID = (int)commandTransfer.ExecuteScalar();
                        transfer.ZetId(newID);
                        //speler updaten
                        commandSpeler.CommandText = sqlSpeler;
                        commandSpeler.Parameters.AddWithValue("@id", transfer.Speler.Id);
                        if (transfer.Speler.Team == null)
                            commandSpeler.Parameters.AddWithValue("@teamid", DBNull.Value);
                        else
                            commandSpeler.Parameters.AddWithValue("@teamid", transfer.Speler.Team.Stamnummer);
                        commandSpeler.ExecuteNonQuery();
                        transaction.Commit();
                        return transfer;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransferRepositoryException("Schrijftransferindb", ex);
                    }
                }
            }
        }
    }
}
