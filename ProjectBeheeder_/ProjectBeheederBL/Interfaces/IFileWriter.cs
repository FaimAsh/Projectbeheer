using System.Collections.Generic;
using ProjectBeheerderBL.Domein;

namespace ProjectBeheerderBL.Interfaces
{
    public interface IFileWriter
    {
        void Write(string path, List<Project> projecten);
        void GetWriterType(string type);
    }
}