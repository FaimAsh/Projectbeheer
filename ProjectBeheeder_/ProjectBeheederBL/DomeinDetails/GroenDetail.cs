using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.DomeinDetails {
    public class GroenDetail : ProjectDetail
    {
        public double Oppervlakte {  get; set; }
        public int Biodiversiteit { get; set; }
        public int Wandelpaden { get; set; }
        public string Faciliteiten { get; set; }
        public bool ToeristischeRoute {  get; set; }
        public int Beoordeling {  get; set; }

    }
}
