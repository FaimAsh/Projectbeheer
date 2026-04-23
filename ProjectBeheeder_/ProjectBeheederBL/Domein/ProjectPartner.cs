using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Domein
{
    public class ProjectPartner
    {


        public ProjectPartner(Project project, Partner partner, string rolomschrijving) {
            this.Partner = partner;
            this.RolBeschrijving = rolomschrijving;
        }
        public Project Project { get; set; }
        public Partner Partner { get; set; }
        public string RolBeschrijving { get; set; }

    }
}
