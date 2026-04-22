using ProjectBeheerderBL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Interfaces {
    public interface IReportExporter {

        void Export(string type, string path, List<Project> projecten);
    }
}
