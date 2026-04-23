using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System.Text;
using System.IO;

public class FileWriterCSV : IFileWriter
{
    public void Write(string path, List<Project> projecten)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Titel;Status;Wijk");

        foreach (var p in projecten)
        {
            sb.AppendLine($"{p.Titel};{p.Status};{p.Locatie?.Wijk}");
        }

        File.WriteAllText(path, sb.ToString());
    }
}