using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Domein {
    public class Locatie {
        public int LocatieId {  get; set; }
        public string Gemeente { get; set; }
        public string Postcode { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public string Wijk { get; set; }
    }
}
