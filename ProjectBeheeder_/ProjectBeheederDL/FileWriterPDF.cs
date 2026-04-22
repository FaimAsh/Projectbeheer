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
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var p in projecten)
                {
                    sw.WriteLine($"Project: {p.Titel}");
                    sw.WriteLine($"Status: {p.Status}");
                    sw.WriteLine($"Wijk: {p.Locatie?.Wijk}");
                    sw.WriteLine("------");
                }
            }
        }
    }
}