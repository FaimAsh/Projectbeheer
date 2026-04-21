using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ProjectBeheerderBL.Domein.Enums;

namespace WpfAppProjectBeheeder
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NieuwProjectWindow : Window
    {
        private readonly ProjectBeheerder _service;
        private readonly Project? _bestaand;
        public NieuwProjectWindow(ProjectBeheerder service, Project? bestaand = null)
        {
            InitializeComponent();
            _service = service;
            _bestaand = bestaand;

            if (_bestaand != null)
            {
                Title = "Project wijzigen";
                VulFormulierIn(_bestaand);
                CmbType.IsEnabled = false;
            }
        }
        private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? type = (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();
            GbStads.Visibility = type == "StadsProject" ? Visibility.Visible : Visibility.Collapsed;
            GbGroen.Visibility = type == "GroenProject" ? Visibility.Visible : Visibility.Collapsed;
            GbInno.Visibility = type == "WonenProject" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void VulFormulierIn(Project p)
        {
            // Set type combobox
            foreach (ComboBoxItem item in CmbType.Items)
                if (item.Content.ToString() == p.GetType().Name) { CmbType.SelectedItem = item; break; }

            TxtTitel.Text = p.Titel;
            DpStart.SelectedDate = p.StartDatum;
            foreach (ComboBoxItem item in CmbStatus.Items)
                if (item.Content.ToString() == p.Status.ToString()) { CmbStatus.SelectedItem = item; break; }
            TxtBeschrijving.Text = p.Beschrijving;

            TxtGemeente.Text = p.Locatie.Gemeente;
            TxtPostcode.Text = p.Locatie.Postcode;
            TxtStraat.Text = p.Locatie.Straat;
            TxtHuisnr.Text = p.Locatie.Huisnummer;
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
                        break;

                    case GroenDetail gd:
                        TxtOppervlakte.Text = gd.Oppervlakte.ToString();
                        TxtBioScore.Text = gd.Biodiversiteit.ToString();
                        TxtWandelpaden.Text = gd.Wandelpaden.ToString();
                        TxtFaciliteiten.Text = gd.Faciliteiten;
                        ChkToerRoute.IsChecked = gd.ToeristischeRoute;
                        TxtBeoordeling.Text = gd.Beoordeling.ToString();
                        break;

                    case WonenDetail wd:
                        TxtEenheden.Text = wd.AantalEenheden.ToString();
                        TxtWoningTypes.Text = wd.Woningtypes;
                        ChkRondleiding.IsChecked = wd.Rondleidingen;
                        ChkShowwoning.IsChecked = wd.Showwoningen;
                        TxtInnoScore.Text = wd.ArchitecturaleScore.ToString();
                        ChkErfgoed.IsChecked = wd.ErfgoedSamenwerking;
                        break;
                }
            }
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string type = ((ComboBoxItem)CmbType.SelectedItem).Content.ToString()!;
                var status = Enum.Parse<ProjectStatus>(((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString()!);
                var locatie = new Locatie(TxtGemeente.Text, TxtPostcode.Text,
                                            TxtStraat.Text, TxtHuisnr.Text, TxtWijk.Text);

                Project project = type switch
                {
                    "StadsProject" => _service.Factory.CreateStadsProject(
                        TxtTitel.Text, DpStart.SelectedDate ?? DateTime.Today, status, locatie,
                        Enum.Parse<VergunningStatus>(((ComboBoxItem)CmbVergunning.SelectedItem).Content.ToString()!),
                        ChkArchWaarde.IsChecked == true,
                        Enum.Parse<Toegankelijkheid>(((ComboBoxItem)CmbToegang.SelectedItem).Content.ToString()!),
                        ChkToeristisch.IsChecked == true,
                        ChkInfowandeling.IsChecked == true,
                        TxtBeschrijving.Text),

                    "GroenProject" => _service.Factory.CreateGroenProject(
                        TxtTitel.Text, DpStart.SelectedDate ?? DateTime.Today, status, locatie,
                        double.Parse(TxtOppervlakte.Text),
                        int.Parse(TxtBioScore.Text),
                        int.Parse(TxtWandelpaden.Text),
                        TxtFaciliteiten.Text,
                        ChkToerRoute.IsChecked == true,
                        int.Parse(TxtBeoordeling.Text),
                        TxtBeschrijving.Text),

                    "WonenProject" => _service.Factory.CreateWonenProject(
                        TxtTitel.Text, DpStart.SelectedDate ?? DateTime.Today, status, locatie,
                        int.Parse(TxtEenheden.Text),
                        TxtWoningTypes.Text,
                        ChkRondleiding.IsChecked == true,
                        ChkShowwoning.IsChecked == true,
                        int.Parse(TxtInnoScore.Text),
                        ChkErfgoed.IsChecked == true,
                        TxtBeschrijving.Text),

                    _ => throw new GentException("Onbekend type.")
                };

                if (_bestaand != null)
                {
                    project.Id = _bestaand.Id;
                    project.Locatie.LocatieID = _bestaand.Locatie.LocatieID;
                    project.LaadPartners(_bestaand.ProjectPartners);
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
            {
                MessageBox.Show(ex.Message, "Validatiefout", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annuleer_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
    }
}
}
