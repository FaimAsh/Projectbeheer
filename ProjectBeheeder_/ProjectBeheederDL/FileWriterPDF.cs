using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectBeheerderBL.Domein;
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
                string type = p.GetType().Name;
                string wijk = p.Locatie?.Wijk ?? "";
                string start = p.StartDatum.ToString("dd/MM/yyyy");

                string partner = string.Join(", ",
                    p.Partners
                     .Where(pp => pp.Partner != null)
                     .Select(pp => $"{pp.Partner.Naam} ({pp.RolBeschrijving})"));

                document.Add(new Paragraph($"Naam: {p.Titel}"));
                document.Add(new Paragraph($"Type: {type}"));
                document.Add(new Paragraph($"Status: {p.Status}"));
                document.Add(new Paragraph($"Wijk: {wijk}"));
                document.Add(new Paragraph($"Partners: {partner}"));
                document.Add(new Paragraph($"Startdatum: {start}"));
                document.Add(new Paragraph("----------------------------------"));
            }

            document.Close();
        }
    }
}