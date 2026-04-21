using System.Collections.Generic;
using System.IO;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;


namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {
        private readonly IProjectRepository repository;
        private IFileWriter writer;

        public ProjectBeheerder(IProjectRepository repository)
        {
            this.repository = repository;
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

     
    }
}