using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.DomeinDetails {
    public class WonenDetail : ProjectDetail
    {
        //private int v1;
        //private int v2;
        //private string v3;
        //private bool v4;
        //private bool v5;
        //private int v6;
        //private bool v7;

        public WonenDetail(int v1, int v2, string v3, bool v4, bool v5, int v6, bool v7) {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;
            this.v7 = v7;
        }

        public int AantalEenheden {  get; set; }
        public string Woningtypes { get; set; }
        public bool Rondleidingen { get; set; }
        public bool Showwoningen { get; set; }
        public int InnovatieScore { get; set; }
        public bool ErfgoedSamenwerking { get; set; }
       public int ArchitecturaleScore { get; set; }
    }
}
