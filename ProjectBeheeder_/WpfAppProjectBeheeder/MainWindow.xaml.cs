using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using ProjectBeheerderBL.Interfaces;
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

            _Beheerder = new ProjectBeheerder(RepositoryFactory.GeefRepository(databaseType, connectionstring));
            LeesConfig();
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

        private void LaadProjecten(ProjectFilter? filter = null) {
            try
            {
                DgProjecten.ItemsSource = _Beheerder.Search(new ProjectFilter());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden projecten: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zoeken_Click(object sender, RoutedEventArgs e) {

            var filter = new ProjectFilter
            { 
                Wijk = string.IsNullOrWhiteSpace(TxtWijk.Text) ? null : TxtWijk.Text.Trim(),
                PartnerNaam = string.IsNullOrWhiteSpace(TxtPartner.Text) ? null : TxtPartner.Text.Trim(),
                StartDatumVan = DpVan.SelectedDate,
                StartDatumTot = DpTot.SelectedDate
            };

            bool planningChecked = ChkBowfirm.IsChecked == true;
            bool uitvoeringChecked = ChkBuwfira.IsChecked == true;
            bool afgerondChecked = ChkBouwfrma.IsChecked == true;
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

        private void DgProjecten_SelectionChanged(object sender, RoutedEventArgs e){ }
        private void NieuwProject_Click(object sender, SelectionChangedEventArgs e)
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
                var volledig = _Beheerder.GetByID(geselecteerd.Id);
                var w = new NieuwProjectWindow(_Beheerder, volledig);
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
                try { _Beheerder.DeleteProject(selected); LaadProjecten(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            if (DgProjecten.SelectedItem is not Project selected)
            {
                MessageBox.Show("Selecteer een project.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var volledig = _Beheerder.GetByID(selected.Id);
                new DetailProjectWindow(volledig).ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);}
        }

        private void Partners_Click(object sender, RoutedEventArgs e)
            => new PartnerBeheerderWindow(_Beheerder).ShowDialog();        

        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "projecten.csv" };
            if (dlg.ShowDialog() != true) return;
            try
            {
                var projecten = (List<Project>)DgProjecten.ItemsSource;
 
                _Beheerder.ExportCsv(projecten, dlg.FileName);

                
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

                _Beheerder.ExportPdf(projecten, dlg.FileName);

               

                MessageBox.Show("Export klaar.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}