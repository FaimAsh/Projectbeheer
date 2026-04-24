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

        public PartnerRij(Partner partner, string rol, string categorie = "algemeen")
        {
            Partner   = partner;
            Rol       = rol;
            Categorie = categorie;
        }

        public override string ToString() => Categorie == "bouwfirma"
            ? $"[bouwfirma] {Partner.Naam}  — {Rol}"
            : $"{Partner.Naam}  — {Rol}";
    }

    public partial class WijzigenWindow : Window
    {
        private readonly ProjectBeheerder _service;
        private readonly Project          _project;

        private bool IsStads => ChkStad?.IsChecked == true;
        private bool IsGroen => ChkGroen?.IsChecked == true;
        private bool IsWonen => ChkWonen?.IsChecked == true;
        private List<Partner>    _allePartners  = new();
        private List<PartnerRij> _partnerRijen = new();
        private List<ProjectPartner> GekoppeldePartners = new();

        public WijzigenWindow(ProjectBeheerder service, Project project)
        {
            InitializeComponent();
            _service = service;
            _project = project;

            ChkStad.IsChecked = project.Details.Any(d => d is StadDetail);
            ChkGroen.IsChecked = project.Details.Any(d => d is GroenDetail);
            ChkWonen.IsChecked = project.Details.Any(d => d is WonenDetail);

            UpdateVisibility();

            if (IsStads)
            {
                LblCategorie.Visibility      = Visibility.Visible;
                CmbWijzigCategorie.Visibility = Visibility.Visible;
                GridNieuweCategorie.Visibility = Visibility.Visible;
            }

            VulFormulierIn();
            LaadBeschikbarePartners();
        }

        private void ProjectCheckbox_Click(object sender, RoutedEventArgs e) => UpdateVisibility();

        private void UpdateVisibility()
        {
            if (GbStads != null) GbStads.Visibility = IsStads ? Visibility.Visible : Visibility.Collapsed;
            if (GbGroen != null) GbGroen.Visibility = IsGroen ? Visibility.Visible : Visibility.Collapsed;
            if (GbInno != null) GbInno.Visibility = IsWonen ? Visibility.Visible : Visibility.Collapsed;

            bool nowStads = IsStads;
            if (LblCategorie != null) LblCategorie.Visibility = nowStads ? Visibility.Visible : Visibility.Collapsed;
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

            //var detail = _project.Details.FirstOrDefault();
            foreach (var d in _project.Details)
            {
                switch (d)
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
                        ChkBeziens.IsChecked = sd.Bezienswaardigheid;
                        ChkInfobord.IsChecked = sd.InfoBordVoorzien;

                        _partnerRijen = _project.Partners
                            .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving, "algemeen"))
                            .ToList();
                        if (sd.Bouwfirmas != null)
                            foreach (var b in sd.Bouwfirmas)
                                _partnerRijen.Add(new PartnerRij(b, "bouwfirma", "bouwfirma"));
                        break;

                    case GroenDetail gd:
                        GbGroen.Visibility = Visibility.Visible;
                        TxtOppervlakte.Text = gd.Oppervlakte.ToString();
                        TxtBioScore.Text = gd.Biodiversiteit.ToString();
                        TxtWandelpaden.Text = gd.Wandelpaden.ToString();
                        TxtFaciliteiten.Text = gd.Faciliteiten;
                        ChkToerRoute.IsChecked = gd.ToeristischeRoute;
                        TxtBeoordeling.Text = gd.Beoordeling.ToString();

                        _partnerRijen = _project.Partners
                            .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving))
                            .ToList();
                        break;

                    case WonenDetail wd:
                        GbInno.Visibility = Visibility.Visible;
                        TxtEenheden.Text = wd.AantalEenheden.ToString();
                        TxtWoningTypes.Text = wd.Woningtypes;
                        ChkRondleiding.IsChecked = wd.Rondleidingen;
                        ChkShowwoning.IsChecked = wd.Showwoningen;
                        TxtInnoScore.Text = wd.ArchitecturaleScore.ToString();
                        ChkErfgoed.IsChecked = wd.ErfgoedSamenwerking;

                        _partnerRijen = _project.Partners
                            .Select(pp => new PartnerRij(pp.Partner, pp.RolBeschrijving))
                            .ToList();
                        break;
                }
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
            LstGekoppeld.ItemsSource = null;
            LstGekoppeld.ItemsSource = _partnerRijen.Select(r => r.ToString()).ToList();
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
                kat = ((ComboBoxItem)CmbNieuweCategorie.SelectedItem).Content.ToString()!;

            _partnerRijen.Add(new PartnerRij(gekozen, TxtNieuweRol.Text.Trim(), kat));
            TxtNieuweRol.Clear();
            RefreshGekoppeld();
            RefreshBeschikbaar();
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _project.Titel = TxtTitel.Text;
                _project.StartDatum = DpStart.SelectedDate ?? _project.StartDatum;
                _project.Beschrijving = TxtBeschrijving.Text ?? "";
                _project.Status = Enum.Parse<ProjectStatus>(
                    ((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString()!, ignoreCase: true);

                _project.Locatie.Gemeente = TxtGemeente.Text;
                _project.Locatie.Postcode = TxtPostCode.Text;
                _project.Locatie.Straat = TxtStraat.Text;
                _project.Locatie.Huisnummer = TxtHuisNummer.Text;
                _project.Locatie.Wijk = TxtWijk.Text;

                int? bestaandStadId = _project.Details.OfType<StadDetail>().FirstOrDefault()?.Id;
                int? bestaandGroenId = _project.Details.OfType<GroenDetail>().FirstOrDefault()?.Id;
                int bestaandWonenId = _project.Details.OfType<WonenDetail>().FirstOrDefault()?.Id ?? 0;
                //_project.Details.Clear();
                if (IsStads)
                {
                    var sd = new StadDetail(
                        bestaandStadId,
                        Enum.Parse<VergunningStatus>(((ComboBoxItem)CmbVergunning.SelectedItem).Content.ToString()!),
                        ChkArchWaarde.IsChecked == true,
                        Enum.Parse<Toegankelijkheid>(((ComboBoxItem)CmbToegang.SelectedItem).Content.ToString()!),
                        ChkBeziens.IsChecked == true,
                        ChkInfobord.IsChecked == true);
                    sd.Bouwfirmas = _partnerRijen
                        .Where(r => r.Categorie == "bouwfirma").Select(r => r.Partner).ToList();
                    _project.Partners = _partnerRijen
                        .Where(r => r.Categorie == "algemeen")
                        .Select(r => new ProjectPartner(_project, r.Partner, r.Rol)).ToList();
                    _project.Details.Add(sd);
                }

                if (IsGroen)
                {
                    _project.Details.Add(new GroenDetail(
                        bestaandGroenId,
                        decimal.Parse(TxtOppervlakte.Text), int.Parse(TxtBioScore.Text),
                        int.Parse(TxtWandelpaden.Text), TxtFaciliteiten.Text,
                        ChkToerRoute.IsChecked == true, int.Parse(TxtBeoordeling.Text)));
                    _project.Partners = _partnerRijen
                        .Select(r => new ProjectPartner(_project, r.Partner, r.Rol)).ToList();
                }

                if (IsWonen)
                {
                    _project.Details.Add(new WonenDetail(
                        bestaandWonenId,
                        int.Parse(TxtEenheden.Text), TxtWoningTypes.Text,
                        ChkRondleiding.IsChecked == true, ChkShowwoning.IsChecked == true,
                        int.Parse(TxtInnoScore.Text), ChkErfgoed.IsChecked == true));
                    _project.Partners = _partnerRijen
                        .Select(r => new ProjectPartner(_project, r.Partner, r.Rol)).ToList();
                }
                foreach (var pp in _project.Partners) 
                {
                    foreach(var partners in _service.GeefGeKoppeldePartners(_project.Id))
                    if(pp == partners) _service.VerwijderKoppeling(pp);
                }
                _service.UpdateProject(_project);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Annuleer_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}
