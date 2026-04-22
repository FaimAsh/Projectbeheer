using ProjectBeheerderBL.Beheerder;
using ProjectBeheerderBL.Domein;
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
    public partial class PartnerBeheerderWindow : Window
    {
        private readonly ProjectBeheerder _service;
        private List<Partner> _partners = new();

        public PartnerBeheerderWindow(ProjectBeheerder service)
        {
            InitializeComponent();
            _service = service;
            Laad();
        }

        private void Laad()
        {
            try { _partners = _service.GetPartners(); }
            catch { _partners = new List<Partner>(); }
            Filter(TxtNaam?.Text ?? "");
        }

        private void Filter(string zoek)
        {
            zoek = zoek.Trim().ToLower();
            var lijst = string.IsNullOrEmpty(zoek)
                ? _partners
                : _partners.Where(p => p.Naam.ToLower().Contains(zoek)).ToList();

            LstPartners.ItemsSource = null;
            LstPartners.ItemsSource = lijst
                .Select(p => $"{p.Naam}  ({p.PartnerType})")
                .ToList();
        }

        private void TxtNaam_TextChanged(object sender, TextChangedEventArgs e)
            => Filter(TxtNaam.Text);

        private void LstPartners_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void Aanmaken_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNaam.Text))
            {
                MessageBox.Show("Vul een naam in.", "Validatie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var type = Enum.Parse<PartnerType>(
                    ((ComboBoxItem)CmbTypePartner.SelectedItem).Content.ToString()!);
                var partner = new Partner(0, TxtNaam.Text.Trim(), type);
                _service.AddPartner(partner);
                TxtNaam.Clear();
                Laad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Verwijder_Click(object sender, RoutedEventArgs e)
        {
            int idx = LstPartners.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Selecteer eerst een partner.", "Geen selectie",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string filterTekst = TxtNaam.Text.Trim().ToLower();
            var zichtbaar = string.IsNullOrEmpty(filterTekst)
                ? _partners
                : _partners.Where(p => p.Naam.ToLower().Contains(filterTekst)).ToList();

            if (idx >= zichtbaar.Count) return;
            var teVerwijderen = zichtbaar[idx];

            var bevestig = MessageBox.Show(
                $"Partner '{teVerwijderen.Naam}' verwijderen?", "Bevestigen",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (bevestig != MessageBoxResult.Yes) return;

            try
            {
                _service.DeletePartner(teVerwijderen);
                Laad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sluiten_Click(object sender, RoutedEventArgs e) => Close();
    }
}
