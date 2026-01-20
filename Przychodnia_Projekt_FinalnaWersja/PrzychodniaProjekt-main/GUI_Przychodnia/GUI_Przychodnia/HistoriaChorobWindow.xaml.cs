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
using SystemObslugiPrzychodni;

namespace GUI_Przychodnia
{
    /// <summary>
    /// Logika interakcji dla klasy HistoriaChorobWindow.xaml
    /// </summary>
    public partial class HistoriaChorobWindow : Window
    {
        Pacjent p = new Pacjent();
        public HistoriaChorobWindow(Pacjent pacjent)
        {
            InitializeComponent();
            p = pacjent;
            lblPacjentNazwa.Text = $"Pacjent: {p.Imie} {p.Nazwisko}";
            lblPacjentPesel.Text = $"{p.Pesel}";
            lstChoroby.ItemsSource = p.HistoriaChorob;
        }

        private void BtnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            string choroba = txtNowaChoroba.Text.Trim();
            if (string.IsNullOrEmpty(choroba))
            {
                MessageBox.Show("Wpisz nazwe choroby"); return;
            }
            p.HistoriaChorob.Add(choroba);
            OdwierzChorobe();
            txtNowaChoroba.Text = string.Empty;
        }
        public void OdwierzChorobe()
        {
            lstChoroby.ItemsSource = null;
            lstChoroby.ItemsSource = p.HistoriaChorob;
        }

        private void btnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (lstChoroby.SelectedItem != null)
            {
                p.UsunChoroba((string)lstChoroby.SelectedItem);
                OdwierzChorobe();
            }
            else { MessageBox.Show("Wybierz Chorobe"); }
        }

        private void btnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            
            if (lstChoroby.SelectedItem != null)
            {
                int index = lstChoroby.SelectedIndex;
                txtNowaChoroba.Text = (string)lstChoroby.SelectedItem;
                
            }

        }
    }
}
