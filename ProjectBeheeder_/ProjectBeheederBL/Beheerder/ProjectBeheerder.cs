using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System;
using System.IO;

namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {
        private IProjectRepository _repository;
        private IFileWriter _writer;

        public ProjectBeheerder(string type, IFileWriter writer)
        {
            _writer = writer;
        }

        public void Export(string path, List<Project> projecten)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _writer.Write(path, projecten);
        }

        public ProjectBeheerder(IProjectRepository repository)
        {
            _repository = repository;
            
        }
       
        public List<Project> GetAll() =>    _repository.Search(new ProjectFilter());
        public List<Project> Search(ProjectFilter filter) =>    _repository.Search(filter);
        public Project GetByID(int id) =>    _repository.GeefProject(id);

        public void AddProject(Project p) =>    _repository.AllesImporteren(p);
        public void UpdateProject(Project p) =>    _repository.UpdateProject(p);
        public void DeleteProject(Project p) =>    _repository.ProjectVerwijderen(p);
        public void DeletePartner(Partner p) =>    _repository.PartnerVerwijderen(p);

        public List<Partner> GetPartners() => _repository.GeefPartners();
        
       
        public void AddPartner(Partner NieuwePartner) => _repository.PartnerAanmaken(NieuwePartner);
     
        public void ExportCsv(List<Project> projecten, string path)
        {
            if (_writer == null) throw new InvalidOperationException("Geen exporter geconfigureerd.");
            _writer.Write(path, projecten);
        }

        public void ExportPdf(List<Project> projecten, string path) {
            if (_writer == null) throw new InvalidOperationException("Geen exporter geconfigureerd.");
            _writer.Write(path, projecten);
        }


        public void GetPartners(Partner p) => _repository.GeefPartners();
       

        
        public List<Partner> GivePartner() => _repository.GeefPartners();


    }
}