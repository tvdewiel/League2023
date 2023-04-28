using League.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Model
{
    public class Team
    {
        internal Team(int stamnummer, string naam)
        {
            ZetStamnummer(stamnummer);
            ZetNaam(naam);
        }
        public int Stamnummer { get;private set; }
        public string Naam { get; private set; }
        public string Bijnaam { get; private set; }
        private List<Speler> _spelers = new List<Speler>();
        public IReadOnlyList<Speler> Spelers()
        {
            return _spelers.AsReadOnly();
        }
        public void VoegSpelerToe(Speler speler)
        {

        }
        public void VerwijderSpeler(Speler speler)
        {

        }
        public void ZetStamnummer(int stamnummer)
        {
            if (stamnummer <= 0) throw new TeamException("ZetStamnummer");
            Stamnummer= stamnummer;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new TeamException("ZetNaam");
            Naam= naam.Trim();
        }
        public void ZetBijnaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new TeamException("ZetBijnaam");
            Bijnaam = naam.Trim();
        }
    }
}
