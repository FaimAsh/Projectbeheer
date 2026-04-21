using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
<<<<<<< HEAD

=======
using System;
using System.IO;
>>>>>>> 9523682d1b95f96e270e42ad17aae71041e26b77

namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {
<<<<<<< HEAD
        private readonly IProjectRepository repository;
        private IFileWriter writer;
=======
        private IProjectRepository _repository;
        private IFileWriter _writer;
>>>>>>> 9523682d1b95f96e270e42ad17aae71041e26b77

        public ProjectBeheerder(string type,IProjectRepository repository, IFileWriter writer)
        {
<<<<<<< HEAD
            this.repository = repository;
=======
            _repository = repository;
            _writer = writer;
>>>>>>> 9523682d1b95f96e270e42ad17aae71041e26b77
        }

        public void Export(string path, List<Project> projecten)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            this.writer.Write(path, projecten);
        }
<<<<<<< HEAD

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

     
=======
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
>>>>>>> 9523682d1b95f96e270e42ad17aae71041e26b77
    }
}