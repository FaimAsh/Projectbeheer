using ProjectBeheerderBL.DomeinDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectBeheerderBL.Domein.Enums;

namespace ProjectBeheerderBL.Domein
{

    public class Project
    {
      

        public Project(string titel, DateTime startdatum, string beschrijving, ProjectStatus projectStatus, Locatie locatie) {

            
            this.Titel = titel;
            this.StartDatum = startdatum;
            this.Beschrijving = beschrijving;
            this.Status = projectStatus;
            this.Locatie = locatie;
        }


        public Project(int? id, string titel, DateTime startdatum, string beschrijving, ProjectStatus projectStatus, Locatie locatie) : this (titel,startdatum,beschrijving,projectStatus,locatie){

            this.Id = id;
        }

        public int? Id { get; set; }
        public string Titel { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDatum { get; set; }
        public string Beschrijving { get; set; }
        public Locatie Locatie { get; set; }
        public List<ProjectPartner> Partners { get; set; } = new List<ProjectPartner>();
        public List<ProjectDetail> Details { get; set; } = new List<ProjectDetail>();
    }


}
