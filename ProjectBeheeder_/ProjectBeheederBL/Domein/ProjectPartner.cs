using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Domein
{
    public class ProjectPartner
    {
      

        public ProjectPartner(Partner partner, string rolomschrijving) {
            this.Partner = partner;
            this.RolBeschrijving = rolomschrijving;
        }

        public Partner Partner { get; set; }
        public string RolBeschrijving { get; set; }
        public int ProjectId { get; set; }
        public int PartnerId { get; set; }

    }
}
