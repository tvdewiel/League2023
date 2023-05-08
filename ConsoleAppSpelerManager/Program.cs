using League.BL.Interfaces;
using League.BL.Managers;
using League.BL.Model;
using League.DL.Repositories;

namespace ConsoleAppSpelerManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string connString = "Data Source=NB21-6CDPYD3\\SQLEXPRESS;Initial Catalog=LeagueDB_A;Integrated Security=True";
            ISpelerRepository repo = new SpelerRepositoryADO(connString);
            SpelerManager sm=new SpelerManager(repo);
            //sm.RegistreerSpeler("jos", 189, 87);
            ITeamRepository repoT = new TeamRepositoryADO(connString);
            TeamManager tm = new TeamManager(repoT);
            //tm.RegistreerTeam(78, "Lierse","de schapen");
            //tm.RegistreerTeam(79, "Lyra", null);
            Team t79 = tm.SelecteerTeam(79);
            Team t78 = tm.SelecteerTeam(78);
            ITransferRepository repoTransfer= new TransferRepositoryADO(connString);
            TransferManager transferM = new TransferManager(repoTransfer);
            //Speler s = new Speler(1, "jos", 187, 87);
            //transferM.RegistreerTransfer(t79.Spelers()[0], t78,255);
            transferM.RegistreerTransfer(t78.Spelers()[0], null, 255);
        }
    }
}