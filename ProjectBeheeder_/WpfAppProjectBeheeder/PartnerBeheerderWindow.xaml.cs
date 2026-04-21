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
        private readonly ProjectService _service;
        public PartnerBeheerderWindow(ProjectService service)
        {
            InitializeComponent();
            _service = service;
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var type = Enum.Parse<PartnerType>(((ComboBoxItem)CmbTypePartner.SelectedItem).Content.ToString()!);
                var partner = new Partner(TxtNaam.Text, type);
                _service.AddPartner(partner);
                TxtNaam.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sluiten_Click(object sender, RoutedEventArgs e) => Close();
    }
}
