using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.DomeinDetails {
    public class WonenDetail : ProjectDetail
    {
        public int AantalEenheden {  get; set; }
        public string Woningtypes { get; set; }
        public bool Rondleidingen { get; set; }
        public bool Showwoningen { get; set; }
        public int InnovatieScore { get; set; }
        public bool ErfgoedSamenwerking { get; set; }
    }
}
