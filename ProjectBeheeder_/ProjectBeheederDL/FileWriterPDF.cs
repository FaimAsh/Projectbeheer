using System.Collections.Generic;
using System.IO;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;

namespace ProjectBeheerderDL_SQL
{
    public class FileWriterPDF : IFileWriter
    {
        public void Write(string path, List<Project> projecten)
        {
            string file = Path.Combine(path, "export.pdf");

            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (var p in projecten)
                {
                    sw.WriteLine($"Project: {p.Titel}");
                    sw.WriteLine($"Status: {p.Status}");
                    sw.WriteLine("------");
                }
            }
        }
    }
}