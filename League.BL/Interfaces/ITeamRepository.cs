using League.BL.DTO;
using League.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ITeamRepository
    {
        //bool BestaatTeam(Team t);
        bool BestaatTeam(int stamnummer);
        void SchrijfTeamInDB(Team t);
        Team SelecteerTeam(int stamnummer);
        IReadOnlyList<TeamInfo> SelecteerTeams();
        void UpdateTeam(Team team);
    }
}
