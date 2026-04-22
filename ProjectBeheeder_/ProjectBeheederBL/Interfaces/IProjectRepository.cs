using ProjectBeheerderBL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBeheerderBL.Interfaces {
    public interface IProjectRepository {
        void AllesImporteren(Project project);

        void ProjectVerwijderen(Project project);
        
        void PartnerVerwijderen(Partner partner);
        
        Project GeefProject(int id);
        
        void UpdateProject(Project project);

        void PartnerAanmaken(ProjectPartner NieuwePartner);

        List<Partner> GeefPartners();


        //FILTER/SEARCH

        List<Project> Search(ProjectFilter filter);

      



        }
    }
