using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Domein {
    public class Locatie {
       

        public Locatie(string gemeente, string postcode, string straat, string huisnummer,string wijk) {

           
            this.Gemeente = gemeente;
            this.Postcode = postcode;
            this.Straat = straat;
            this.Huisnummer = huisnummer;
            this.Wijk = wijk;
        }

        public Locatie(int? id, string gemeente, string postcode, string straat, string huisnummer, string wijk) : this(gemeente,postcode,straat,huisnummer,wijk) {

            this.LocatieId = id;
           
        }

        public int? LocatieId {  get; set; }
        public string Gemeente { get; set; }
        public string Postcode { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public string Wijk { get; set; }
    }
}
