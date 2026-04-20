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
    public partial class DetailProjectWindow : Window
    {
        public DetailProjectWindow(Project project)
        {
            InitializeComponent();
            Title = $"Detail — {project.Titel}";
            ToonDetails(project);
        }

        private void ToonDetails(Project p)
        {
            Rij("Type", p.GetType().Name);
            Rij("Titel", p.Titel);
            Rij("Startdatum", p.StartDatum.ToString("dd/MM/yyyy"));
            Rij("Status", p.Status.ToString());
            Rij("Beschrijving", p.Beschrijving ?? "/");
            Rij("Locatie",
                $"{p.Locatie.Straat} {p.Locatie.Huisnummer}, {p.Locatie.Postcode} {p.Locatie.Gemeente} ({p.Locatie.Wijk})");

            switch (p) 
            {
                case StadsProject sp:
                    Sep();

                    break;
            }
        }

        private void Rij(string label, string waarde)
        {
            var sp = new StackPanel { Orientation = Orientation.Horizontal, Margin = new(0, 2, 0, 2) };
            sp.Children.Add(new TextBlock { Text = label + ":", Width = 170, FontWeight = FontWeights.SemiBold });
            sp.Children.Add(new TextBlock { Text = waarde, TextWrapping = TextWrapping.Wrap, MaxWidth = 260 });
            PnlDetail.Children.Add(sp);
        }

        private void Sep() =>
            PnlDetail.Children.Add(new System.Windows.Shapes.Rectangle
                { Height = 1, Fill = System.Windows.Media.Brushes.LightGray, Margin = new(0, 6, 0, 6)});
    }
}
