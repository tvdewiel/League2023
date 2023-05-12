using League.BL.DTO;
using League.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ISpelerRepository
    {
        bool BestaatSpeler(int id);
        bool BestaatSpeler(Speler speler);
        Speler SchrijfSpelerInDB(Speler s);
        Speler SelecteerSpeler(int id);
        IReadOnlyList<SpelerInfo> SelecteerSpelers(int? id, string naam);
        void UpdateSpeler(Speler speler);
    }
}
