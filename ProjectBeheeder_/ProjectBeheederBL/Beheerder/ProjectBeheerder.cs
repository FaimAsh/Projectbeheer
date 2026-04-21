using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
<<<<<<< HEAD

using System;
using System.IO;


namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {

        private readonly IProjectRepository repository;
        private IFileWriter writer;

        private IProjectRepository _repository;
        private IFileWriter _writer;


        public ProjectBeheerder(string type,IProjectRepository repository, IFileWriter writer)
        {

            this.repository = repository;

            _repository = repository;
            _writer = writer;
        }

        public void Export(string path, List<Project> projecten)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            this.writer.Write(path, projecten);
        }

        public void UpdateProject(Project project) {

            repository.UpdateProject(project);

        }

        public void ProjectVerwijderen(Project project) {

            repository.ProjectVerwijderen(project);

        }

        public void AllesImporteren(Project project) {

            repository.AllesImporteren(project);
        }

        public void PartnerVerwijderen(Partner partner) {

            repository.PartnerVerwijderen(partner);
        }

        public Project GeefProject(int id) {

            return repository.GeefProject(id);
        }

        public List<Project> Search(ProjectFilter filter) {

            return repository.Search(filter);
        }

     

        public void ExportCsv(List<Project> projecten, string path)
        {
            if (_writer == null) throw new InvalidOperationException("Geen exporter geconfigureerd.");
            _writer.Write(path, projecten);
        }

        public void ExportPdf(List<Project> projecten, string path)
        {
            if (_writer == null) throw new InvalidOperationException("Geen exporter geconfigureerd.");
            _writer.Write(path, projecten);
        }

    }
}