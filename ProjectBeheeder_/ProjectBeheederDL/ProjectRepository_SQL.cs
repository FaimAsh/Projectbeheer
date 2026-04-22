using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Interfaces;
using System.Data;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Transactions;
using static ProjectBeheerderBL.Domein.Enums;


namespace ProjectBeheederDL
{
    public class ProjectRepository_SQL : IProjectRepository
    {

        string _connectionstring;

        public ProjectRepository_SQL(string connectionstring)
        {

            _connectionstring = connectionstring;
        }

        public void AllesImporteren(Project project)
        {
            string ProjectQuery = "INSERT INTO Project (Titel,StartDatum,Beschrijving,Status,LocatieID) OUTPUT INSERTED.ProjectID VALUES (@Titel,@StartDatum,@Beschrijving,@Status,@LocatieID);";
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
            using (SqlCommand cmdExternePartner = conn.CreateCommand())
            {

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
                cmdProject.Parameters.Add(new SqlParameter("@Titel", System.Data.SqlDbType.NVarChar));
                cmdProject.Parameters.Add(new SqlParameter("@StartDatum", System.Data.SqlDbType.DateTime));
                cmdProject.Parameters.Add(new SqlParameter("@Beschrijving", System.Data.SqlDbType.NVarChar));
                cmdProject.Parameters.Add(new SqlParameter("@Status", System.Data.SqlDbType.Int));
                cmdProject.Parameters.Add(new SqlParameter("@LocatieID", System.Data.SqlDbType.Int));

                cmdGroenDetail.CommandText = GroenDetailQuery;
                cmdGroenDetail.Parameters.Add(new SqlParameter("@Oppervlakte", System.Data.SqlDbType.Decimal));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@Biodiversiteitscore", System.Data.SqlDbType.Int));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@AantalwandelPaden", System.Data.SqlDbType.Int));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@BeschikbareFaciliteit", System.Data.SqlDbType.NVarChar));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@ToeristischeRoute", System.Data.SqlDbType.Bit));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@BezoekersBeoordeling", System.Data.SqlDbType.Int));
                cmdGroenDetail.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));

                cmdWonenDetail.CommandText = WonenDetailQuery;
                cmdWonenDetail.Parameters.Add(new SqlParameter("@AantalWooneenheden", System.Data.SqlDbType.Int));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@TypeWoonVorm", System.Data.SqlDbType.NVarChar));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@RondLeidingMogelijk", System.Data.SqlDbType.Bit));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ShowWoningMogelijk", System.Data.SqlDbType.Bit));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ArchitecturaleScore", System.Data.SqlDbType.Int));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@SamenwerkingErfgoed", System.Data.SqlDbType.Bit));
                cmdWonenDetail.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));

                cmdLocatie.CommandText = LocatieQuery;
                cmdLocatie.Parameters.Add(new SqlParameter("@Gemeente", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Postcode", System.Data.SqlDbType.VarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Straat", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Huisnummer", System.Data.SqlDbType.NVarChar));
                cmdLocatie.Parameters.Add(new SqlParameter("@Wijk", System.Data.SqlDbType.NVarChar));

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

                cmdExternePartner.CommandText = ExternePartnerQuery;
                cmdExternePartner.Parameters.Add(new SqlParameter("@ProjectID", System.Data.SqlDbType.Int));
                cmdExternePartner.Parameters.Add(new SqlParameter("@PartnerID", System.Data.SqlDbType.Int));
                cmdExternePartner.Parameters.Add(new SqlParameter("@RolOmschrijving", System.Data.SqlDbType.NVarChar));

                try
                {

                    cmdLocatie.Parameters["@Gemeente"].Value = project.Locatie.Gemeente;
                    cmdLocatie.Parameters["@Postcode"].Value = project.Locatie.Postcode;
                    cmdLocatie.Parameters["@Straat"].Value = project.Locatie.Straat;
                    cmdLocatie.Parameters["@Huisnummer"].Value = project.Locatie.Huisnummer;
                    cmdLocatie.Parameters["@Wijk"].Value = project.Locatie.Wijk;


                    int idLocatie = (int)cmdLocatie.ExecuteScalar();


                    cmdProject.Parameters["@Titel"].Value = project.Titel;
                    cmdProject.Parameters["@StartDatum"].Value = project.StartDatum;
                    cmdProject.Parameters["@Beschrijving"].Value = project.Beschrijving;
                    cmdProject.Parameters["@Status"].Value = (int)project.Status;
                    cmdProject.Parameters["@LocatieID"].Value = idLocatie;


                    int idProject = (int)cmdProject.ExecuteScalar();


                    foreach (ProjectDetail detail in project.Details)
                    {
                        if (detail.GetType() == typeof(StadDetail))
                        {
                            StadDetail stad = (StadDetail)detail;

                            cmdStadDetail.Parameters["@VergunningStatus"].Value = (int)stad.VergunningStatus;
                            cmdStadDetail.Parameters["@ArchitecturaleWaarde"].Value = Convert.ToInt32(stad.ArchitecturaleWaarde);
                            cmdStadDetail.Parameters["@Toegankelijkheid"].Value = (int)stad.Toegankelijkheid;
                            cmdStadDetail.Parameters["@Bezienswaardigheid"].Value = Convert.ToInt32(stad.Bezienswaardigheid);
                            cmdStadDetail.Parameters["@InfobordVoorzien"].Value = Convert.ToInt32(stad.InfoBordVoorzien);
                            cmdStadDetail.Parameters["@ProjectID"].Value = idProject;

                            int idStadDetail = (int)cmdStadDetail.ExecuteScalar();


                            foreach (Partner firma in stad.Bouwfirmas)
                            {
                                cmdBouwFirma.Parameters["@StadDetailID"].Value = idStadDetail;
                                cmdBouwFirma.Parameters["@PartnerID"].Value = firma.Id;
                                cmdBouwFirma.ExecuteNonQuery();
                            }
                        }
                        else if (detail.GetType() == typeof(WonenDetail))
                        {
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


                    foreach (ProjectPartner p in project.Partners)
                    {
                        cmdExternePartner.Parameters["@ProjectID"].Value = idProject;
                        cmdExternePartner.Parameters["@PartnerID"].Value = p.Partner.Id;
                        cmdExternePartner.Parameters["@Rolomschrijving"].Value = p.RolBeschrijving ;
                        cmdExternePartner.ExecuteNonQuery();
                    }



                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public List<Partner> GeefPartners() {

            List<Partner> partner = new();

            using (SqlConnection conn = new SqlConnection(_connectionstring)) {

                conn.Open();

                string PartnerQuery = "SELECT * FROM Partner";

                using (SqlCommand cmd = conn.CreateCommand()) {

                    cmd.CommandText = PartnerQuery;

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        while (reader.Read()) {

                            Partner partner1 = new Partner((int)reader["PartnerID"],
                                (string)reader["Naam"],
                                (Enums.PartnerType)reader["TypePartner"]
                                );

                            partner.Add(partner1);

                            

                        }
                        return partner;
                    }
                }

            }
        }
        





       

        public void ProjectVerwijderen(Project project)
        {

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                string sql = "DELETE FROM Project WHERE Projectid=@id";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", project.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }


        }

        public void PartnerVerwijderen(Partner partner)
        {

            string sql = "DELETE FROM Partner where id = @id";

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", partner.Id);
                conn.Open();
                cmd.ExecuteNonQuery();


            }


        }

        public Project GeefProject(int id)
        {

            Project project = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();


                string ProLocQuery = "SELECT p.ID, p.Titel, p.StartDatum, p.Beschrijving, p.Status, p.LocatieID, l.Gemeente, l.Postcode, l.Straat, l.Huisnummer, l.Wijk FROM Project p INNER JOIN Locatie l ON p.LocatieID = l.LocatieID WHERE p.ProjectID = @id;";

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = ProLocQuery;
                    cmd.Parameters.AddWithValue("@ProjectId", id);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            Locatie locatie1 = new Locatie(
                                (int)reader["LocatieID"],
                                (string)reader["Gemeente"],
                                (string)reader["Postcode"],
                                (string)reader["Straat"],
                                (string)reader["Huisnummer"],
                                (string)reader["Wijk"]
                            );




                            project = new Project(
                                (int)reader["ProjectID"],
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
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = StadQuery;
                    cmd.Parameters.AddWithValue("@ProjectId", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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

                string WonenQuery = "SELECT * FROM InnovatiefwonenDetail WHERE ProjectID = @id AND ";
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = WonenQuery;
                    cmd.Parameters.AddWithValue("@ProjectId", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = GroenQuery;
                    cmd.Parameters.AddWithValue("@ProjectId", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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
        }

        public void UpdateProject(Project project)
        {

            string LocatieQuery = "UPDATE Locatie SET Gemeente=@Gemeente,Postcode=@Postcode,Straat=@Straat,Huisnummer=@Huisnummer,wijk=@wijk WHERE id = LocatieID;";
            string ProjectQuery = "UPDATE Project SET Titel=@Titel,StartDatum=@StartDatum,Status=@Status,Beschrijving=@BeSchrijving,Locatie=@Locatie WHERE id = ProjectID;";


            using (SqlConnection conn = new SqlConnection(_connectionstring))
            using (SqlCommand ProjectCmd = conn.CreateCommand())
            using (SqlCommand LocatieCmd = conn.CreateCommand())
            {

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {



                    LocatieCmd.Transaction = transaction;
                    ProjectCmd.Transaction = transaction;

                    LocatieCmd.CommandText = LocatieQuery;
                    LocatieCmd.Parameters.AddWithValue("@LocatieID", project.Locatie.LocatieId);
                    LocatieCmd.Parameters.AddWithValue("@Gemeente", project.Locatie.Gemeente);
                    LocatieCmd.Parameters.AddWithValue("@Postcode", project.Locatie.Postcode);
                    LocatieCmd.Parameters.AddWithValue("@Straat", project.Locatie.Straat);
                    LocatieCmd.Parameters.AddWithValue("@Huisnummer", project.Locatie.Huisnummer);
                    LocatieCmd.Parameters.AddWithValue("@Wijk", project.Locatie.Wijk);

                    LocatieCmd.ExecuteNonQuery();

                    ProjectCmd.CommandText = ProjectQuery;
                    ProjectCmd.Parameters.AddWithValue("@ProjectID", project.Id);
                    ProjectCmd.Parameters.AddWithValue("@Titel", project.Titel);
                    ProjectCmd.Parameters.AddWithValue("@StartDatum", project.StartDatum);
                    ProjectCmd.Parameters.AddWithValue("@Status", project.Status);
                    ProjectCmd.Parameters.AddWithValue("@Beschriving", project.Beschrijving);
                    ProjectCmd.Parameters.AddWithValue("@Locatie", project.Locatie);

                    ProjectCmd.ExecuteNonQuery();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }

        }


        public List<Project> Search(ProjectFilter filter)
        {
            var projects = new Dictionary<int, Project>();

            var projectPartnerIds = new Dictionary<int, HashSet<int>>();
            var stadPartnerIds = new Dictionary<int, HashSet<int>>();

            var hasGroen = new HashSet<int>();
            var hasStad = new HashSet<int>();
            var hasWonen = new HashSet<int>();

            using var conn = new SqlConnection(_connectionstring);
            conn.Open();

            using var cmd = new SqlCommand(BuildQuery(filter), conn);
            AddParameters(cmd, filter);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int projectId = Convert.ToInt32(reader["ID"]);

                if (!projects.TryGetValue(projectId, out var project))
                {
                    project = CreateProject(reader, projectId);
                    projects[projectId] = project;
                }

                EnsureGroenDetail(reader, project, projectId, hasGroen);
                EnsureStadDetail(reader, project, projectId, hasStad);
                EnsureWonenDetail(reader, project, projectId, hasWonen);

                AddProjectPartner(reader, project, projectId, projectPartnerIds);
                AddStadPartner(reader, project, projectId, stadPartnerIds);
            }

            return projects.Values.ToList();
        }

        // =========================
        // PROJECT CREATION
        // =========================
        private Project CreateProject(SqlDataReader reader, int projectId)
        {
            Locatie locatie = new Locatie(
                Convert.ToInt32(reader["LocatieID"]),
                reader["Gemeente"]?.ToString(),
                reader["Wijk"]?.ToString(),
                reader["Postcode"]?.ToString(),
                reader["Straat"]?.ToString(),
                reader["Huisnummer"]?.ToString()
            );

            Project p = new Project(
                projectId,
                reader["Titel"]?.ToString(),
                Convert.ToDateTime(reader["StartDatum"]),
                reader["Beschrijving"]?.ToString(),
                (ProjectStatus)Convert.ToInt32(reader["Status"]),
                locatie
            );

            p.Details = new List<ProjectDetail>();
            p.Partners = new List<ProjectPartner>();
            return p;
        }

        // =========================
        // DETAILS
        // =========================
        private void EnsureGroenDetail(SqlDataReader reader, Project project, int projectId, HashSet<int> hasGroen)
        {
            if (reader["GroenDetailID"] == DBNull.Value || !hasGroen.Add(projectId))
                return;

            project.Details.Add(new GroenDetail(
                Convert.ToInt32(reader["GroenDetailID"]),
                Convert.ToDecimal(reader["Oppervlakte"]),
                Convert.ToInt32(reader["Biodiversiteitscore"]),
                Convert.ToInt32(reader["AantalWandelpaden"]),
                reader["BeschikbareFaciliteit"]?.ToString(),
                Convert.ToBoolean(reader["ToeristischeRoute"]),
                Convert.ToInt32(reader["BezoekersBeoordeling"])
            ));
        }

        private void EnsureStadDetail(SqlDataReader reader, Project project, int projectId, HashSet<int> hasStad)
        {
            if (reader["StadDetailID"] == DBNull.Value || !hasStad.Add(projectId))
                return;

            project.Details.Add(new StadDetail(
                Convert.ToInt32(reader["StadDetailID"]),
                (VergunningStatus)Convert.ToInt32(reader["Vergunningstatus"]),
                Convert.ToBoolean(reader["ArchitecturaleWaarde"]),
                (Toegankelijkheid)Convert.ToInt32(reader["Toegankelijkheid"]),
                Convert.ToBoolean(reader["Bezienswaardigheid"]),
                Convert.ToBoolean(reader["Infobordvoorzien"])
            )
            {
                Bouwfirmas = new List<Partner>(),
                ToeristischeWaarde = Convert.ToBoolean(reader["ToeristischeWaarde"])
            });
        }

        private void EnsureWonenDetail(SqlDataReader reader, Project project, int projectId, HashSet<int> hasWonen)
        {
            if (reader["WonenDetailID"] == DBNull.Value || !hasWonen.Add(projectId))
                return;

            project.Details.Add(new WonenDetail(
                Convert.ToInt32(reader["WonenDetailID"]),
                Convert.ToInt32(reader["AantalWooneenheden"]),
                reader["TypeWoonVorm"]?.ToString() ?? string.Empty,
                Convert.ToBoolean(reader["RondLeidingMogelijk"]),
                Convert.ToBoolean(reader["ShowWoningMogelijk"]),
                Convert.ToInt32(reader["ArchitecturaleScore"]),
                Convert.ToBoolean(reader["SamenwerkingErfgoed"])
            ));
        }

        // =========================
        // PARTNERS
        // =========================
        private void AddProjectPartner(SqlDataReader reader, Project project, int projectId,
            Dictionary<int, HashSet<int>> projectPartnerIds)
        {
            if (reader["PartnerId"] == DBNull.Value)
                return;

            int partnerId = Convert.ToInt32(reader["PartnerId"]);

            if (!projectPartnerIds.TryGetValue(projectId, out var set))
            {
                set = new HashSet<int>();
                projectPartnerIds[projectId] = set;
            }

            if (!set.Add(partnerId))
                return;

            var partner = new Partner(
                partnerId,
                reader["PartnerNaam"]?.ToString(),
                (PartnerType)Convert.ToInt32(reader["PartnerType"])
            );

            var rol = reader["Rolomschrijving"]?.ToString();

            var projectPartner = new ProjectPartner(partner, rol);

            project.Partners.Add(projectPartner);
        }   

        private void AddStadPartner(SqlDataReader reader, Project project, int projectId,
            Dictionary<int, HashSet<int>> stadPartnerIds)
        {
            if (reader["StadPartnerId"] == DBNull.Value)
                return;

            int partnerId = Convert.ToInt32(reader["StadPartnerId"]);

            if (!stadPartnerIds.TryGetValue(projectId, out var set))
            {
                set = new HashSet<int>();
                stadPartnerIds[projectId] = set;
            }

            if (!set.Add(partnerId))
                return;

            var stad = project.Details.OfType<StadDetail>().FirstOrDefault();
            if (stad == null) return;

            stad.Bouwfirmas.Add(new Partner(partnerId, reader["StadPartnerNaam"]?.ToString(), (PartnerType)Convert.ToInt32(reader["StadPartnerType"]))) ;
        }

        // =========================
        // QUERY
        // =========================
        private string BuildQuery(ProjectFilter filter)
        {
            var query = @"
SELECT 
    p.ID, p.Titel, p.StartDatum, p.Status, p.Beschrijving,

    l.LocatieID, l.Gemeente, l.Wijk, l.Postcode, l.Straat, l.Huisnummer,

    gd.GroenDetailID, gd.Oppervlakte, gd.Biodiversiteitscore, gd.AantalWandelpaden,
    gd.BeschikbareFaciliteit, gd.ToeristischeRoute, gd.BezoekersBeoordeling,

    sd.StadDetailID, sd.Vergunningstatus, sd.ArchitecturaleWaarde, sd.Toegankelijkheid,
    sd.Bezienswaardigheid, sd.Infobordvoorzien, sd.ToeristischeWaarde,

    wd.WonenDetailID, wd.AantalWooneenheden, wd.TypeWoonVorm, wd.RondLeidingMogelijk,
    wd.ShowWoningMogelijk, wd.ArchitecturaleScore, wd.SamenwerkingErfgoed,

    pr.PartnerID AS PartnerId, pr.Naam AS PartnerNaam, pr.TypePartner AS PartnerType,
    pp.Rolomschrijving,

    sop.PartnerID AS StadPartnerId, sp.Naam AS StadPartnerNaam, sp.TypePartner AS StadPartnerType

FROM Project p
JOIN Locatie l ON p.LocatieID = l.LocatieID
LEFT JOIN GroenDetail gd ON gd.ProjectID = p.ID
LEFT JOIN StadDetail sd ON sd.ProjectID = p.ID
LEFT JOIN InnovatiefwonenDetail wd ON wd.ProjectID = p.ID
LEFT JOIN Project_Partner pp ON pp.ProjectID = p.ID
LEFT JOIN Partner pr ON pr.PartnerID = pp.PartnerID
LEFT JOIN StadsOntwikkeling_Partner sop ON sop.StadDetailID = sd.StadDetailID
LEFT JOIN Partner sp ON sp.PartnerID = sop.PartnerID
WHERE 1=1";

            if (!string.IsNullOrEmpty(filter.Wijk))
                query += " AND l.Wijk LIKE @Wijk";

            if (filter.StartDatumVan != null)
                query += " AND p.StartDatum >= @StartVan";

            if (filter.StartDatumTot != null)
                query += " AND p.StartDatum <= @StartTot";

            if (filter.Status != null)
                query += " AND p.Status = @Status";

            return query;
        }

        // =========================
        // PARAMETERS
        // =========================
        private void AddParameters(SqlCommand cmd, ProjectFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Wijk))
                cmd.Parameters.Add("@Wijk", SqlDbType.NVarChar).Value = $"%{filter.Wijk}%";

            if (filter.StartDatumVan != null)
                cmd.Parameters.Add("@StartVan", SqlDbType.DateTime).Value = filter.StartDatumVan;

            if (filter.StartDatumTot != null)
                cmd.Parameters.Add("@StartTot", SqlDbType.DateTime).Value = filter.StartDatumTot;

            if (filter.Status != null)
                cmd.Parameters.Add("@Status", SqlDbType.Int).Value = (int)filter.Status;
        }
    }
}
