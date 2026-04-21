using System.Collections.Generic;
using System.IO;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderUtil;

namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {
        private readonly IProjectRepository _repository;
        private IFileWriter _writer;

        public ProjectBeheerder(IProjectRepository repository)
        {
            _repository = repository;
        }

        public void Export(string path, List<Project> projecten)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _writer.Write(path, projecten);
        }
    }
}