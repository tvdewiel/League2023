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
        //TODO make internal
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
        public bool HeeftSpeler(Speler speler)
        {
            return _spelers.Contains(speler);
        }
        public void VoegSpelerToe(Speler speler)
        {
            if (speler == null) throw new TeamException("voegspelertoe - speler is null");
            if (_spelers.Contains(speler)) throw new TeamException("Voegspelertoe - speler bestaat reeds");
            _spelers.Add(speler);
            if (speler.Team != this)
                speler.ZetTeam(this);
        }
        public void VerwijderSpeler(Speler speler)
        {
            if (speler == null) throw new TeamException("verwijderspeler - speler is null");
            if (!_spelers.Contains(speler)) throw new TeamException("Verwijderspeler - speler bestaat niet");
            _spelers.Remove(speler);
            if (speler.Team == this)
                speler.VerwijderTeam();
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
        public void VerwijderBijnaam()
        {
            Bijnaam = null;
        }

        public override bool Equals(object? obj)
        {
            return obj is Team team &&
                   Stamnummer == team.Stamnummer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Stamnummer);
        }
    }
}
