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
        //private int v1;
        //private VergunningStatus vergunningStatus;
        //private bool v2;
        //private Toegankelijkheid toegankelijkheid;
        //private bool v3;
        //private bool v4;

        public StadDetail(int v1, VergunningStatus vergunningStatus, bool v2, Toegankelijkheid toegankelijkheid, bool v3, bool infobordenVoorzien) {
            this.id = v1;
            this.VergunningStatus = vergunningStatus;
            this.v2 = v2;
            this.Toegankelijkheid = toegankelijkheid;
            this.v3 = v3;
            this.InfoBordVoorzien = ;
        }

        public VergunningStatus VergunningStatus { get; set; }
        public bool ArchitecturaleWaarde {  get; set; }
        public Toegankelijkheid Toegankelijkheid { get; set; }
        public bool ToeristischeWaarde { get; set; }
        public bool Bezienswaardigheid { get; set; }

        public bool InfoBordVoorzien {  get; set; }

        public List <Partner> Bouwfirmas { get; set; }
    }
}
