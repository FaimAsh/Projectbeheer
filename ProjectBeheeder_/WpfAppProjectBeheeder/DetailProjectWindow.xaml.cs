using ProjectBeheerderBL.Domein;
using ProjectBeheerderBL.DomeinDetails;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            if (p.Partners != null && p.Partners.Count > 0)
            {
                Sep();
                Rij("Partners", string.Join(", ",
                    p.Partners
                        .Where(pp => pp.Partner != null)
                        .Select(pp => $"{pp.Partner.Naam} ({pp.RolBeschrijving})")));
            }
            else
            {
                Rij("Partners", "/");
            }

            if (p.Details == null) return;

            foreach (var detail in p.Details)
            {
                switch (detail)
                {
                    case StadDetail sd:
                        Sep();
                        Rij("Vergunningstatus", sd.VergunningStatus.ToString());
                        Rij("Architecturale waarde", sd.ArchitecturaleWaarde ? "Ja" : "Nee");
                        Rij("Toegankelijkheid", sd.Toegankelijkheid.ToString());
                        Rij("Bezienswaardigheid", sd.Bezienswaardigheid ? "Ja" : "Nee");
                        Rij("Infowandeling", sd.InfoBordVoorzien ? "Ja" : "Nee");


                        if (sd.Bouwfirmas != null && sd.Bouwfirmas.Count > 0)
                        {
                            Rij("Bouwfirmas", string.Join(", ", sd.Bouwfirmas.Select(b => b.Naam)));
                        }
                        break;

                    case GroenDetail gd:
                        Sep();
                        Rij("Oppervlakte", $"{gd.Oppervlakte} m²");
                        Rij("Biodiversiteitscore", gd.Biodiversiteit + "/10");
                        Rij("Wandelpaden", gd.Wandelpaden.ToString());
                        Rij("Faciliteiten", string.IsNullOrWhiteSpace(gd.Faciliteiten) ? "/" : gd.Faciliteiten);
                        Rij("Toeristische route", gd.ToeristischeRoute ? "Ja" : "Nee");
                        Rij("Beoordeling", gd.Beoordeling + "/5");
                        break;

                    case WonenDetail wd:
                        Sep();
                        Rij("Aantal eenheden", wd.AantalEenheden.ToString());
                        Rij("Woning types", string.IsNullOrWhiteSpace(wd.Woningtypes) ? "/" : wd.Woningtypes);
                        Rij("Rondleidingen", wd.Rondleidingen ? "Ja" : "Nee");
                        Rij("Showwoning", wd.Showwoningen ? "Ja" : "Nee");
                        Rij("Innovatie score", wd.ArchitecturaleScore + "/10");
                        Rij("Erfgoed samenwerking", wd.ErfgoedSamenwerking ? "Ja" : "Nee");
                        break;
                }
            }
        }

        private void Rij(string label, string waarde)
        {
            var sp = new StackPanel { Orientation = Orientation.Horizontal, Margin = new(0, 2, 0, 2) };
            sp.Children.Add(new TextBlock { Text = label + ":", Width = 170, FontWeight = FontWeights.SemiBold });
            sp.Children.Add(new TextBlock { Text = waarde, TextWrapping = TextWrapping.Wrap, MaxWidth = 260 });
            PnlDetail.Children.Add(sp);
        }

        private void Sep()
        {
            PnlDetail.Children.Add(new System.Windows.Shapes.Rectangle
            {
                Height = 1,
                Fill = Brushes.LightGray,
                Margin = new(0, 6, 0, 6)
            });
        }
    }
}