using ProjectBeheerderBL.Interfaces;
using ProjectBeheederDL;
using ProjectBeheerderDL_SQL;

namespace ProjectBeheerderUtil
{
    public class FileWriterFactory
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