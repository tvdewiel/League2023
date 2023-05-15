using League.BL.DTO;
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

namespace League.UI
{
    /// <summary>
    /// Interaction logic for SelecteerSpelerWindow.xaml
    /// </summary>
    public partial class SelecteerSpelerWindow : Window
    {
        private List<SpelerInfo> spelers;
        public SpelerInfo? SelectedSpeler = null;
        public SelecteerSpelerWindow(List<SpelerInfo> spelers)
        {
            InitializeComponent();
            this.spelers= spelers;
            SpelersListBox.ItemsSource = spelers;
        }

        private void SpelersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OkButton.IsEnabled = true;
            SelectedSpeler=SpelersListBox.SelectedItem as SpelerInfo;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
