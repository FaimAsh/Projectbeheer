using System.Collections.Generic;
using System.Linq;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace ProjectBeheerderDL_SQL
{
    public class FileWriterPDF : IFileWriter
    {
        public void Write(string path, List<Project> projecten)
        {
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            foreach (var p in projecten)
            {
                document.Add(new Paragraph($"Naam: {p.Titel}"));
                document.Add(new Paragraph($"Type: {p.GetType().Name}"));
                document.Add(new Paragraph($"Status: {p.Status}"));
                document.Add(new Paragraph($"Wijk: {p.Locatie?.Wijk ?? ""}"));
                document.Add(new Paragraph($"Startdatum: {p.StartDatum:dd/MM/yyyy}"));

                document.Add(new Paragraph("Partners:"));
                string partner = string.Join(", ",
                    p.Partners?.Select(pp =>
                        $"{pp.Partner?.Naam} ({pp.RolBeschrijving})"
                    ) ?? new List<string>());

                document.Add(new Paragraph(partner));

                if (p.Details == null || !p.Details.Any())
                {
                    document.Add(new Paragraph("Geen details gevonden"));
                }
                else
                {
                    foreach (var detail in p.Details)
                    {
                        if (detail is StadDetail sd)
                        {
                            document.Add(new Paragraph("--- Stad details ---"));
                            document.Add(new Paragraph($"Vergunningstatus: {sd.VergunningStatus}"));
                            document.Add(new Paragraph($"Architecturale waarde: {sd.ArchitecturaleWaarde}"));
                            document.Add(new Paragraph($"Toegankelijkheid: {sd.Toegankelijkheid}"));
                            document.Add(new Paragraph($"Bezienswaardigheid: {sd.Bezienswaardigheid}"));
                            document.Add(new Paragraph($"Infobord: {sd.InfoBordVoorzien}"));
                        }
                        else if (detail is GroenDetail gd)
                        {
                            document.Add(new Paragraph("--- Groen details ---"));
                            document.Add(new Paragraph($"Oppervlakte: {gd.Oppervlakte} m²"));
                            document.Add(new Paragraph($"Biodiversiteit: {gd.Biodiversiteit}/10"));
                            document.Add(new Paragraph($"Wandelpaden: {gd.Wandelpaden}"));
                            document.Add(new Paragraph($"Faciliteiten: {gd.Faciliteiten}"));
                            document.Add(new Paragraph($"Toeristische route: {gd.ToeristischeRoute}"));
                            document.Add(new Paragraph($"Beoordeling: {gd.Beoordeling}"));
                        }
                        else if (detail is WonenDetail wd)
                        {
                            document.Add(new Paragraph("--- Wonen details ---"));
                            document.Add(new Paragraph($"Eenheden: {wd.AantalEenheden}"));
                            document.Add(new Paragraph($"Type: {wd.Woningtypes}"));
                            document.Add(new Paragraph($"Rondleidingen: {wd.Rondleidingen}"));
                            document.Add(new Paragraph($"Showwoningen: {wd.Showwoningen}"));
                        }
                    }
                }

                document.Add(new Paragraph("=================================="));
            }

            document.Close();
        }
    }
}