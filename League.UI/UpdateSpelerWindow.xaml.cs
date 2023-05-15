using League.BL.DTO;
using League.BL.Managers;
using League.DL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace League.UI
{
    /// <summary>
    /// Interaction logic for UpdateSpelerWindow.xaml
    /// </summary>
    public partial class UpdateSpelerWindow : Window
    {
        private SpelerManager spelerManager;
        public UpdateSpelerWindow()
        {
            InitializeComponent();
            spelerManager = new SpelerManager(new SpelerRepositoryADO(ConfigurationManager.ConnectionStrings["LeagueDBconnection"].ToString()));
        }

        private void ZoekSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? spelerId = null;
                string naam = null;
                if (!string.IsNullOrWhiteSpace(ZoekNaamTextBox.Text)) naam = ZoekNaamTextBox.Text;
                if (!string.IsNullOrWhiteSpace(ZoekSpelerIDTextBox.Text))
                {
                    spelerId = int.Parse(ZoekSpelerIDTextBox.Text);
                }
                List<SpelerInfo> spelers = (List<SpelerInfo>)spelerManager.SelecteerSpelers(spelerId, naam);
                if (spelers.Count == 0)
                {
                    NaamTextBox.Text = "";
                    SpelerIDTextBox.Text = "";
                    GewichtTextBox.Text = "";
                    LengteTextBox.Text = "";
                    RugnummerTextBox.Text = "";
                    TeamTextBox.Text = "";
                }
                if (spelers.Count == 1)
                {
                    NaamTextBox.Text = spelers[0].Naam;
                    SpelerIDTextBox.Text = spelers[0].Id.ToString();
                    GewichtTextBox.Text = spelers[0].Gewicht.ToString();
                    LengteTextBox.Text = spelers[0].Lengte.ToString();
                    RugnummerTextBox.Text = spelers[0].Rugnummer.ToString();
                    TeamTextBox.Text = spelers[0].Team;
                }
                if (spelers.Count > 1)
                {
                    SelecteerSpelerWindow w=new SelecteerSpelerWindow(spelers);
                    if (w.ShowDialog()==true)
                    {
                        NaamTextBox.Text = w.SelectedSpeler.Naam;
                        SpelerIDTextBox.Text = w.SelectedSpeler.Id.ToString();
                        GewichtTextBox.Text = w.SelectedSpeler.Gewicht.ToString();
                        LengteTextBox.Text = w.SelectedSpeler.Lengte.ToString();
                        RugnummerTextBox.Text = w.SelectedSpeler.Rugnummer.ToString();
                        TeamTextBox.Text = w.SelectedSpeler.Team;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int spelerid = int.Parse(SpelerIDTextBox.Text);
                int? gewicht = null;
                if (!string.IsNullOrWhiteSpace(GewichtTextBox.Text)) { gewicht = int.Parse(GewichtTextBox.Text); }
                int? lengte = null;
                if (!string.IsNullOrWhiteSpace(LengteTextBox.Text)) { lengte = int.Parse(LengteTextBox.Text); }
                int? rugnummer = null;
                if (!string.IsNullOrWhiteSpace(RugnummerTextBox.Text)) { rugnummer = int.Parse(RugnummerTextBox.Text); }
                SpelerInfo spelerInfo = new SpelerInfo(spelerid, NaamTextBox.Text, rugnummer,gewicht, lengte, TeamTextBox.Text);
                spelerManager.UpdateSpeler(spelerInfo);
                MessageBox.Show($"speler : {spelerInfo}", "speler is up to date");
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
