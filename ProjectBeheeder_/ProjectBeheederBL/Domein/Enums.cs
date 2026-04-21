using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Domein
{
    public class Enums
    {
        public enum ProjectStatus {
            Planning = 0,
            Uitvoering = 1,
            Afgerond = 2
        }
        public enum VergunningStatus
        {
            InAanvraag = 0,
            Goedgekeurd = 1,
            Geweigerd = 2
        }

        public enum Toegankelijkheid
        {
            VolledigOpenbaar = 0,
            Gedeeltelijk = 1,
            Gesloten = 2
        }

        public enum PartnerType
        {
            Bedrijf = 0, 
            Organisatie = 1,
            Burger = 2
        }
    }
}
