using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.DomeinDetails
{
    public class GroenDetail : ProjectDetail
    {


        public GroenDetail(decimal oppervlakte, int biodiversiteit, int aantalWandelpaden, string faciliteiten, bool toeristischeRoute, int beoordeling)
        {

            this.Oppervlakte = oppervlakte;
            this.Biodiversiteit = biodiversiteit;
            this.Wandelpaden = aantalWandelpaden;
            this.Faciliteiten = faciliteiten;
            this.ToeristischeRoute = toeristischeRoute;
            this.Beoordeling = beoordeling;
        }

        public GroenDetail(int? id, decimal oppervlakte, int biodiversiteit, int aantalWandelpaden, string faciliteiten, bool toeristischeRoute, int beoordeling) : this(oppervlakte, biodiversiteit, aantalWandelpaden, faciliteiten, toeristischeRoute, beoordeling)
        {

            this.Id = id;

        }

        public override string TypeNaam => "Groen";



        public int? Id { get; set; }
        public decimal Oppervlakte { get; set; }
        public int Biodiversiteit { get; set; }
        public int Wandelpaden { get; set; }
        public string Faciliteiten { get; set; }
        public bool ToeristischeRoute { get; set; }
        public int Beoordeling { get; set; }

    }
}
