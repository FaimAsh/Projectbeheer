using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppProjectBeheeder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectService _service;
        public MainWindow() {
            InitializeComponent();
            LeesConfig();
            LaadProjecten();
        }

        private void LeesConfig() {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            string cs = config.GetConnectionString("SQLServerConnection");
            _service = new ProjectService- (new Project- (cs));
        }

        private void LaadProjecten() {
            try
            {
                DgProjecten.ItemsSource = _service.Search(filter ?? new ProjectFilter());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden projecten: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zoeken_Click(object sender, RoutedEventArgs e) {
            string? type = (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString() is "(alle)" ? null
                              : (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string? status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() is "(alle)" ? null
                              : (CmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var filter = new ProjectFilter
            {
                ProjectDetail = type,
                Status = status == null ? null : Enum.Parse<ProjectStatus>(status),
                Wijk = string.IsNullOrWhiteSpace(TxtWijk.Text) ? null : TxtWijk.Text.Trim(),
                PartnerNaam = string.IsNullOrWhiteSpace(TxtPartner.Text) ? null : TxtPartner.Text.Trim(),
                StartDatumVan = DpVan.SelectedDate,
                EndDatumVan = DpTot.SelectedDate
            };
            LaadProjecten(filter);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            CmbType.SelectedIndex = 0;
            CmbStatus.SelectedIndex = 0;
            TxtWijk.Clear();
            TxtPartner.Clear();
            DpVan.SelectedDate = null;
            DpTot.SelectedDate = null;
            LaadProjecten();
        }

        private void DgProjecten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var w = new NieuwProjectWindow(_service);
            if (w.ShowDialog() == true) LaadProjecten();
        }

        private void WijzigProject_Click(object sender, RoutedEventArgs e)
        {
            if (DgProjecten.SelectedItem is not Project geselecteerd)
            {
                MessageBox.Show("Selecteer een project.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var volledig = _service.GetById(geselecteerd.Id!.Value);
                var w = new NieuwProjectWindow(_service, volledig);
                if (w.ShowDialog() == true) LaadProjecten();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VerwijderProject_Click(object sender, RoutedEventArgs e)
        {
            if (DgProjecten.SelectedItem is not Project selected)
            {
                MessageBox.Show("Selecteer een project.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var res = MessageBox.Show($"Project '{selected.Titel}' verwijderen?", "Bevestigen",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                try { _service.DeleteProject(selected.Id!.Value); LaadProjecten(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void DetailProject_Click(object sender, RoutedEventArgs e)
        {
            if (DgProjecten.SelectedItem is not Project selected)
            {
                MessageBox.Show("Selecteer een project.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var volledig = _service.GetById(selected.Id!.Value);
                new DetailProjectWindow(volledig).ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);}
        }

        private void Partners_Click(object sender, RoutedEventArgs e)
            => new PartnerBeheerderWindow(_service).ShowDialog();

        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "projecten.csv" };
            if (dlg.ShowDialog() != true) return;
            try
            {
                var projecten = (List<Project>)DgProjecten.ItemsSource;
                _service.ExportCsv(projecten, dlg.FileName);
                MessageBox.Show("CSV geëxporteerd.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "PDF|*.pdf|Tekstbestand|*.txt", FileName = "projecten.pdf" };
            if (dlg.ShowDialog() != true) return;
            try
            {
                var projecten = (List<Project>)DgProjecten.ItemsSource;
                _service.ExportPdf(projecten, dlg.FileName);
                MessageBox.Show("Export klaar.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}