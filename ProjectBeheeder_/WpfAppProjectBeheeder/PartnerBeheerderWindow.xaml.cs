using ProjectBeheerderBL.Beheerder;
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

namespace WpfAppProjectBeheeder
{
    /// <summary>
    /// Interaction logic for PartnerManagerWindow.xaml
    /// </summary>
    public partial class PartnerBeheerderWindow : Window
    {
        private readonly ProjectService _service;
        public PartnerBeheerderWindow(ProjectService service)
        {
            InitializeComponent();
            _service = service;
            Laad();
        }

        private void Laad() => DgPartners.ItemsSource = _service.GetAllPartners();

    }
}
