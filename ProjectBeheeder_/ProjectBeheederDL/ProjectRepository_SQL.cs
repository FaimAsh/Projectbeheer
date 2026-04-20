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

            string ProjectQuery = "INSERT INTO Project (Titel,StartDatum,Beschrijving,Status,LocatieID) VALUES (@Titel,@StartDatum,@Beschrijving,@Status,@LocatieID);";
            string LocatieQuery = "INSERT INTO Locatie (Gemeente,Postcode,Straat,Huisnummer,wijk) VALUES (@Gemeente,@Postcode,@Straat,@Huisnummer,@wijk);";
            string StadDetailQuery = "INSERT INTO StadDetail (Vergunningstatus,AchitecturaleWaarde,Toegankelijkheid,Bezienswaardigheid,InfobordVoorzien,ProjectID) VALUES (@Vergunningstatus,@AchitecturaleWaarde,@Toegankelijkheid,@Bezienswaardigheid,@InfobordVoorzien,@ProjectID);";
            string WonenDetailQuery = "INSERT INTO InnovatiefWoenenDetail (AantalWoonEenheden,TypeWoonVorm,RondLeidingMogelijk,ShowWoningMogelijk,ArchitecturaleScore,SamenwerkingErfgoed,ProjectID) VALUES (@InfobordVoorzien,@TypeWoonVorm,@RondLeidingMogelijk,@ShowWoningMogelijk,@ArchitecturaleScore,@SamenwerkingErfgoed,@ProjectID);";
            string GroenDetailQuery = "INSERT INTO GroenDetail (Oppevlakte,BiodiversiteitScore,AantalWandelpaden,BeschikbareFaciliteit,ToeristischeRoute,BezoekersBeoordering,ProjectID) VALUES (@Oppervlakte,@Biodiversiteitscore,@AantalWandelpaden,@BeschikbareFaciliteit,@ToeristischeRoute,@BezoekersBeoordering,@ProjectID);";
            string BouwfirmaQuery = "INSERT INTO StadsOntwikkeling_Partner (StadDetailID,PartnerID) VALUES (@StadDetailID,@PartnerID);";
            string ExternePartnerQuery = "INSERT INTO Project_Partner (ProjectID,PartnerID,Rolomschrijving) VALUES (@ProjectID,@PartnerID,@Rolomschrijving);";


            using (SqlConnection conn = new SqlConnection(_connectionstring)) 
            using (SqlCommand cmdProject = conn.CreateCommand())
            using (SqlCommand cmdLocatie = conn .CreateCommand())
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

                cmdProject.CommandText = ProjectQuery;
                cmdProject.Parameters.Add(new SqlParameter("@Titel",System.Data.SqlDbType.NVarChar));
                cmdProject.Parameters.Add(new SqlParameter("@StartDatum", System.Data.SqlDbType.DateTime));
                cmdProject.Parameters.Add(new SqlParameter("@Beschrijving", System.Data.SqlDbType.NVarChar));
                cmdProject.Parameters.Add(new SqlParameter("@Status",System.Data.SqlDbType.Int));
                cmdProject.Parameters.Add(new SqlParameter("@LocatieID", System.Data.SqlDbType.Int));


                cmdLocatie.CommandText = LocatieQuery;
                cmdLocatie.Parameters.Add(new SqlParameter("@Gemeente", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Postcode", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Straat", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Huisnummer", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Wijk", System.Data.SqlDbType.NVarChar));


                cmdStadDetail.CommandText = StadDetailQuery;
                cmdStadDetail.Parameters.Add(new SqlParameter("@Veregunningstatus", System.Data.SqlDbType.Int));
                cmdStadDetail.Parameters.Add(new SqlParameter("@AchitecturaleWaarde", System.Data.SqlDbType.Bit));
                cmdStadDetail.Parameters.Add(new SqlParameter("@Toegankelijkheid", System.Data.SqlDbType.Int));
                cmdStadDetail.Parameters.Add(new SqlParameter("@Bezienswaardigheid", System.Data.SqlDbType.Bit));
                cmdStadDetail.Parameters.Add(new SqlParameter("@InfobordVoorzien", System.Data.SqlDbType.Bit));
                cmdStadDetail.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));


                cmdWonenDetail.CommandText = WonenDetailQuery;
                cmdWonenDetail.Parameters.Add(new SqlParameter("@AantalWoonEenheden", System.Data.SqlDbType.Int));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@TypeWoonVorm", System.Data.SqlDbType.NVarChar));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@RondLeidingMogelijk", System.Data.SqlDbType.Bit));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ShowWoningMogelijk", System.Data.SqlDbType.Bit));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ArchitecturaleScore", System.Data.SqlDbType.Int));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@SamenwerkingErfgoed", System.Data.SqlDbType.Bit));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));


                cmdGroenDetail.CommandText = GroenDetailQuery;
                cmdGroenDetail.Parameters.Add(new SqlParameter("@Oppervlakte", System.Data.SqlDbType.Decimal));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@Biodiversiteitscore", System.Data.SqlDbType.Int));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@AantalWandelpaden", System.Data.SqlDbType.Int));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@BeschikbareFaciliteit", System.Data.SqlDbType.NVarChar));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@ToeristischeRoute", System.Data.SqlDbType.Bit));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@BezoekersBeoordering", System.Data.SqlDbType.Int));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));


                cmdBouwFirma.CommandText = BouwfirmaQuery;
                cmdBouwFirma.Parameters.Add(new SqlParameter("@StadDetailID", System.Data.SqlDbType.Int));
                cmdBouwFirma.Parameters.Add(new SqlParameter("@PartnerID",System.Data.SqlDbType.Int));                


                cmdExternePartner.CommandText = ExternePartnerQuery;
                cmdExternePartner.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));
                cmdExternePartner.Parameters.Add(new SqlParameter("@PartnerID", System.Data.SqlDbType.Int));
                cmdExternePartner.Parameters.Add(new SqlParameter("@Rolomschrijving", System.Data.SqlDbType.NVarChar));

                try {

                    cmdLocatie.Parameters["@Gemeente"].Value = project.Locatie.Gemeente;
                    cmdLocatie.Parameters["@Postcode"].Value = project.Locatie.Postcode;
                    cmdLocatie.Parameters["@Straat"].Value = project.Locatie.Straat;
                    cmdLocatie.Parameters["@Huisnummer"].Value = project.Locatie.Huisnummer;
                    cmdLocatie.Parameters["@Wijk"].Value = project.Locatie.Wijk;

                    //int idLocatie = (int)cmdLocatie.ExecuteScalar();


                    cmdProject.Parameters["@Titel"].Value = project.Titel;
                    cmdProject.Parameters["@StartDatum"].Value = project.StartDatum;
                    cmdProject.Parameters["@Beschrijving"].Value = project.Beschrijving;
                    cmdProject.Parameters["@Status"].Value = project.Status;
                    cmdProject.Parameters["@LocatieID"].Value = project.Locatie.LocatieId;

                    foreach (ProjectDetail detail in project.Details) {

                        if (detail.GetType() == typeof(StadDetail)) {

                            StadDetail stad = (StadDetail)detail;

                            cmdStadDetail.Parameters["@VergunningStatus"].Value = (int)stad.VergunningStatus;
                            cmdStadDetail.Parameters["@AchitecturaleWaarde"].Value = Convert.ToInt32(stad.VergunningStatus);
                            cmdStadDetail.Parameters["@Toegankelijkheid"].Value = Convert.ToInt32(stad.Toegankelijkheid);
                            cmdStadDetail.Parameters["@Bezienswaardigheid"].Value = Convert.ToInt32(stad.Bezienswaardigheid);
                            cmdStadDetail.Parameters["@InfobordVoorzien"].Value = Convert.ToInt32(stad.InfoBordVoorzien);
                            cmdStadDetail.Parameters["@ProjectID"].Value = project.Id;
                            cmdStadDetail.ExecuteNonQuery();

                            int idStadDetail = (int)cmdStadDetail.ExecuteScalar();


                            foreach (Partner firma in stad.Bouwfirmas) {
                                cmdBouwFirma.Parameters["@StadID"].Value = idStadDetail;
                                cmdBouwFirma.Parameters["@PartID"].Value = firma.Id;
                                cmdBouwFirma.ExecuteNonQuery();

                            }
                        }

                        else if (detail.GetType() == typeof(WonenDetail)) {

                            WonenDetail wonen = (WonenDetail)detail;

                            cmdWonenDetail.Parameters["@AantalWoonEenheden"].Value = wonen.AantalEenheden;
                            cmdWonenDetail.Parameters["@TypeWoonVorm"].Value = wonen.Woningtypes;
                            cmdWonenDetail.Parameters["@RondLeidingMogelijk"].Value = wonen.Rondleidingen;
                            cmdWonenDetail.Parameters["@ShowWoningMogelijk"].Value = wonen.Showwoningen;                        ///////
                            cmdWonenDetail.Parameters["@ArchitecturaleScore"].Value = wonen.ArchitecturaleScore;
                            cmdWonenDetail.Parameters["@SamenwerkingErfgoed"].Value =
                            cmdWonenDetail.Parameters["@ProjectID"].Value = project.Id;

                        }

                        else if (detail.GetType() == typeof(WonenDetail)) {

                            GroenDetail groen = (GroenDetail)detail;

                            cmdGroenDetail.Parameters["@Oppervlakte"].Value = groen.Oppervlakte;
                            cmdGroenDetail.Parameters["@Biodiversiteitscore"].Value = groen.Biodiversiteit;
                            cmdGroenDetail.Parameters["@AantalWandelpaden"].Value = groen.Wandelpaden;
                            cmdGroenDetail.Parameters["@BeschikbareFaciliteit"].Value = groen.Faciliteiten;
                            cmdGroenDetail.Parameters["@ToeristischeRoute"].Value = Convert.ToInt32(groen.ToeristischeRoute);
                            cmdGroenDetail.Parameters["@BezoekersBeoordering"].Value = groen.Beoordeling;
                            cmdGroenDetail.Parameters["@ProjectID"].Value = project.Id;
                        }

                        transaction.Commit();

                    }
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    throw ex;












                }







                }



        }

        public void ProjectVerwijderen(Project project) {

            using(SqlConnection conn = new SqlConnection(_connectionstring))
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
                cmd.Parameters.AddWithValue("@id",partner.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            

        }


    }

        public Project GeefProject(Project id)
}
