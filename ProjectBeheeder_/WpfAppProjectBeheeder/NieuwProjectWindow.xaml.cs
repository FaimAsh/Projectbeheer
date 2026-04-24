using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Exeptions;
using ProjectBeheerderUtil;
using System.ComponentModel.DataAnnotations;
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
            UpdateVisibility();

            if (_bestand != null)
            {
                MessageBox.Show("Dit project bestaat al.", "Bestaande Project", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
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

        private bool IsStads => ChkStad.IsChecked == true;
        private bool IsGroen => ChkGroen.IsChecked == true;
        private bool IsWonen => ChkWonen.IsChecked == true;

        private void ProjectCheckbox_Click(object sender, RoutedEventArgs e) => UpdateVisibility();

        private void UpdateVisibility()
        {
            if (GbStads != null) GbStads.Visibility = IsStads ? Visibility.Visible : Visibility.Collapsed;
            if (GbGroen != null) GbGroen.Visibility = IsGroen ? Visibility.Visible : Visibility.Collapsed;
            if (GbInno != null) GbInno.Visibility = IsWonen ? Visibility.Visible : Visibility.Collapsed;

            if (ChkBouwfirma != null)
            {
                ChkBouwfirma.IsEnabled = IsStads;
                if (!IsStads) ChkBouwfirma.IsChecked = false;
            }
        }

        private void TxtPartnerZoek_TextChanged(object sender, TextChangedEventArgs e)
            => RefreshLijst();

        private void RefreshLijst()
        {
            string zoek = TxtPartnerZoek?.Text.Trim().ToLower() ?? "";
            var gefilterd = string.IsNullOrEmpty(zoek)
                ? _weergave
                : _weergave.Where(w => w.Partner.Naam.Contains(zoek)).ToList();

            LstPartners.ItemsSource = null;
            LstPartners.ItemsSource = gefilterd;
        }
      

        private void VoegPartnerToe_Click(object sender, RoutedEventArgs e)
        {
            if (LstPartners.SelectedItem is not PartnerWeergave pw)
            { MessageBox.Show("Selecteer een partner.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning); 
                return; }
            if (pw.IsToegevoegd)
            { MessageBox.Show("Partner is al toegevoegd (grijs).", "Al toegevoegd", MessageBoxButton.OK, MessageBoxImage.Information); 
                return; }
            if (string.IsNullOrEmpty(TxtRol.Text))
            { MessageBox.Show("Rol moet ingevuld worden.", "Geen Rol Meegegeven", MessageBoxButton.OK, MessageBoxImage.Information); 
                return; }
            string kat = (IsStads && ChkBouwfirma.IsChecked == true) ? "bouwfirma" : "algemeen";
            _partnerRijen.Add(new PartnerRij(pw.Partner, TxtRol.Text.Trim(), kat));
            pw.IsToegevoegd = true; // grijs
            TxtRol.Clear();
            ChkBouwfirma.IsChecked = false;
            RefreshLijst();
        }

        private void LstPartners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstPartners.SelectedItem is not PartnerWeergave Geselecteerd)
            { ClearPartnerInputs(); return; }
            var rij = _partnerRijen.FirstOrDefault(r => r.Partner.Id == Geselecteerd.Partner.Id);

            if (rij != null)
            {
                TxtRol.Text = rij.Rol;
                if (IsStads) { ChkBouwfirma.IsChecked = (rij.Categorie == "bouwfirma"); }
            }
            else
            { ClearPartnerInputs(); }
        }
        private void ClearPartnerInputs()
        {
            TxtRol.Clear();
            if (ChkBouwfirma != null) ChkBouwfirma.IsChecked = false;
        }

        private void VerwijderKoppeling_Click(object sender, RoutedEventArgs e) 
        {
            if (LstPartners.SelectedItem is not PartnerWeergave pw)
            { MessageBox.Show("Selecteer een partner.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning); 
                return; }
            if (pw.IsToegevoegd)
            {
                _partnerRijen.RemoveAll(r => (r.Partner == pw.Partner));
                pw.IsToegevoegd = false;
                RefreshLijst();
            }
            else { MessageBox.Show("Deze partner is nog niet toegevoegd aan het project.", "Niet gekoppeld", MessageBoxButton.OK, MessageBoxImage.Information); }
        }


        private void PartnerAanmaken_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNieuweNaam.Text))
            { MessageBox.Show("Vul een naam in.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; }
            try
            {
                var type = Enum.Parse<PartnerType>(
                    ((ComboBoxItem)CmbNieuwType.SelectedItem).Content.ToString()!);
                var partner = new Partner(0, TxtNieuweNaam.Text.Trim(), type);
                _service.PartnerAanmaken(partner);
                TxtNieuweNaam.Clear();
                LaadPartners();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string statusStr = ((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString()!;
                var status = Enum.Parse<ProjectStatus>(statusStr, ignoreCase: true);
                var locatie = new Locatie(null, TxtGemeente.Text, TxtPostCode.Text,
                                              TxtStraat.Text, TxtHuisNummer.Text, TxtWijk.Text);
                string beschr = TxtBeschrijving.Text ?? "";

                var project = _factory.CreateProject(TxtTitel.Text,
                    DpStart.SelectedDate ?? DateTime.Today, beschr, status, locatie);

                if (IsStads)
                {
                    var vergunning = Enum.Parse<VergunningStatus>(((ComboBoxItem)CmbVergunning.SelectedItem).Content.ToString()!);
                    var toegankelijk = Enum.Parse<Toegankelijkheid>(((ComboBoxItem)CmbToegang.SelectedItem).Content.ToString()!);
                    var stadDetail = _factory.AddStadsDetail(0, vergunning,
                        ChkArchWaarde.IsChecked == true, toegankelijk,
                        ChkToeristisch.IsChecked == true, false,
                        ChkInfowandeling.IsChecked == true);
                    stadDetail.Bouwfirmas = _partnerRijen
                        .Where(r => r.Categorie == "bouwfirma")
                        .Select(r => r.Partner).ToList();
                    project.Details.Add(stadDetail);
                }

                if (IsGroen)
                {
                    project.Details.Add(_factory.AddGroenDetail(0,
                            decimal.Parse(TxtOppervlakte.Text), int.Parse(TxtBioScore.Text),
                            int.Parse(TxtWandelpaden.Text), TxtFaciliteiten.Text,
                            ChkToerRoute.IsChecked == true, int.Parse(TxtBeoordeling.Text)));
                }

                if (IsWonen)
                {
                    project.Details.Add(_factory.AddWonenDetail(0,
                        int.Parse(TxtEenheden.Text), TxtWoningTypes.Text,
                        ChkRondleiding.IsChecked == true, ChkShowwoning.IsChecked == true,
                        int.Parse(TxtInnoScore.Text), ChkErfgoed.IsChecked == true));
                }
                project.Partners = _partnerRijen
                .Select(r => new ProjectPartner(project, r.Partner, r.Rol)).ToList();

                if (_bestand != null)
                { MessageBox.Show("Selecteer een partner.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning); 
                    return; }
                else
                { _service.AddProject(project); }

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
