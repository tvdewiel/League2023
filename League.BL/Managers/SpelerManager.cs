using League.BL.DTO;
using League.BL.Exceptions;
using League.BL.Interfaces;
using League.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Managers
{
    public class SpelerManager
    {
        private ISpelerRepository repo;

        public SpelerManager(ISpelerRepository repo)
        {
            this.repo = repo;
        }

        public Speler RegistreerSpeler(string naam, int? lengte, int? gewicht)
        {
            try
            {
                Speler s = new Speler(naam, lengte, gewicht);
                if (!repo.BestaatSpeler(s))
                {
                    s = repo.SchrijfSpelerInDB(s);
                    return s;
                }
                else
                    throw new SpelerManagerException("Registreerspeler");
            }
            catch (SpelerManagerException e) { throw; }
            catch (Exception e)
            {
                throw new SpelerManagerException("Registreerspeler", e);
            }
        }
        public void UpdateSpeler(SpelerInfo spelerInfo)
        {
            if (spelerInfo == null) throw new SpelerManagerException("update speler  - speler is null");
            if (spelerInfo.Id == 0) throw new SpelerManagerException("update speler - id = 0");
            try
            {
                if (repo.BestaatSpeler(spelerInfo.Id))
                {
                    //repo.UpdateSpeler(speler);
                    Speler speler = repo.SelecteerSpeler(spelerInfo.Id);
                    bool changed = false;
                    if (speler.Naam != spelerInfo.Naam)
                    {
                        speler.ZetNaam(spelerInfo.Naam);
                        changed = true;
                    }
                    if (((speler.Lengte.HasValue) && (speler.Lengte != spelerInfo.Lengte))
                        || ((spelerInfo.Lengte.HasValue) && (!speler.Lengte.HasValue)))
                    {
                        speler.ZetLengte((int)spelerInfo.Lengte);
                        changed = true;
                    }
                    if (((speler.Gewicht.HasValue) && (speler.Gewicht != spelerInfo.Gewicht))
                        || ((spelerInfo.Gewicht.HasValue) && (!speler.Gewicht.HasValue)))
                    {
                        speler.ZetGewicht((int)spelerInfo.Gewicht);
                        changed = true;
                    }
                    if (((speler.Rugnummer.HasValue) && (speler.Rugnummer != spelerInfo.Rugnummer))
                        || ((spelerInfo.Rugnummer.HasValue) && (!speler.Rugnummer.HasValue)))
                    {
                        speler.ZetRugnummer((int)spelerInfo.Rugnummer);
                        changed = true;
                    }
                    if (!changed) throw new SpelerManagerException("updatespeler - no changes");
                    repo.UpdateSpeler(speler);
                }
                else
                {
                    throw new SpelerManagerException("update speler  - speler niet gevonden");
                }
            }
            catch (SpelerManagerException e) { throw; }
            catch (Exception e)
            {
                throw new SpelerManagerException("updatespeler", e);
            }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam)
        {
            if ((id == null) && (string.IsNullOrWhiteSpace(naam))) throw new SpelerManagerException("selecteerspelers - no input");
            try
            {
                return repo.SelecteerSpelers(id, naam);
            }
            catch (SpelerManagerException e) { throw; }
            catch (Exception e) { throw new SpelerManagerException("selecteerspelers", e); }
        }
    }
}
