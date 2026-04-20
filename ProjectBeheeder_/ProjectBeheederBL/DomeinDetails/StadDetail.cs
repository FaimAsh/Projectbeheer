using ProjectBeheerderBL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderBL.DomeinDetails {
    public class StadDetail :ProjectDetail
    {
        public VergunningStatus VergunningStatus { get; set; }
        public bool ArchitecturaleWaarde {  get; set; }
        public Toegankelijkheid Toegankelijkheid { get; set; }
        public bool ToeristischeWaarde { get; set; }
        public bool Bezienswaardigheid { get; set; }

        public bool InfoBordVoorzien {  get; set; }

        public List <Partner> Bouwfirmas { get; set; }
    }
}
