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
    public class TeamManager
    {
        private ITeamRepository repo;
        public TeamManager(ITeamRepository repo)
        {
            this.repo = repo;
        }
        public void RegistreerTeam(int stamnummer, string naam,string bijnaam)
        {
            try
            {
                Team t = new Team(stamnummer, naam);
                if (!string.IsNullOrWhiteSpace(bijnaam)) t.ZetBijnaam(bijnaam);
                if (!repo.BestaatTeam(stamnummer))
                {
                    repo.SchrijfTeamInDB(t);
                }
                else
                {
                    throw new TeamManagerException("RegistreerTeam - Team bestaat al");
                }
            }
            catch(TeamManagerException) { throw; }
            catch(Exception ex)
            {
                throw new TeamManagerException("RegistreerTeam", ex);
            }
        }
        public Team SelecteerTeam(int stamnummer)
        {
            try
            {
                Team team=repo.SelecteerTeam(stamnummer);
                if (team == null) throw new TeamManagerException("Selecteerteam - team bestaat niet");
                return team;
            }
            catch(TeamManagerException) { throw; }
            catch (Exception ex)
            {
                throw new TeamManagerException("selecteerteam", ex);
            }
        }
        public void UpdateTeam(TeamInfo teamInfo)
        {
            if (teamInfo == null) throw new TeamManagerException("update team - team is null");
            if (string.IsNullOrWhiteSpace(teamInfo.Naam)) throw new TeamManagerException("updateteam - naam is null");
            try
            {
                if (repo.BestaatTeam(teamInfo.Stamnummer))
                {
                    Team team = repo.SelecteerTeam(teamInfo.Stamnummer);
                    team.ZetNaam(teamInfo.Naam);
                    if (!string.IsNullOrWhiteSpace(teamInfo.Bijnaam)) team.ZetBijnaam(teamInfo.Bijnaam);
                    else team.VerwijderBijnaam();
                    repo.UpdateTeam(team);
                }
                else
                {
                    throw new TeamManagerException("update team");
                }
            }
            catch(TeamManagerException) { throw; }
            catch(Exception ex) { throw new TeamManagerException("Updateteam", ex); }
        }
        public IReadOnlyList<TeamInfo> SelecteerTeams()
        {
            try
            {
                return repo.SelecteerTeams();
            }
            catch(Exception e)
            {
                throw new TeamManagerException("selecteerteams", e);
            }
        }
    }
}
