using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System;
using System.IO;

namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {
        private IProjectRepository _repository;

        public ProjectBeheerder(IProjectRepository repository)
        {
            _repository = repository;
            
        }
        public void AddProject(Project p) =>    _repository.AllesImporteren(p);
        public void VerwijderKoppeling(ProjectPartner projectPartner) =>    _repository.KoppelingVerwijderen(projectPartner);
        public List<ProjectPartner> GeefGeKoppeldePartners(int projectId) =>    _repository.GeefGeKoppeldePartners(projectId);
 
        public Project GeefProject(int id) =>    _repository.GeefProject(id);

        public void UpdateProject(Project p) =>    _repository.UpdateProject(p);
        public void VerwijderProject(Project p) =>    _repository.ProjectVerwijderen(p);
        public void VerwijderPartner(Partner p) =>    _repository.PartnerVerwijderen(p);

        public List<Partner> GeefPartners() => _repository.GeefPartners();
        
        public void PartnerAanmaken(Partner NieuwePartner) => _repository.PartnerAanmaken(NieuwePartner);
        public void PartnerKoppelingAanmaken(Partner KoppelPartner) => _repository.PartnerKoppelingAanmaken(KoppelPartner);

        public List<Project> Search(ProjectFilter filter) => _repository.Search(filter);

        //public void UpdateRol(int ProjectID,int PartnerID, string Rolomschrijving) => _repository.UpdateRol(ProjectID,PartnerID,Rolomschrijving);


    }
}