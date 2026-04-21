using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderBL.Domein
{
    public class Partner
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public PartnerType PartnerType { get; set; }
    }
}
