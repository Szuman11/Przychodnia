using System.IO;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using SystemObslugiPrzychodni;

namespace GUI_Przychodnia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KlinikaManager manager = new KlinikaManager();
        private const string PLIK_BAZY = "baza_danych.xml";
        public MainWindow()
        {
            InitializeComponent();
            PrzygotujStart();
        }

        private void PrzygotujStart()
        {
            cmbSpecjalizacja.ItemsSource = Enum.GetValues(typeof(EnumSpecjalizacja));
            cmbSpecjalizacja.SelectedIndex = 0;

            cmbFiltrSpecjalizacja.ItemsSource = Enum.GetValues(typeof(EnumSpecjalizacja));
            cmbFiltrSpecjalizacja.SelectedIndex = 0;

            cmbTyp.SelectedIndex = 1;
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imie = txtImie.Text;
                string nazwisko = txtNazwisko.Text;
                string pesel = txtPesel.Text;

                ComboBoxItem wybranyTyp = (ComboBoxItem)cmbTyp.SelectedItem;
                string typTekst = wybranyTyp.Content.ToString();

                Osoba nowaOsoba = null;

                if (typTekst == "Lekarz")
                {
                    string pwz = txtPWZ.Text;
                    EnumSpecjalizacja spec = (EnumSpecjalizacja)cmbSpecjalizacja.SelectedItem;
                    nowaOsoba = new Lekarz(imie, nazwisko, pesel, spec, pwz);
                }
                else
                {
                    nowaOsoba = new Pacjent(imie, nazwisko, pesel);
                }
                manager.DodajOsobe(nowaOsoba);
                OdswiezWidok();
                WyczyscPola();
                lblStatus.Text = "Dodawanie zakończone";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message} podczas dodawania","Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSortujWiek_Click(object sender, RoutedEventArgs e)
        {
            gridOsoby.ItemsSource = manager.PobierzOsobyPosortowanePoWieku(true);
            lblStatus.Text = "Posortowano: Najstarsi.";
        }

        private void BtnSortujWiekRosnaco_Click(object sender, RoutedEventArgs e)
        {
            gridOsoby.ItemsSource = manager.PobierzOsobyPosortowanePoWieku(false);
            lblStatus.Text = "Posortowano najmłodsi";
        }
        private void BtnFiltruj_Click(object sender, RoutedEventArgs e)
        {
            if(cmbFiltrSpecjalizacja.SelectedItem is EnumSpecjalizacja spec)
            {
                var wyniki = manager.ZnajdzLekarzySpecjalistow(spec);
                gridOsoby.ItemsSource = wyniki;
                lblStatus.Text = $"Znaleziono {wyniki.Count} lekarzy";
            }
        }
        private void BtnZapiszXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var lista = manager.PobierzOsobyPosortowanePoWieku(true);
                XmlSerializer xs = new XmlSerializer(typeof(List<Osoba>));
                using (StreamWriter sw = new StreamWriter(PLIK_BAZY))
                {
                    xs.Serialize(sw, lista);
                }
                MessageBox.Show("Zapisano dane do XML");
                lblStatus.Text = "Zapisano bazę XML";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd zapisu:" + ex.Message);
            }
        }

        private void BtnWczytajXML_Click(Object sender, RoutedEventArgs e)
        {
            if(!File.Exists(PLIK_BAZY))
            {
                MessageBox.Show("Brak pliku bazy danych");
                return;
            }
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Osoba>));
                using (StreamReader sr = new StreamReader(PLIK_BAZY))
                {
                    List<Osoba> wczytani = (List<Osoba>)xs.Deserialize(sr);
                    manager = new KlinikaManager();
                    foreach (var o in wczytani) manager.DodajOsobe(o);
                }
                OdswiezWidok();
                MessageBox.Show("Wczytano dane");
                lblStatus.Text = "Wczytano baze XML";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Blad odczytu: "+ex.Message);
            }
        }

        private void cmbTyp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OpcjeLekarz == null) return;

            ComboBoxItem item = (ComboBoxItem)cmbTyp.SelectedItem;

            if(item.Content.ToString()=="Lekarz") OpcjeLekarz.Visibility = Visibility.Visible;
            else OpcjeLekarz.Visibility=Visibility.Collapsed;
        }

        private void OdswiezWidok()
        {
            gridOsoby.ItemsSource = null;
            gridOsoby.ItemsSource = manager.PobierzOsobyPosortowanePoWieku(true);
            

            var pacjenci = manager.PobierzWszystkichPacjentow();
            txtLiczbaPacjentow.Text = pacjenci.Count.ToString();

            double srednia = manager.ObliczSredniWiekPacjentow();
            txtSredniWiek.Text = $"{srednia:F1} lat";
            var lekarze = manager.PobierzWszystkichLekarzy();
            txtLiczbaLekarzy.Text = lekarze.Count.ToString();
        }

        private void WyczyscPola()
        {
            txtImie.Clear();
            txtNazwisko.Clear();
            txtPesel.Clear();
            txtPWZ.Clear();
        }

        private void HistoriaButton_Click(object sender, RoutedEventArgs e)
        {
            if (gridOsoby.SelectedItem == null)
            {
                MessageBox.Show("Nie wybrano osoby");
                return;
            }
            if(gridOsoby.SelectedItem is Pacjent p)
            {
                HistoriaChorobWindow Choroby = new HistoriaChorobWindow(p);
                Choroby.Owner = this;
                Choroby.ShowDialog();
                OdswiezWidok();
            }
            else
            {
                MessageBox.Show("Nie wybrano pacjenta");
            }
        }

        private void btnOdswierz_Click(object sender, RoutedEventArgs e)
        {
            OdswiezWidok();
        }
    }
}