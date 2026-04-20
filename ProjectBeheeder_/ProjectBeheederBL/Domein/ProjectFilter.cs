using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderBL.Domein {
    public class ProjectFilter {
        public ProjectStatus Status { get; set; }
        public string Wijk {  get; set; }

        public string PartnerNaam { get; set; }

        public DateTime StartDatumVan { get; set; }

        public DateTime EndDatumVan { get; set; }
    
    }
}
