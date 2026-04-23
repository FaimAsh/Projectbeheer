using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Exeptions;
using ProjectBeheerderUtil;
using System.Windows;
using System.Windows.Controls;
using static ProjectBeheerderBL.Domein.Enums;

namespace WpfAppProjectBeheeder
{
    internal class PartnerWeergave
    {
        public Partner Partner { get; set; }
        public bool IsToegevoegd { get; set; }
        public string Display => $"{Partner.Naam}  ({Partner.PartnerType}){(IsToegevoegd ? "  ✓" : "")}";
        public PartnerWeergave(Partner p) { Partner = p; }
        public bool IsOntkoppelt { get; set; }
    }

    public partial class NieuwProjectWindow : Window
    {
        private readonly ProjectBeheerder _service;
        private readonly Project? _bestand;
        private readonly ProjectFactory _factory = new ProjectFactory();

        private List<PartnerWeergave> _weergave = new();
        private List<PartnerRij> _partnerRijen = new();

        public NieuwProjectWindow(ProjectBeheerder service, Project? bestaand = null)
        {
            InitializeComponent();
            _service = service;
            _bestand = bestaand;

            LaadPartners();

            if (_bestand != null)
            {
                Title = "Project wijzigen";
                VulFormulierIn(_bestand);
                CmbType.IsEnabled = false;
            }
        }

        private void LaadPartners()
        {
            try
            {
                var all = _service.GeefPartners();
                _weergave = all.Select(p => new PartnerWeergave(p)).ToList();
            }
            catch { _weergave = new List<PartnerWeergave>(); }
            RefreshLijst();
        }

        private bool IsStads =>
            (CmbType?.SelectedItem as ComboBoxItem)?.Content?.ToString() == "StadsProject";

        private void TxtPartnerZoek_TextChanged(object sender, TextChangedEventArgs e)
            => RefreshLijst();

        private void RefreshLijst()
        {
            string zoek = TxtPartnerZoek?.Text.Trim().ToLower() ?? "";
            var gefilterd = string.IsNullOrEmpty(zoek)
                ? _weergave
                : _weergave.Where(w => w.Partner.Naam.ToLower().Contains(zoek)).ToList();

            LstPartners.ItemsSource = null;
            LstPartners.ItemsSource = gefilterd;
        }
      

        private void VoegPartnerToe_Click(object sender, RoutedEventArgs e)
        {
            if (LstPartners.SelectedItem is not PartnerWeergave pw)
            {
                MessageBox.Show("Selecteer een partner.", "Geen selectie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (pw.IsToegevoegd)
            {
                MessageBox.Show("Partner is al toegevoegd (grijs).", "Al toegevoegd",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string kat = (IsStads && ChkBouwfirma.IsChecked == true) ? "bouwfirma" : "algemeen";
            _partnerRijen.Add(new PartnerRij(pw.Partner, TxtRol.Text.Trim(), kat));

            pw.IsToegevoegd = true; // grijs
            TxtRol.Clear();
            ChkBouwfirma.IsChecked = false;
            RefreshLijst();
        }

        private void VerwijderKoppeling_Click(object sender, RoutedEventArgs e) {


           

            if (LstPartners.SelectedItem is not PartnerWeergave pw)
            {
                MessageBox.Show("Selecteer een partner.", "Geen selectie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (pw.IsOntkoppelt)
            {
                _partnerRijen.RemoveAll(r => (r.Partner == pw.Partner));
                pw.IsToegevoegd = false;
            
            pw.IsToegevoegd = false;

            RefreshLijst();
        }
    else
    {
        MessageBox.Show("Deze partner is nog niet toegevoegd aan het project.", "Niet gekoppeld",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
    }


        private void PartnerAanmaken_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNieuweNaam.Text))
            {
                MessageBox.Show("Vul een naam in.", "Validatie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var type = Enum.Parse<PartnerType>(
                    ((ComboBoxItem)CmbNieuwType.SelectedItem).Content.ToString()!);
                var partner = new Partner(0, TxtNieuweNaam.Text.Trim(), type);
                _service.PartnerAanmaken(partner);
                TxtNieuweNaam.Clear();
                LaadPartners();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? type = (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (GbStads != null) GbStads.Visibility = type == "StadsProject" ? Visibility.Visible : Visibility.Collapsed;
            if (GbGroen != null) GbGroen.Visibility = type == "GroenProject" ? Visibility.Visible : Visibility.Collapsed;
            if (GbInno != null) GbInno.Visibility = type == "WonenProject" ? Visibility.Visible : Visibility.Collapsed;

            // ChkBouwfirma: grijs
            if (ChkBouwfirma != null)
            {
                ChkBouwfirma.IsEnabled = (type == "StadsProject");
                if (type != "StadsProject") ChkBouwfirma.IsChecked = false;
            }
        }

        private void VulFormulierIn(Project p)
        {
            string typeName = p.Details.FirstOrDefault() switch
            {
                StadDetail => "StadsProject",
                GroenDetail => "GroenProject",
                WonenDetail => "WonenProject",
                _ => "StadsProject"
            };
            foreach (ComboBoxItem item in CmbType.Items)
                if (item.Content.ToString() == typeName) { CmbType.SelectedItem = item; break; }

            TxtTitel.Text = p.Titel;
            DpStart.SelectedDate = p.StartDatum;
            foreach (ComboBoxItem item in CmbStatus.Items)
                if (item.Content.ToString()!.Equals(p.Status.ToString(), StringComparison.OrdinalIgnoreCase))
                { CmbStatus.SelectedItem = item; break; }
            TxtBeschrijving.Text = p.Beschrijving;
            TxtGemeente.Text = p.Locatie.Gemeente;
            TxtPostCode.Text = p.Locatie.Postcode;
            TxtStraat.Text = p.Locatie.Straat;
            TxtHuisNummer.Text = p.Locatie.Huisnummer;
            TxtWijk.Text = p.Locatie.Wijk;

            foreach (var detail in p.Details)
            {
                switch (detail)
                {
                    case StadDetail sd:
                        foreach (ComboBoxItem i in CmbVergunning.Items)
                            if (i.Content.ToString() == sd.VergunningStatus.ToString()) { CmbVergunning.SelectedItem = i; break; }
                        ChkArchWaarde.IsChecked = sd.ArchitecturaleWaarde;
                        foreach (ComboBoxItem i in CmbToegang.Items)
                            if (i.Content.ToString() == sd.Toegankelijkheid.ToString()) { CmbToegang.SelectedItem = i; break; }
                        ChkToeristisch.IsChecked = sd.Bezienswaardigheid;
                        ChkInfowandeling.IsChecked = sd.InfoBordVoorzien;

                        // Grijs
                        _partnerRijen = p.Partners
                            .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving, "algemeen")).ToList();
                        if (sd.Bouwfirmas != null)
                            foreach (var b in sd.Bouwfirmas)
                                _partnerRijen.Add(new PartnerRij(b, b.Naam, "bouwfirma"));
                        break;

                    case GroenDetail gd:
                        TxtOppervlakte.Text = gd.Oppervlakte.ToString();
                        TxtBioScore.Text = gd.Biodiversiteit.ToString();
                        TxtWandelpaden.Text = gd.Wandelpaden.ToString();
                        TxtFaciliteiten.Text = gd.Faciliteiten;
                        ChkToerRoute.IsChecked = gd.ToeristischeRoute;
                        TxtBeoordeling.Text = gd.Beoordeling.ToString();
                        _partnerRijen = p.Partners
                            .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving)).ToList();
                        break;

                    case WonenDetail wd:
                        TxtEenheden.Text = wd.AantalEenheden.ToString();
                        TxtWoningTypes.Text = wd.Woningtypes;
                        ChkRondleiding.IsChecked = wd.Rondleidingen;
                        ChkShowwoning.IsChecked = wd.Showwoningen;
                        TxtInnoScore.Text = wd.ArchitecturaleScore.ToString();
                        ChkErfgoed.IsChecked = wd.ErfgoedSamenwerking;
                        _partnerRijen = p.Partners
                            .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving)).ToList();
                        break;
                }
            }

            var addedIds = _partnerRijen.Select(r => r.Partner.Id).ToHashSet();
            foreach (var pw in _weergave) pw.IsToegevoegd = addedIds.Contains(pw.Partner.Id);
            RefreshLijst();
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string type = ((ComboBoxItem)CmbType.SelectedItem).Content.ToString()!;
                string statusStr = ((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString()!;
                var status = Enum.Parse<ProjectStatus>(statusStr, ignoreCase: true);
                var locatie = new Locatie(null, TxtGemeente.Text, TxtPostCode.Text,
                                              TxtStraat.Text, TxtHuisNummer.Text, TxtWijk.Text);
                string beschr = TxtBeschrijving.Text ?? "";

                var project = _factory.CreateProject(TxtTitel.Text,
                    DpStart.SelectedDate ?? DateTime.Today, beschr, status, locatie);

                switch (type)
                {
                    case "StadsProject":
                        var vergunning = Enum.Parse<VergunningStatus>(((ComboBoxItem)CmbVergunning.SelectedItem).Content.ToString()!);
                        var toegankelijk = Enum.Parse<Toegankelijkheid>(((ComboBoxItem)CmbToegang.SelectedItem).Content.ToString()!);
                        var stadDetail = _factory.AddStadsDetail(0, vergunning,
                            ChkArchWaarde.IsChecked == true, toegankelijk,
                            ChkToeristisch.IsChecked == true, false,
                            ChkInfowandeling.IsChecked == true);
                        stadDetail.Bouwfirmas = _partnerRijen
                            .Where(r => r.Kategorie == "bouwfirma")
                            .Select(r => r.Partner).ToList();
                        project.Details.Add(stadDetail);
                        project.Partners = _partnerRijen
                            .Where(r => r.Kategorie == "algemeen")
                            .Select(r => new ProjectPartner(_bestand, r.Partner, r.Rol)).ToList();
                        break;

                    case "GroenProject":
                        project.Details.Add(_factory.AddGroenDetail(0,
                            decimal.Parse(TxtOppervlakte.Text), int.Parse(TxtBioScore.Text),
                            int.Parse(TxtWandelpaden.Text), TxtFaciliteiten.Text,
                            ChkToerRoute.IsChecked == true, int.Parse(TxtBeoordeling.Text)));
                        project.Partners = _partnerRijen
                            .Select(r => new ProjectPartner(_bestand, r.Partner, r.Rol)).ToList();
                        break;

                    case "WonenProject":
                        project.Details.Add(_factory.AddWonenDetail(0,
                            int.Parse(TxtEenheden.Text), TxtWoningTypes.Text,
                            ChkRondleiding.IsChecked == true, ChkShowwoning.IsChecked == true,
                            int.Parse(TxtInnoScore.Text), ChkErfgoed.IsChecked == true));
                        project.Partners = _partnerRijen
                            .Select(r => new ProjectPartner(_bestand, r.Partner, r.Rol)).ToList();
                        break;

                    default: throw new GentException("Onbekend projecttype.");
                }

                if (_bestand != null)
                {
                    project.Id = _bestand.Id;
                    project.Locatie.LocatieId = _bestand.Locatie.LocatieId;
                    _service.UpdateProject(project);
                }
                else
                {
                    _service.AddProject(project);
                }

                DialogResult = true;
                Close();
            }
            catch (GentException ex)
            { MessageBox.Show(ex.Message, "Validatiefout", MessageBoxButton.OK, MessageBoxImage.Warning); }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Annuleer_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}
