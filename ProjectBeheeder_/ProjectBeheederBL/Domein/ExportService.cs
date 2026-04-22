using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;

namespace ProjectBeheederBL.Domein
{
    public class ExportService
    {
        private readonly IFileWriter _writer;

        //public ExportService(string type) {
        //    _writer = FileWriterFactory.GetWriter(type);
        //}

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