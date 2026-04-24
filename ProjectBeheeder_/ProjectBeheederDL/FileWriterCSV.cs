using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Interfaces;
using System.Text;
using System.IO;
using System.Linq;

public class FileWriterCSV : IFileWriter
{
    public void Write(string path, List<Project> projecten)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Titel;Type;Status;Wijk;Partner;Startdatum;Details");

        foreach (var p in projecten)
        {
            string type = p.GetType().Name;
            string wijk = p.Locatie?.Wijk ?? "";

            string partner = string.Join(", ",
                p.Partners
                 .Where(pp => pp.Partner != null)
                 .Select(pp => $"{pp.Partner.Naam} ({pp.RolBeschrijving})"));

            string start = p.StartDatum.ToString("dd/MM/yyyy");

            string details = "/";

            if (p.Details != null && p.Details.Any())
            {
                details = string.Join(" | ",
                    p.Details.Select(d =>
                    {
                        if (d is StadDetail sd)
                            return $"Stad: Vergunning={sd.VergunningStatus}, Architectuur={sd.ArchitecturaleWaarde}, Toegankelijkheid={sd.Toegankelijkheid}";

                        if (d is GroenDetail gd)
                            return $"Groen: Opp={gd.Oppervlakte}m², Bio={gd.Biodiversiteit}/10, Wandelpaden={gd.Wandelpaden}";

                        if (d is WonenDetail wd)
                            return $"Wonen: Eenheden={wd.AantalEenheden}, Types={wd.Woningtypes}, Showwoning={wd.Showwoningen}";

                        return "";
                    }).Where(x => !string.IsNullOrWhiteSpace(x)));
            }

            sb.AppendLine($"{p.Titel};{type};{p.Status};{wijk};{partner};{start};{details}");
        }

        File.WriteAllText(path, sb.ToString());
    }
}