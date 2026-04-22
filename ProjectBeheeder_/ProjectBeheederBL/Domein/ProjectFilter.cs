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
        public HashSet<string> Statussen { get; set; } = new HashSet<string>();
        public string? Wijk { get; set; }

        public string? PartnerNaam { get; set; }

        public DateTime? StartDatumVan { get; set; }

        public DateTime? StartDatumTot { get; set; }
        public HashSet<string> Details { get; set; } = new HashSet<string>();
    }
}
