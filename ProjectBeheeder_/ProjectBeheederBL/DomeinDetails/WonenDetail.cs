using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.DomeinDetails {
    public class WonenDetail : ProjectDetail
    {
       

        public WonenDetail(int id, int aantalEenheden, string woningtypes, bool rondleiding, bool showwoning, int innovatieScore, bool erfgoedSamenwerking ) {
            this.Id = id;
            this.AantalEenheden = aantalEenheden;
            this.Woningtypes = woningtypes;
            this.Rondleidingen = rondleiding;
            this.Showwoningen = showwoning;
            this.InnovatieScore = innovatieScore;
            this.ErfgoedSamenwerking = erfgoedSamenwerking;
        }

        public int Id { get; set; }
        public int AantalEenheden {  get; set; }
        public string Woningtypes { get; set; }
        public bool Rondleidingen { get; set; }
        public bool Showwoningen { get; set; }
        public int InnovatieScore { get; set; }
        public bool ErfgoedSamenwerking { get; set; }
     
    }
}
