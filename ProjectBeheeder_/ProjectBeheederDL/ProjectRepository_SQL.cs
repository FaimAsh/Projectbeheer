using Microsoft.Data.Sql;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderBL.Domein;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using ProjectBeheerderBL.DomeinDetails;


namespace ProjectBeheederDL {
    public class ProjectRepository_SQL : IProjectRepository {

        string _connectionstring;

        public ProjectRepository_SQL(string connectionstring) {

            _connectionstring = connectionstring;
        }

        public void AllesImporteren(Project project) {
            string ProjectQuery = "INSERT INTO Project (Titel,StartDatum,Beschrijving,Status,LocatieID) OUTPUT INSERTED.ID VALUES (@Titel,@StartDatum,@Beschrijving,@Status,@LocatieID);";
            string LocatieQuery = "INSERT INTO Locatie (Gemeente,Postcode,Straat,Huisnummer,wijk) OUTPUT INSERTED.LocatieID VALUES (@Gemeente,@Postcode,@Straat,@Huisnummer,@wijk);";
            string StadDetailQuery = "INSERT INTO StadDetail (Vergunningstatus,ArchitecturaleWaarde,Toegankelijkheid,Bezienswaardigheid,InfobordVoorzien,ProjectID) OUTPUT INSERTED.StadDetailID VALUES (@Vergunningstatus,@ArchitecturaleWaarde,@Toegankelijkheid,@Bezienswaardigheid,@InfobordVoorzien,@ProjectID);";
            string WonenDetailQuery = "INSERT INTO InnovatiefwonenDetail (AantalWooneenheden,TypeWoonVorm,RondLeidingMogelijk,ShowWoningMogelijk,ArchitecturaleScore,SamenwerkingErfgoed,ProjectID) VALUES (@AantalWooneenheden,@TypeWoonVorm,@RondLeidingMogelijk,@ShowWoningMogelijk,@ArchitecturaleScore,@SamenwerkingErfgoed,@ProjectID);";
            string GroenDetailQuery = "INSERT INTO GroenDetail (Oppervlakte,BiodiversiteitScore,AantalWandelpaden,BeschikbareFaciliteit,ToeristischeRoute,BezoekersBeoordeling,ProjectID) VALUES (@Oppervlakte,@Biodiversiteitscore,@AantalWandelpaden,@BeschikbareFaciliteit,@ToeristischeRoute,@BezoekersBeoordeling,@ProjectID);";
            string BouwfirmaQuery = "INSERT INTO StadsOntwikkeling_Partner (StadDetailID,PartnerID) VALUES (@StadDetailID,@PartnerID);";
            string ExternePartnerQuery = "INSERT INTO Project_Partner (ProjectID,PartnerID,Rolomschrijving) VALUES (@ProjectID,@PartnerID,@Rolomschrijving);";

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            using (SqlCommand cmdProject = conn.CreateCommand())
            using (SqlCommand cmdLocatie = conn.CreateCommand())
            using (SqlCommand cmdStadDetail = conn.CreateCommand())
            using (SqlCommand cmdWonenDetail = conn.CreateCommand())
            using (SqlCommand cmdGroenDetail = conn.CreateCommand())
            using (SqlCommand cmdBouwFirma = conn.CreateCommand())
            using (SqlCommand cmdExternePartner = conn.CreateCommand()) {

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                cmdProject.Transaction = transaction;
                cmdLocatie.Transaction = transaction;
                cmdStadDetail.Transaction = transaction;
                cmdWonenDetail.Transaction = transaction;
                cmdGroenDetail.Transaction = transaction;
                cmdBouwFirma.Transaction = transaction;
                cmdExternePartner.Transaction = transaction;

               
                cmdStadDetail.CommandText = StadDetailQuery;
                cmdStadDetail.Parameters.Add(new SqlParameter("@Vergunningstatus", System.Data.SqlDbType.Int));
                cmdStadDetail.Parameters.Add(new SqlParameter("@ArchitecturaleWaarde", System.Data.SqlDbType.Bit)); 
                cmdStadDetail.Parameters.Add(new SqlParameter("@Toegankelijkheid", System.Data.SqlDbType.Int));
                cmdStadDetail.Parameters.Add(new SqlParameter("@Bezienswaardigheid", System.Data.SqlDbType.Int));
                cmdStadDetail.Parameters.Add(new SqlParameter("@InfobordVoorzien", System.Data.SqlDbType.Int));
                cmdStadDetail.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));


                cmdBouwFirma.CommandText = BouwfirmaQuery;
                cmdBouwFirma.Parameters.Add(new SqlParameter("@StadDetailID", System.Data.SqlDbType.Int)); 
                cmdBouwFirma.Parameters.Add(new SqlParameter("@PartnerID", System.Data.SqlDbType.Int));

                try {
                
                    cmdLocatie.Parameters["@Gemeente"].Value = project.Locatie.Gemeente;
                    cmdLocatie.Parameters["@Postcode"].Value = project.Locatie.Postcode;
                    cmdLocatie.Parameters["@Straat"].Value = project.Locatie.Straat;
                    cmdLocatie.Parameters["@Huisnummer"].Value = project.Locatie.Huisnummer;
                    cmdLocatie.Parameters["@Wijk"].Value = string.IsNullOrEmpty(project.Locatie.Wijk) ? DBNull.Value : project.Locatie.Wijk;

                    
                    int idLocatie = (int)cmdLocatie.ExecuteScalar();

                    
                    cmdProject.Parameters["@Titel"].Value = project.Titel;
                    cmdProject.Parameters["@StartDatum"].Value = project.StartDatum;
                    cmdProject.Parameters["@Beschrijving"].Value = string.IsNullOrEmpty(project.Beschrijving) ? DBNull.Value : project.Beschrijving;
                    cmdProject.Parameters["@Status"].Value = (int)project.Status;
                    cmdProject.Parameters["@LocatieID"].Value = idLocatie; 

                    
                    int idProject = (int)cmdProject.ExecuteScalar();

                    
                    foreach (ProjectDetail detail in project.Details) {
                        if (detail.GetType() == typeof(StadDetail)) {
                            StadDetail stad = (StadDetail)detail;

                            cmdStadDetail.Parameters["@VergunningStatus"].Value = (int)stad.VergunningStatus;
                            cmdStadDetail.Parameters["@ArchitecturaleWaarde"].Value = Convert.ToInt32(stad.ArchitecturaleWaarde); 
                            cmdStadDetail.Parameters["@Toegankelijkheid"].Value = (int)stad.Toegankelijkheid;
                            cmdStadDetail.Parameters["@Bezienswaardigheid"].Value = Convert.ToInt32(stad.Bezienswaardigheid);
                            cmdStadDetail.Parameters["@InfobordVoorzien"].Value = Convert.ToInt32(stad.InfoBordVoorzien);
                            cmdStadDetail.Parameters["@ProjectID"].Value = idProject; 

                            int idStadDetail = (int)cmdStadDetail.ExecuteScalar();

                           
                            foreach (Partner firma in stad.Bouwfirmas) {
                                cmdBouwFirma.Parameters["@StadDetailID"].Value = idStadDetail;
                                cmdBouwFirma.Parameters["@PartnerID"].Value = firma.Id;
                                cmdBouwFirma.ExecuteNonQuery();
                            }
                        }
                        else if (detail.GetType() == typeof(WonenDetail)) {
                            WonenDetail wonen = (WonenDetail)detail;

                            cmdWonenDetail.Parameters["@AantalWooneenheden"].Value = wonen.AantalEenheden;
                            cmdWonenDetail.Parameters["@TypeWoonVorm"].Value = wonen.Woningtypes;
                            cmdWonenDetail.Parameters["@RondLeidingMogelijk"].Value = Convert.ToInt32(wonen.Rondleidingen);
                            cmdWonenDetail.Parameters["@ShowWoningMogelijk"].Value = Convert.ToInt32(wonen.Showwoningen);
                            cmdWonenDetail.Parameters["@ArchitecturaleScore"].Value = wonen.ArchitecturaleScore;
                            cmdWonenDetail.Parameters["@SamenwerkingErfgoed"].Value = Convert.ToInt32(wonen.ErfgoedSamenwerking);
                            cmdWonenDetail.Parameters["@ProjectID"].Value = idProject; 

                            cmdWonenDetail.ExecuteNonQuery(); 
                        }
                        else if (detail.GetType() == typeof(GroenDetail)) 
                        {
                            GroenDetail groen = (GroenDetail)detail;

                            cmdGroenDetail.Parameters["@Oppervlakte"].Value = groen.Oppervlakte;
                            cmdGroenDetail.Parameters["@Biodiversiteitscore"].Value = groen.Biodiversiteit;
                            cmdGroenDetail.Parameters["@AantalWandelpaden"].Value = groen.Wandelpaden;
                            cmdGroenDetail.Parameters["@BeschikbareFaciliteit"].Value = groen.Faciliteiten;
                            cmdGroenDetail.Parameters["@ToeristischeRoute"].Value = Convert.ToInt32(groen.ToeristischeRoute);
                            cmdGroenDetail.Parameters["@BezoekersBeoordeling"].Value = groen.Beoordeling;
                            cmdGroenDetail.Parameters["@ProjectID"].Value = idProject; 

                            cmdGroenDetail.ExecuteNonQuery();
                        }
                    }


                    foreach (ProjectPartner p in project.Partners) {
                        cmdExternePartner.Parameters["@ProjectID"].Value = idProject;
                        cmdExternePartner.Parameters["@PartnerID"].Value = p.Partner.id;
                        cmdExternePartner.Parameters["@Rolomschrijving"].Value = p.Rolbeschrijving;
                        cmdExternePartner.ExecuteNonQuery();
                    }



                        transaction.Commit();
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }   



        

        public void ProjectVerwijderen(Project project) {

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            using (SqlCommand cmd = conn.CreateCommand()) {

                string sql = "DELETE FROM Project WHERE id=@id";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", project.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }


        }

        public void PartnerVerwijderen(Partner partner) {

            string sql = "DELETE FROM Partner where id = @id";

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            using (SqlCommand cmd = conn.CreateCommand()) {

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", partner.Id);
                conn.Open();
                cmd.ExecuteNonQuery();


            }


        }

        public Project GeefProject(int id) {

            Project project = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring)) {
                conn.Open();

                
                string ProLocQuery = "SELECT p.ID, p.Titel, p.StartDatum, p.Beschrijving, p.Status, p.LocatieID, l.Gemeente, l.Postcode, l.Straat, l.Huisnummer, l.Wijk FROM Project p INNER JOIN Locatie l ON p.LocatieID = l.LocatieID WHERE p.ID = @id;";

                using (SqlCommand cmd = conn.CreateCommand()) {
                    cmd.CommandText = ProLocQuery;
                    cmd.Parameters.AddWithValue("@id", id);

                 
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                         
                            Locatie locatie1 = new Locatie(
                                (int)reader["LocatieID"],
                                (string)reader["Gemeente"], 
                                (string)reader["Postcode"],
                                (string)reader["Straat"],
                                (string)reader["Huisnummer"],
                                (string)reader["Wijk"]
                            );




                            project = new Project(
                                (int)reader["ID"],
                                (string)reader["Titel"],
                                (DateTime)reader["StartDatum"],
                                (string)reader["Beschrijving"],
                                (Enums.ProjectStatus)reader["Status"],
                                locatie1 
                            );

                            project.Details = new List<ProjectDetail>();
                            project.Partners = new List<ProjectPartner>();
                        }
                    } 
                }

          

                
                string StadQuery = "SELECT * FROM StadDetail WHERE ProjectID = @id";
                using (SqlCommand cmd = conn.CreateCommand()) {
                    cmd.CommandText = StadQuery;
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            StadDetail staddetail = new StadDetail(
                                (int)reader["StadDetailID"], 
                                (Enums.VergunningStatus)reader["Vergunningstatus"],
                                (bool)reader["ArchitecturaleWaarde"], 
                                (Enums.Toegankelijkheid)reader["Toegankelijkheid"],
                                (bool)reader["Bezienswaardigheid"],
                                 (bool)reader["InfobordVoorzien"]
                            );

                            
                            project.Details.Add(staddetail);
                        }
                    }
                }

                string WonenQuery = "SELECT * FROM InnovatiefwonenDetail WHERE ProjectID = @id";
                using (SqlCommand cmd = conn.CreateCommand()) {
                    cmd.CommandText = WonenQuery; 
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            WonenDetail wonenDetail = new WonenDetail(
                                (int)reader["InnovatiefwonenDetailID"],
                                (int)reader["AantalWoonEenheden"],
                                (string)reader["TypeWoonVorm"],
                                (bool)reader["RondLeidingMogelijk"],
                                (bool)reader["ShowWoningMogelijk"],
                                (int)reader["ArchitecturaleScore"],
                                (bool)reader["SamenwerkingErfgoed"]
                            );

                            project.Details.Add(wonenDetail); 
                        }
                    }
                }

                
                string GroenQuery = "SELECT * FROM GroenDetail WHERE ProjectID = @id";
                using (SqlCommand cmd = conn.CreateCommand()) {
                    cmd.CommandText = GroenQuery; 
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            GroenDetail groenDetail = new GroenDetail(
                                (int)reader["GroenDetailID"],
                                (decimal)reader["Oppervlakte"],
                                (int)reader["BiodiversiteitScore"],
                                (int)reader["AantalWandelpaden"],
                                (string)reader["BeschikbareFaciliteit"],
                                (bool)reader["ToeristischeRoute"],
                                (int)reader["BezoekersBeoordeling"]
                            );

                            project.Details.Add(groenDetail); 
                        }
                    }
                }
            }

            return project; 

        public Project UpdateProject(int id) {

            string sql = "UPDATE INTO Project"

        }
    }

}