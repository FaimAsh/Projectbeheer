using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Interfaces;
using ProjectBeheerderDL_SQL;
using ProjectBeheerderUtil;
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
using static ProjectBeheerderBL.Domein.Enums;

namespace WpfAppProjectBeheeder {
    public partial class MainWindow : Window
    {

     

        string connectionstring;
        string databaseType;

       

        ProjectBeheerder _Beheerder;


        public MainWindow() {
            InitializeComponent();
            LeesConfig();
            _Beheerder = new ProjectBeheerder(RepositoryFactory.GeefRepository(databaseType, connectionstring));
            
            LaadProjecten();
        }

        private void LeesConfig() {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            connectionstring = config.GetConnectionString("SQLServerConnection");
            databaseType = config.GetSection("appsettings")["databaseType"];

          
          
        }

        private void LaadProjecten(ProjectFilter? filter = null)
        {
            try
            {
                DgProjecten.ItemsSource = _Beheerder.Search(filter ?? new ProjectFilter());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden projecten: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zoeken_Click(object sender, RoutedEventArgs e)
        {
            var filter = new ProjectFilter
            {
                Wijk = string.IsNullOrWhiteSpace(TxtWijk.Text) ? null : TxtWijk.Text.Trim(),
                PartnerNaam = string.IsNullOrWhiteSpace(TxtPartner.Text) ? null : TxtPartner.Text.Trim(),
                StartDatumVan = DpVan.SelectedDate,
                StartDatumTot = DpTot.SelectedDate
            };

            if (ChkBowfirm.IsChecked == true)
                filter.Statussen.Add(ProjectStatus.Planning);

            if (ChkBuwfira.IsChecked == true)
                filter.Statussen.Add(ProjectStatus.Uitvoering);

            if (ChkBouwfrma.IsChecked == true)
                filter.Statussen.Add(ProjectStatus.Afgerond);

            if (ChkBowfirma.IsChecked == true)
                filter.Details.Add("Stad");

            if (ChkBuwfirma.IsChecked == true)
                filter.Details.Add("Groen");

            if (ChkBouwfirma.IsChecked == true)
                filter.Details.Add("Wonen");

            LaadProjecten(filter);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ChkBowfirma.IsChecked = true;
            ChkBuwfirma.IsChecked = true;
            ChkBouwfirma.IsChecked = true;
            ChkBowfirm.IsChecked = true;
            ChkBuwfira.IsChecked = true;
            ChkBouwfrma.IsChecked = true;
            TxtWijk.Clear();
            TxtPartner.Clear();
            DpVan.SelectedDate = null;
            DpTot.SelectedDate = null;
            LaadProjecten();
        }

        private void DgProjecten_SelectionChanged(object sender, SelectionChangedEventArgs e){ }
        private void NieuwProject_Click(object sender, RoutedEventArgs e)
        {
            var w = new NieuwProjectWindow(_Beheerder);
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
                var volledig = _Beheerder.GeefProject(geselecteerd.Id);
                var w = new WijzigenWindow(_Beheerder, volledig);
                if (w.ShowDialog() == true) LaadProjecten();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VerwijderProject_Click(object sender, RoutedEventArgs e)
        {
            var geselecteerdeProjecten = DgProjecten.SelectedItems.Cast<Project>().ToList();
            if (geselecteerdeProjecten.Count == 0)
            {
                MessageBox.Show("Selecteer een of meer projecten.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string message = geselecteerdeProjecten.Count == 1
                ? $"Project '{geselecteerdeProjecten[0].Titel}' verwijderen?"
                : $"{geselecteerdeProjecten.Count} projecten verwijderen?";

            var res = MessageBox.Show(message, "Bevestigen",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                try {
                    foreach (var project in geselecteerdeProjecten)
                    { _Beheerder.VerwijderProject(project); }
                    LaadProjecten(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e) {
            try {
                var geselecteerd = DgProjecten.SelectedItems.Cast<Project>().ToList();
                if (geselecteerd.Count == 0) {
                    MessageBox.Show("Selecteer eerst een of meer projecten.");
                    return;
                }

                foreach (var project in geselecteerd) {
                    var alles = _Beheerder.GeefProject(project.Id);
                    new DetailProjectWindow(alles).ShowDialog();
                }
            }

            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void Partners_Click(object sender, RoutedEventArgs e)
            => new PartnerBeheerderWindow(_Beheerder).ShowDialog();

        
        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var projecten = DgProjecten.SelectedItems.Cast<Project>().ToList();

            if (projecten.Count == 0)
            {
                MessageBox.Show(
                    "Selecteer minstens 1 project om te exporteren.",
                    "Geen selectie",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var dlg = new SaveFileDialog
            {
                Filter = "CSV|*.csv",
                FileName = "export.csv"
            };

            if (dlg.ShowDialog() != true) return;

            try
            {
                List<Project> projectenvolledig = new List<Project>();
                foreach (Project p in projecten)
                {
                    projectenvolledig.Add(_Beheerder.GeefProject(p.Id));
                }
                IFileWriter writer = new FileWriterCSV();
                writer.Write(dlg.FileName, projectenvolledig);

                MessageBox.Show("CSV geëxporteerd.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            var projecten = DgProjecten.SelectedItems.Cast<Project>().ToList();

            if (projecten.Count == 0)
            {
                MessageBox.Show(
                    "Selecteer minstens 1 project om te exporteren.",
                    "Geen selectie",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var dlg = new SaveFileDialog
            {
                Filter = "PDF|*.pdf",
                FileName = "export.pdf"
            };

            if (dlg.ShowDialog() != true) return;

            try
            {
                List<Project> projectenvolledig = new List<Project>();
                foreach(Project p in projecten)
                {
                   projectenvolledig.Add(_Beheerder.GeefProject(p.Id));
                }
                IFileWriter writer = new FileWriterPDF();
                writer.Write(dlg.FileName, projectenvolledig);

                MessageBox.Show("PDF geëxporteerd.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}