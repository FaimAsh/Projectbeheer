using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System;

namespace ProjectBeheerderBL.Beheerder
{
    public class ProjectBeheerder
    {
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
        public void ExportCsv(List<Project> projecten, string fileName)
        {
            throw new NotImplementedException();
        }

        public void ExportPdf(List<Project> projecten, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}