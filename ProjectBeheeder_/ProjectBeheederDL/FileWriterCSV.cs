using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using System.Text;
using System.IO;
using System.Linq;

public class FileWriterCSV : IFileWriter
{
    public void Write(string path, List<Project> projecten)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Titel;Type;Status;Wijk;Partner;Startdatum");

        foreach (var p in projecten)
        {
            string type = p.GetType().Name;
            string wijk = p.Locatie?.Wijk ?? "";

            string partner = string.Join(", ",
                p.Partners
                 .Where(pp => pp.Partner != null)
                 .Select(pp => $"{pp.Partner.Naam} ({pp.RolBeschrijving})"));

            string start = p.StartDatum.ToString("dd/MM/yyyy");

            sb.AppendLine($"{p.Titel};{type};{p.Status};{wijk};{partner};{start}");
        }

        File.WriteAllText(path, sb.ToString());
    }
}