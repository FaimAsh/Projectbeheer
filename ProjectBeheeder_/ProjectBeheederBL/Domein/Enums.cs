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
            InAanvraag,
            Goedgekeurd,
            Geweigerd
        }

        public enum Toegankelijkheid
        {
            VolledigOpenbaar,
            Gedeeltelijk,
            Gesloten
        }

        public enum PartnerType
        {
            Bedrijf, 
            Organisatie,
            Burger
        }
    }
}
