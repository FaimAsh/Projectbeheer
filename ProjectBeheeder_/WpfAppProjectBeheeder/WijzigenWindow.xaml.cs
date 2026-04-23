using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using System.Windows;
using System.Windows.Controls;
using static ProjectBeheerderBL.Domein.Enums;

namespace WpfAppProjectBeheeder
{
    // Holds partner + editable rol + optional kategorie (stads only)
    internal class PartnerRij
    {
        public Partner Partner      { get; set; }
        public string  Rol          { get; set; }
        public string  Categorie    { get; set; } = "algemeen";

        public PartnerRij(Partner partner, string rol, string kategorie = "algemeen")
        {
            Partner   = partner;
            Rol       = rol;
            Categorie = kategorie;
        }

        public override string ToString() => Categorie == "bouwfirma"
            ? $"[bouwfirma] {Partner.Naam}  — {Rol}"
            : $"{Partner.Naam}  — {Rol}";
    }

    public partial class WijzigenWindow : Window
    {
        private readonly ProjectBeheerder _service;
        private readonly Project          _project;
        private readonly bool             _isStads;

        private bool IsStads =>
            (CmbType?.SelectedItem as ComboBoxItem)?.Content?.ToString() == "StadsProject";

        private List<Partner>    _allePartners  = new();
        private List<PartnerRij> _partnerRijen = new();
        private List<ProjectPartner> GekoppeldePartners = new();

        public WijzigenWindow(ProjectBeheerder service, Project project)
        {
            InitializeComponent();
            _service = service;
            _project = project;

            _isStads = project.Details.Any(d => d is StadDetail);

            // Set type combobox to current project type
            CmbType.SelectionChanged -= CmbType_SelectionChanged;
            string initType = project.Details.FirstOrDefault() switch
            {
                StadDetail  => "StadsProject",
                GroenDetail => "GroenProject",
                WonenDetail => "WonenProject",
                _           => "StadsProject"
            };
            foreach (ComboBoxItem item in CmbType.Items)
                if (item.Content.ToString() == initType) { CmbType.SelectedItem = item; break; }
            CmbType.SelectionChanged += CmbType_SelectionChanged;

            GbStads.Visibility = project.Details.Any(d => d is StadDetail) ? Visibility.Visible : Visibility.Collapsed;
            GbGroen.Visibility = project.Details.Any(d => d is GroenDetail) ? Visibility.Visible : Visibility.Collapsed;
            GbInno.Visibility  = project.Details.Any(d => d is WonenDetail) ? Visibility.Visible : Visibility.Collapsed;

            if (IsStads)
            {
                LblKategorie.Visibility      = Visibility.Visible;
                CmbWijzigCategorie.Visibility = Visibility.Visible;
                GridNieuweCategorie.Visibility = Visibility.Visible;
            }

            VulFormulierIn();
            LaadBeschikbarePartners();
        }

        private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? type = (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (GbStads != null) GbStads.Visibility = type == "StadsProject" ? Visibility.Visible : Visibility.Collapsed;
            if (GbGroen != null) GbGroen.Visibility = type == "GroenProject"  ? Visibility.Visible : Visibility.Collapsed;
            if (GbInno  != null) GbInno.Visibility  = type == "WonenProject"  ? Visibility.Visible : Visibility.Collapsed;

            bool nowStads = type == "StadsProject";
            if (LblKategorie      != null) LblKategorie.Visibility       = nowStads ? Visibility.Visible : Visibility.Collapsed;
            if (CmbWijzigCategorie != null) CmbWijzigCategorie.Visibility = nowStads ? Visibility.Visible : Visibility.Collapsed;
            if (GridNieuweCategorie != null) GridNieuweCategorie.Visibility = nowStads ? Visibility.Visible : Visibility.Collapsed;

            if (!nowStads)
                foreach (var r in _partnerRijen) r.Categorie = "algemeen";
        }


        private void VulFormulierIn()
        {
            TxtTitel.Text        = _project.Titel;
            DpStart.SelectedDate = _project.StartDatum;
            TxtBeschrijving.Text = _project.Beschrijving;

            foreach (ComboBoxItem item in CmbStatus.Items)
                if (item.Content.ToString()!.Equals(_project.Status.ToString(),
                    StringComparison.OrdinalIgnoreCase))
                { CmbStatus.SelectedItem = item; break; }

            TxtGemeente.Text   = _project.Locatie.Gemeente;
            TxtPostCode.Text   = _project.Locatie.Postcode;
            TxtStraat.Text     = _project.Locatie.Straat;
            TxtHuisNummer.Text = _project.Locatie.Huisnummer;
            TxtWijk.Text       = _project.Locatie.Wijk;

            var detail = _project.Details.FirstOrDefault();
            switch (detail)
            {
                case StadDetail sd:
                    GbStads.Visibility = Visibility.Visible;
                    foreach (ComboBoxItem i in CmbVergunning.Items)
                        if (i.Content.ToString() == sd.VergunningStatus.ToString())
                        { CmbVergunning.SelectedItem = i; break; }
                    ChkArchWaarde.IsChecked = sd.ArchitecturaleWaarde;
                    foreach (ComboBoxItem i in CmbToegang.Items)
                        if (i.Content.ToString() == sd.Toegankelijkheid.ToString())
                        { CmbToegang.SelectedItem = i; break; }
                    ChkBeziens.IsChecked  = sd.Bezienswaardigheid;
                    ChkInfobord.IsChecked = sd.InfoBordVoorzien;

                    _partnerRijen = _project.Partners
                        .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving, "algemeen"))
                        .ToList();
                    if (sd.Bouwfirmas != null)
                        foreach (var b in sd.Bouwfirmas)
                            _partnerRijen.Add(new PartnerRij(b, "bouwfirma", "bouwfirma"));
                    break;

                case GroenDetail gd:
                    GbGroen.Visibility     = Visibility.Visible;
                    TxtOppervlakte.Text    = gd.Oppervlakte.ToString();
                    TxtBioScore.Text       = gd.Biodiversiteit.ToString();
                    TxtWandelpaden.Text    = gd.Wandelpaden.ToString();
                    TxtFaciliteiten.Text   = gd.Faciliteiten;
                    ChkToerRoute.IsChecked = gd.ToeristischeRoute;
                    TxtBeoordeling.Text    = gd.Beoordeling.ToString();

                    _partnerRijen = _project.Partners
                        .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving))
                        .ToList();
                    break;

                case WonenDetail wd:
                    GbInno.Visibility        = Visibility.Visible;
                    TxtEenheden.Text         = wd.AantalEenheden.ToString();
                    TxtWoningTypes.Text      = wd.Woningtypes;
                    ChkRondleiding.IsChecked = wd.Rondleidingen;
                    ChkShowwoning.IsChecked  = wd.Showwoningen;
                    TxtInnoScore.Text        = wd.ArchitecturaleScore.ToString();
                    ChkErfgoed.IsChecked     = wd.ErfgoedSamenwerking;

                    _partnerRijen = _project.Partners
                        .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving))
                        .ToList();
                    break;
            }

            RefreshGekoppeld();
        }

        private void LaadBeschikbarePartners()
        {
            try   { _allePartners = _service.GeefPartners(); }
            catch { _allePartners = new List<Partner>(); }
            RefreshBeschikbaar();
        }

        private void RefreshGekoppeld()
        {
            var linked = _partnerRijen.Select(r => r.Partner.Id).ToHashSet();
            var gekoppeld = _allePartners
                .Where(p => linked.Contains(p.Id))
                .ToList();
            LstGekoppeld.ItemsSource = null;
            LstGekoppeld.ItemsSource = gekoppeld;
            TxtWijzigRol.Clear();
        }

        private void LstGekoppeld_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = LstGekoppeld.SelectedIndex;
            if (idx < 0) { TxtWijzigRol.Clear(); return; }

            var rij = _partnerRijen[idx];
            TxtWijzigRol.Text = rij.Rol;

            if (IsStads)
            {
                foreach (ComboBoxItem item in CmbWijzigCategorie.Items)
                    if (item.Content.ToString() == rij.Categorie)
                    { CmbWijzigCategorie.SelectedItem = item; break; }
            }
        }

        private void WijzigGekoppeld_Click(object sender, RoutedEventArgs e)
        {
            int idx = LstGekoppeld.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Selecteer een gekoppelde partner.", "Geen selectie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtWijzigRol.Text))
            {
                MessageBox.Show("Rol mag niet leeg zijn.", "Validatie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _partnerRijen[idx].Rol = TxtWijzigRol.Text.Trim();
            if (IsStads)
                _partnerRijen[idx].Categorie =
                    ((ComboBoxItem)CmbWijzigCategorie.SelectedItem).Content.ToString()!;

            //int ProjectID = _project.Id;
            //int partnerId = _partnerRijen[idx].Partner.Id;
            //string nieuweRol = _partnerRijen[idx].Rol;
            //_service.UpdateRol(ProjectID, partnerId, nieuweRol);

            RefreshGekoppeld();
        }

        private void VerwijderGekoppeld_Click(object sender, RoutedEventArgs e)
        {
            int idx = LstGekoppeld.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Selecteer een gekoppelde partner.", "Geen selectie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _partnerRijen.RemoveAt(idx);
            RefreshGekoppeld();
            RefreshBeschikbaar();
        }

        private void RefreshBeschikbaar()
        {
            string zoek       = TxtPartnerZoek?.Text.Trim().ToLower() ?? "";
            var    linked     = _partnerRijen.Select(r => r.Partner.Id).ToHashSet();
            var    gefilterd  = _allePartners
                .Where(p => !linked.Contains(p.Id))
                .Where(p => string.IsNullOrEmpty(zoek) || p.Naam.ToLower().Contains(zoek))
                .ToList();

            LstBeschikbaar.ItemsSource = null;
            LstBeschikbaar.ItemsSource = gefilterd;
        }

        private void TxtPartnerZoek_TextChanged(object sender, TextChangedEventArgs e)
            => RefreshBeschikbaar();

        private void VoegPartnerToe_Click(object sender, RoutedEventArgs e)
        {
            if (LstBeschikbaar.SelectedItem is not Partner gekozen)
            {
                MessageBox.Show("Selecteer een beschikbare partner.", "Geen selectie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtNieuweRol.Text))
            {
                MessageBox.Show("Vul een rol in.", "Validatie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string kat = "algemeen";
            if (IsStads)
                kat = ((ComboBoxItem)CmbNieuweKategorie.SelectedItem).Content.ToString()!;

            _partnerRijen.Add(new PartnerRij(gekozen, TxtNieuweRol.Text.Trim(), kat));
            TxtNieuweRol.Clear();
            RefreshGekoppeld();
            RefreshBeschikbaar();
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string type = ((ComboBoxItem)CmbType.SelectedItem).Content.ToString()!;

                // Update base fields
                _project.Titel        = TxtTitel.Text;
                _project.StartDatum   = DpStart.SelectedDate ?? _project.StartDatum;
                _project.Beschrijving = TxtBeschrijving.Text ?? "";
                _project.Status       = Enum.Parse<ProjectStatus>(
                    ((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString()!, ignoreCase: true);

                // Update locatie
                _project.Locatie.Gemeente   = TxtGemeente.Text;
                _project.Locatie.Postcode   = TxtPostCode.Text;
                _project.Locatie.Straat     = TxtStraat.Text;
                _project.Locatie.Huisnummer = TxtHuisNummer.Text;
                _project.Locatie.Wijk       = TxtWijk.Text;

                // update detail
                ProjectDetail nieuweDetail;
                switch (type)
                {
                    case "StadsProject":
                        var sd = new StadDetail(
                            (_project.Details.FirstOrDefault() as StadDetail)?.Id,
                            Enum.Parse<VergunningStatus>(((ComboBoxItem)CmbVergunning.SelectedItem).Content.ToString()!),
                            ChkArchWaarde.IsChecked == true,
                            Enum.Parse<Toegankelijkheid>(((ComboBoxItem)CmbToegang.SelectedItem).Content.ToString()!),
                            ChkBeziens.IsChecked  == true,
                            ChkInfobord.IsChecked  == true);
                        sd.Bouwfirmas = _partnerRijen
                            .Where(r => r.Categorie == "bouwfirma").Select(r => r.Partner).ToList();
                        _project.Partners = _partnerRijen
                            .Where(r => r.Categorie == "algemeen")
                            .Select(r => new ProjectPartner(_project, r.Partner, r.Rol)).ToList();
                        nieuweDetail = sd;
                        break;

                    case "GroenProject":
                        nieuweDetail = new GroenDetail(
                            (_project.Details.FirstOrDefault() as GroenDetail)?.Id,
                            decimal.Parse(TxtOppervlakte.Text), int.Parse(TxtBioScore.Text),
                            int.Parse(TxtWandelpaden.Text),     TxtFaciliteiten.Text,
                            ChkToerRoute.IsChecked == true,     int.Parse(TxtBeoordeling.Text));
                        _project.Partners = _partnerRijen
                            .Select(r => new ProjectPartner(_project, r.Partner, r.Rol)).ToList();
                        break;

                    case "WonenProject":
                        nieuweDetail = new WonenDetail(
                            (_project.Details.FirstOrDefault() as WonenDetail)?.Id ?? 0,
                            int.Parse(TxtEenheden.Text),  TxtWoningTypes.Text,
                            ChkRondleiding.IsChecked == true, ChkShowwoning.IsChecked == true,
                            int.Parse(TxtInnoScore.Text),     ChkErfgoed.IsChecked    == true);
                        _project.Partners = _partnerRijen
                            .Select(r => new ProjectPartner(_project, r.Partner, r.Rol)).ToList();
                        break;

                    default: throw new Exception("Onbekend projecttype.");
                }

                _project.Details.Clear();
                _project.Details.Add(nieuweDetail);

                _service.UpdateProject(_project);
                foreach (var gkp in GekoppeldePartners)
                {

                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annuleer_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}
