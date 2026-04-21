using ProjectBeheerderBL.DomeinDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderBL.Domein
{
    public class ProjectFilter
    {
        public ProjectFilter()
        {
        }

        public ProjectFilter(ProjectStatus status, string wijk, string partnernaam, DateTime startDatumVan, DateTime StartDatumTot)
        {
            this.Status = status;
            this.Wijk = wijk;
            this.PartnerNaam = partnernaam;
            this.StartDatumVan = startDatumVan;
            this.StartDatumTot = StartDatumTot;

        }
        public ProjectStatus Status { get; set; }
        public string Wijk { get; set; }

        public string PartnerNaam { get; set; }

        public DateTime? StartDatumVan { get; set; }

        public DateTime? StartDatumTot { get; set; }
        public List<ProjectDetail> Details { get; set; } = new List<ProjectDetail>();

    }
}
