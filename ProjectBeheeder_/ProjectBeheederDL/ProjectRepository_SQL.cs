using Microsoft.Data.Sql;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderBL.Domein;


namespace ProjectBeheederDL {
    public class ProjectRepository_SQL : IProjectRepository {

        string _connectionstring;

        public ProjectRepository_SQL(string connectionstring) {

            _connectionstring = connectionstring;
        }

        public void AllesImporteren(Project project) {

            string ProjectQuery = "INSERT INTO Project (Titel,StartDatum,Beschrijving,Status,LocatieID) VALUES (@Titel,@StartDatum,@Beschrijving,@Status,@LocatieID);";
            string LocatieQuery = "INSERT INTO Locatie (Gemeente,Postcode,Straat,Huisnummer) VALUES (@Gemeente,@Postcode,@Straat,@Huisnummer);";
            string 
        }



    }
}
