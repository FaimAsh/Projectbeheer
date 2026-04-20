using System;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheederDL;           
using ProjectBeheerderDL_SQL;

namespace ProjectBeheerderUtil
{
    public class RepositoryFactory
    {
        public static IProjectRepository GeefRepository(string databaseType, string connectionstring)
        {
            switch (databaseType)
            {
                case "SQL":
                    return new ProjectRepository_SQL(connectionstring);
                default:
                    return null;
            }
        }

        public static IFileWriter GeefFileWriter(string type)
        {
            switch (type)
            {
                case "CSV":
                    return new FileWriterCSV();
                case "PDF":
                    return new FileWriterPDF();
                default:
                    throw new Exception("Onbekend exporttype");
            }
        }
    }
}