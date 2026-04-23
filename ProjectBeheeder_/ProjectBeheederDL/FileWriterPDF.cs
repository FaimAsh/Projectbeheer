using System.Collections.Generic;
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
            using (PdfWriter writer = new PdfWriter(path))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf))
            {
                document.Add(new Paragraph("Projecten Overzicht"));
                document.Add(new Paragraph(" "));

                foreach (var p in projecten)
                {
                    document.Add(new Paragraph($"Project: {p.Titel}"));
                    document.Add(new Paragraph($"Status: {p.Status}"));
                    document.Add(new Paragraph($"Wijk: {p.Locatie?.Wijk}"));
                    document.Add(new Paragraph("------"));
                }
            }
        }
    }
}