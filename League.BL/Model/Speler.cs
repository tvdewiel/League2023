using League.BL.Exceptions;

namespace League.BL.Model
{
    public class Speler
    {
        internal Speler(string naam, int? lengte, int? gewicht)
        {
            ZetNaam(naam);
            if (lengte.HasValue) ZetLengte((int)lengte);
            if (gewicht!=null) ZetGewicht(gewicht.Value);
        }
        internal Speler(int id, string naam, int? lengte, int? gewicht) : this(naam,lengte,gewicht)
        {
            ZetId(id);
        }
        public int Id { get;private set; }
        public string Naam { get;private set; }
        public Team Team { get;private set; }
        public int? Rugnummer { get;private set; }
        public int? Lengte { get;private set; }
        public int? Gewicht { get;private set; }
        public void VerwijderTeam()
        {
            if (Team== null) { throw new SpelerException("verwijdertema - geen team"); }
            if (Team.HeeftSpeler(this))
                Team.VerwijderSpeler(this);
            Team = null;
        }
        public void ZetTeam(Team team)
        {
            if (team == null) throw new SpelerException("zetteam- team is null");
            if (Team == team) throw new SpelerException("zetteam - zelfde team");
            if (Team != null)
            {
                if (Team.HeeftSpeler(this)) Team.VerwijderSpeler(this);
            }
            if (!team.HeeftSpeler(this))
            {
                Team = team;
                team.VoegSpelerToe(this);
            }
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new SpelerException("ZetNaam");
            Naam=naam.Trim();
        }
        public void ZetLengte(int lengte)
        {
            if (lengte < 150) throw new SpelerException("ZetLengte");
            Lengte=lengte;
        }
        public void ZetGewicht(int gewicht)
        {
            if (gewicht < 50) throw new SpelerException("ZetGewicht");
            Gewicht=gewicht;
        }
        public void ZetId(int id)
        {
            if (id <= 0) throw new SpelerException("ZetId");
            Id=id;
        }
        public void ZetRugnummer(int rugnummer)
        {
            if ((rugnummer <= 0) || (rugnummer > 99)) throw new SpelerException("ZetRugnummer");
            Rugnummer=rugnummer;
        }

        public override bool Equals(object? obj)
        {
            return obj is Speler speler &&
                   Id == speler.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}