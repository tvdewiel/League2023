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
    public class TransferManager
    {
        private ITransferRepository repo;
        private ISpelerRepository spelerRepo;
        private ITeamRepository teamRepo;
        public TransferManager(ITransferRepository repo, ISpelerRepository spelerRepo, ITeamRepository teamRepo)
        {
            this.repo = repo;
            this.spelerRepo = spelerRepo;
            this.teamRepo = teamRepo;
        }
        public Transfer RegistreerTransfer(SpelerInfo spelerInfo,TeamInfo nieuwTeamInfo,int prijs)
        {
            if (spelerInfo == null) throw new TransferManagerException("RegistreerTransfer - speler is null");
            if (spelerInfo.Id==0) throw new TransferManagerException("RegistreerTransfer - spelerid is 0");
            Transfer transfer = null;
            try
            {
                //speler stopt
                if (nieuwTeamInfo == null)
                {
                    if (spelerInfo.Team == null) throw new TransferManagerException("RegistreerTransfer - 2xnull");
                    Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.Id);
                    transfer = new Transfer(speler, speler.Team);
                    speler.VerwijderTeam();
                }
                //nieuwe speler
                else if (spelerInfo.Team == null)
                {
                    Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.Id);
                    Team team = teamRepo.SelecteerTeam(nieuwTeamInfo.Stamnummer);
                    speler.ZetTeam(team);
                    transfer = new Transfer(speler, team, prijs);
                }
                //klassieke transfer
                else
                {
                    Speler speler = spelerRepo.SelecteerSpeler(spelerInfo.Id);
                    Team team = teamRepo.SelecteerTeam(nieuwTeamInfo.Stamnummer);
                    transfer = new Transfer(speler, team, speler.Team, prijs);
                    speler.ZetTeam(team);
                }
                return repo.SchrijfTransferInDB(transfer);               
            }
            catch(TransferManagerException) { throw; }
            catch(Exception ex) { throw new TransferManagerException("RegistreerTransfer", ex); }
        }
    }
}
