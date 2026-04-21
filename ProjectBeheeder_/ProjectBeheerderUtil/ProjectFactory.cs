using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderUtil
{
    public class ProjectFactory
    {
        public Project CreateProject(string titel, DateTime startDatum, ProjectStatus status, Locatie locatie)
        {
            return new Project(
                null,
                titel,
                startDatum,
                string.Empty,
                status,
                locatie
            )
            {
                Partners = new List<ProjectPartner>(),
                Details = new List<ProjectDetail>()
            };
        }

        public StadDetail AddStadsDetail(
            int id,
            VergunningStatus vergunningstatus,
            bool architecturaleWaarde,
            Toegankelijkheid toegankelijkheid,
            bool toeristischeWaarde,
            bool infowandeling,
            bool infobordenVoorzien)
            => new(id, vergunningstatus, architecturaleWaarde, toegankelijkheid, toeristischeWaarde, infobordenVoorzien);

        public GroenDetail AddGroenDetail(
            int id,
            decimal oppervlakte,
            int biodiversiteitScore,
            int wandelpaden,
            string faciliteiten,
            bool toeristischeRoute,
            int beoordeling)
            => new(id, oppervlakte, biodiversiteitScore, wandelpaden, faciliteiten, toeristischeRoute, beoordeling);

        public WonenDetail AddWonenDetail(
            int id,
            int aantalEenheden,
            string woningTypes,
            bool rondleidingen,
            bool showwoning,
            int innovatieScore,
            bool erfgoedSamenwerking)
            => new(id, aantalEenheden, woningTypes, rondleidingen, showwoning, innovatieScore, erfgoedSamenwerking);
    }
}
