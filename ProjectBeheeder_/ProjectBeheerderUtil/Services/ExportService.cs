using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderUtil;
using ProjectBeheerderDL_SQL;

public class ExportService
{
    public void Export(string type, string path, List<Project> projecten)
    {
        IFileWriter writer = FileWriterFactory.GetWriter(type);
        writer.Write(path, projecten);
    }
}