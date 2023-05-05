using League.BL.Interfaces;
using League.BL.Managers;
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
            sm.RegistreerSpeler("jos", 189, 87);

        }
    }
}