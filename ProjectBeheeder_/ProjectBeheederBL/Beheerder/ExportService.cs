using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System.Collections.Generic;
using System.IO;
public class ExportService
{
    public void Export(string path, List<Project> projecten, IFileWriter writer)
    {
        if (projecten == null || projecten.Count == 0)
            throw new ArgumentException("Geen projecten om te exporteren.");
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        writer.Write(path, projecten);
    }
}
