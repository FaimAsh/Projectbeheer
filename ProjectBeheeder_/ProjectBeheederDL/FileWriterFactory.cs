using ProjectBeheerderBL.Interfaces;

namespace ProjectBeheerderBL.Factories
{
    public static class FileWriterFactory
    {
        public static IFileWriter GetWriter(string type)
        {
            switch (type)
            {
                case "CSV":
                    return new FileWriterCSV();

                case "PDF":
                    return new FileWriterPDF();

                default:
                    throw new ArgumentException("Unknown type");
            }
        }
    }
}