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
       

        public StadDetail(VergunningStatus vergunningStatus, bool architecturaleWaarde, Toegankelijkheid toegankelijkheid, bool bezienswaardigheid, bool infobordenVoorzien) {
            
            this.VergunningStatus = vergunningStatus;
            this.ArchitecturaleWaarde = architecturaleWaarde;
            this.Toegankelijkheid = toegankelijkheid;
            this.Bezienswaardigheid = bezienswaardigheid;
            this.InfoBordVoorzien = infobordenVoorzien;
        }

        public StadDetail(int? id, VergunningStatus vergunningStatus, bool architecturaleWaarde, Toegankelijkheid toegankelijkheid, bool bezienswaardigheid, bool infobordenVoorzien) : this(vergunningStatus,architecturaleWaarde,toegankelijkheid,bezienswaardigheid,infobordenVoorzien) {

            this.Id = id;          
          
        }

        public int? Id { get; set; }
        public VergunningStatus VergunningStatus { get; set; }
        public bool ArchitecturaleWaarde {  get; set; }
        public Toegankelijkheid Toegankelijkheid { get; set; }
        public bool Bezienswaardigheid { get; set; }

        public bool InfoBordVoorzien {  get; set; }

        public List <Partner> Bouwfirmas { get; set; }
    }
}
