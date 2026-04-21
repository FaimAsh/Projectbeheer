using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.DomeinDetails {
    public class GroenDetail : ProjectDetail
    {
        private int v1;
        private decimal v2;
        private int v3;
        private int v4;
        private string v5;
        private bool v6;
        private int v7;

        public GroenDetail(int v1, decimal v2, int v3, int v4, string v5, bool v6, int v7) {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;
            this.v7 = v7;
        }

        public double Oppervlakte {  get; set; }
        public int Biodiversiteit { get; set; }
        public int Wandelpaden { get; set; }
        public string Faciliteiten { get; set; }
        public bool ToeristischeRoute {  get; set; }
        public int Beoordeling {  get; set; }

    }
}
