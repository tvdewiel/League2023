using League.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Model
{
    public class Transfer
    {
        public Transfer(Speler speler, Team oudTeam)
        {
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
        }

        public Transfer(Speler speler, Team nieuwTeam, int prijs) 
        {
            ZetPrijs(prijs);
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
        }

        public Transfer(Speler speler, Team nieuwTeam, Team oudTeam, int prijs) : this(speler,nieuwTeam,prijs)
        {
            ZetOudTeam(oudTeam);
        }

        public Transfer(int id, Speler speler, Team nieuwTeam, Team oudTeam, int prijs)
        {
            ZetId(id);
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
            ZetPrijs(prijs);
            ZetNieuwTeam(nieuwTeam);
        }

        public int Id { get; private set; }
        public Speler Speler { get; private set; }
        public Team NieuwTeam { get; private set; }
        public Team OudTeam { get; private set; }
        public int Prijs { get; private set; }
        public void ZetId(int id)
        {
            if (id <= 0) throw new TransferException("zetid");
            Id = id;
        }
        public void ZetPrijs(int prijs)
        {
            if (prijs < 0) throw new TransferException("Zetprijs");
            Prijs = prijs;
        }
        public void VerwijderOudTeam()
        {
            if (NieuwTeam is null) throw new TransferException("verwijderoudteam");
            OudTeam = null;
        }
        public void ZetOudTeam(Team team)
        {
            if (team == null) throw new TransferException("Zetoudteam");
            if (team == NieuwTeam) throw new TransferException("zetoudteam");
            OudTeam = team;
        }
        public void VerwijderNieuwTeam()
        {
            if (OudTeam == null) throw new TransferException("verwijdernieuwteam");
            NieuwTeam = null;
        }
        public void ZetNieuwTeam(Team team)
        {
            if (team == null) throw new TransferException("Zetnieuwteam");
            if (team == OudTeam) throw new TransferException("zetnieuwteam");
            NieuwTeam = team;
        }
        public void ZetSpeler(Speler speler)
        {
            if (speler == null) throw new TransferException("Zetspeler");
            Speler= speler;
        }
    }
}
