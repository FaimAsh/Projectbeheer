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

namespace WpfAppProjectBeheeder
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NieuwProjectWindow : Window
    {
        private readonly ProjectService _service;
        private readonly Project? _bestaand;
        public NieuwProjectWindow(ProjectService service, Project? bestaand = null)
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
    }
}
