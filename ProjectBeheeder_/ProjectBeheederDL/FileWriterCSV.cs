using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System.Text;

public class FileWriterCSV : IFileWriter
{
    public void Write(string path, List<Project> projecten)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Titel,Status,Locatie");

        foreach (var p in projecten)
        {
            sb.AppendLine($"{p.Titel},{p.Status},{p.Locatie}");
        }

        File.WriteAllText(path, sb.ToString());
    }
}