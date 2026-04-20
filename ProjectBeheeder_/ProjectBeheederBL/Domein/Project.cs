using ProjectBeheerderBL.DomeinDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderBL.Domein
{

    public class Project
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDatum { get; set; }
        public string Beschrijving { get; set; }
        public Locatie Locatie { get; set; }
        public List<ProjectPartner> Partners { get; set; } = new List<ProjectPartner>();
        public List<ProjectDetail> Details { get; set; } = new List<ProjectDetail>();

    }
}
