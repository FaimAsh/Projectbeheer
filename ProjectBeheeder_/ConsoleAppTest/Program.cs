using Microsoft.Extensions.Configuration;
using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderUtil;
using static ProjectBeheerderBL.Domein.Enums;


var builder = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);

var config = builder.Build();
string connectionstring = config.GetConnectionString("SQLServerConnection");
string databaseType = config.GetSection("appsettings")["databasetype"];


//ProjectBeheerder projectBeheerder = new ProjectBeheerder(,IFileWriter);




public class ConsoleAppTest {
    private readonly IProjectRepository repository;

    public ConsoleAppTest(IProjectRepository repository) {
        this.repository = repository;






        // ==========================================
        // DUMMY PARTNERS 
        // (We gaan ervan uit dat je Partner constructor zo werkt: ID, Naam, Enum)
        // ==========================================
        Partner besix = new Partner(1, "Besix Bouwgroep",PartnerType.Organisatie); // Of voeg de enum toe als dat in je constructor staat
        Partner natuurpunt = new Partner(2, "Natuurpunt",PartnerType.Burger);

        // ==========================================
        // DOSSIER 1: STADSONTWIKKELING GENT
        // ==========================================

        // 1. Eerst de Locatie aanmaken via de constructor (aanname van de volgorde)
        Locatie locatie1 = new Locatie("Gent", "9000", "Korenmarkt", "1", "Centrum");

        // 2. Dan het Project aanmaken via de constructor
        Project project1 = new Project("Renovatie Korenmarkt", new DateTime(2026, 5, 1), "Volledige heraanleg met kasseien.", ProjectStatus.Planning, locatie1);

        // Zorg dat de lijstjes bestaan (als dit niet al in je Project constructor gebeurt!)
        project1.Details = new List<ProjectDetail>();
        project1.Partners = new List<ProjectPartner>();

        // 3. Jouw StadDetail aanmaken! 
        // HIER GEBRUIKEN WE EXACT JOUW 5-PARAMETER CONSTRUCTOR!
        StadDetail stad = new StadDetail(
            VergunningStatus.Goedgekeurd,       // vergunningStatus
            true,                               // architecturaleWaarde
            Toegankelijkheid.VolledigOpenbaar,  // toegankelijkheid
            true,                               // bezienswaardigheid
            false                               // infobordenVoorzien
        );

        // Omdat de lijst met bouwfirmas niet in je constructor zit, vullen we die er netjes onder aan:
        stad.Bouwfirmas = new List<Partner>();
        stad.Bouwfirmas.Add(besix);

        // En we stoppen het StadDetail in het project
        project1.Details.Add(stad);

        // 4. Partner aan het project koppelen
        ProjectPartner projectPartner = new ProjectPartner(besix, "Hoofdaannemer"); // Aanname van constructor
        project1.Partners.Add(projectPartner);


        // ==========================================
        // DOSSIER 2: PARK & ECO-WONINGEN LOKEREN
        // ==========================================
        Locatie locatie2 = new Locatie("Lokeren", "9160", "Daknamstraat", "55", "Daknam");
        Project project2 = new Project("Groen Wonen Lokeren", new DateTime(2026, 9, 15), "Aanleg park en eco-woningen.", ProjectStatus.Uitvoering, locatie2);
        project2.Details = new List<ProjectDetail>();
        project2.Partners = new List<ProjectPartner>();

        // GroenDetail aanmaken (Aanname van jouw constructor volgorde)
        GroenDetail groen = new GroenDetail(
            5000.50m, // Oppervlakte
            8,        // Biodiversiteit
            3,        // Wandelpaden
            "Bankjes, Vuilnisbakken",
            true,     // ToeristischeRoute
            9         // Beoordeling
        );
        project2.Details.Add(groen);

        // WonenDetail aanmaken (Aanname van jouw constructor volgorde)
        WonenDetail wonen = new WonenDetail(
            10,                 // Aantal eenheden
            "Houtskeletbouw",   // Type
            true,               // Rondleiding
            true,               // Showwoning
            7,                  // Score
            false               // Erfgoed
        );
        project2.Details.Add(wonen);

        // Koppel de partner
        ProjectPartner adviesPartner = new ProjectPartner(natuurpunt, "Adviseur Biodiversiteit");
        project2.Partners.Add(adviesPartner);


        // ==========================================
        // OPSLAAN IN DE DATABASE!
        // ==========================================
        Console.WriteLine("Dossier 1 (Gent) inladen...");
        repository.AllesImporteren(project1);
        Console.WriteLine("Succes! Project 1 staat in de database.");

        Console.WriteLine("Dossier 2 (Lokeren) inladen...");
        repository.AllesImporteren(project2);
        Console.WriteLine("Succes! Project 2 staat in de database.");


    }

}