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

        public ProjectBeheerder(string type,IProjectRepository repository, IFileWriter writer)
        {
            _repository = repository;
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