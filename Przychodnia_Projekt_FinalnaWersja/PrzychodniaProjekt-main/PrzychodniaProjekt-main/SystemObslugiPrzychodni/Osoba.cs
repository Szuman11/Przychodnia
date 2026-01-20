using System;
using System.Xml.Serialization;

namespace SystemObslugiPrzychodni;

/// <summary>
/// Abstrakcyjna klasa bazowa reprezentująca osobę w systemie kliniki.
/// Implementuje interfejsy porównywania oraz walidację danych osobowych.
/// </summary>
[XmlInclude(typeof(Pacjent))]
[XmlInclude(typeof(Lekarz))]
public abstract class Osoba : IEquatable<Osoba>, IComparable<Osoba>, IKlinikaObiekt
{
    private string imie = string.Empty;
    /// <summary>
    /// Imię osoby.
    /// </summary>
    public string Imie
    {
        get { return imie; }
        set { imie = value; }
    }

    private string nazwisko = string.Empty;
    /// <summary>
    /// Nazwisko osoby.
    /// </summary>
    public string Nazwisko
    {
        get { return nazwisko; }
        set { nazwisko = value; }
    }

    private DateTime dataUrodzenia;
    /// <summary>
    /// Data urodzenia osoby.
    /// </summary>
    public DateTime DataUrodzenia
    {
        get { return dataUrodzenia; }
        set
        {
            if (value > DateTime.Now)
            {
                throw new BlednaDataException($"Data urodzenia {value.ToShortDateString()} jest z przyszłości!");
            }
            dataUrodzenia = value;
        }
    }

    private string pesel = string.Empty;
    /// <summary>
    /// Numer PESEL (11 cyfr). Zawiera walidację sumy kontrolnej.
    /// </summary>
    public string Pesel
    {
        get { return pesel; }
        set
        {
            ValidatePesel(value);
            pesel = value;
        }
    }

    /// <summary>
    /// Zwraca wiek osoby wyliczony na podstawie daty urodzenia.
    /// </summary>
    public int Wiek
    {
        get
        {
            var dzis = DateTime.Today;
            var wiek = dzis.Year - DataUrodzenia.Year;
            if (DataUrodzenia.Date > dzis.AddYears(-wiek)) wiek--;
            return wiek;
        }
    }

    protected Osoba(string imie, string nazwisko, string pesel)
    {
        Imie = imie;
        Nazwisko = nazwisko;
        Pesel = pesel;
    }

    public Osoba() { }

    public bool Equals(Osoba? other)
    {
        if (other is null) return false;
        return this.Pesel == other.Pesel;
    }

    public int CompareTo(Osoba? other)
    {
        if (other is null) return 1;
        int porownanieNazwisk = string.Compare(this.Nazwisko, other.Nazwisko, StringComparison.CurrentCulture);
        if (porownanieNazwisk != 0) return porownanieNazwisk;
        return string.Compare(this.Imie, other.Imie, StringComparison.CurrentCulture);
    }

    public abstract string PobierzDane();

    public override string ToString() => $"{Imie} {Nazwisko} {Pesel}";

    public string PobierzTypObiektu() => this.GetType().Name;

    public string Typ => PobierzTypObiektu();
    public string Info => PobierzDane();

    protected void ValidatePesel(string pesel)
    {
        if (string.IsNullOrWhiteSpace(pesel)) throw new NiepoprawnyPeselException("PESEL nie może być pusty.");
        if (pesel.Length != 11) throw new NiepoprawnyPeselException($"PESEL musi mieć 11 znaków. Podano: {pesel.Length}");
        if (!long.TryParse(pesel, out _)) throw new NiepoprawnyPeselException("PESEL może składać się tylko z cyfr.");

        int[] wagi = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        int suma = 0;
        for (int i = 0; i < 10; i++) suma += (int.Parse(pesel[i].ToString()) * wagi[i]);

        int cyfraKontrolna = 10 - (suma % 10);
        if (cyfraKontrolna == 10) cyfraKontrolna = 0;

        if (cyfraKontrolna != int.Parse(pesel[10].ToString()))
            throw new NiepoprawnyPeselException("Nieprawidłowa suma kontrolna PESEL.");

        int rok = int.Parse(pesel.Substring(0, 2));
        int miesiac = int.Parse(pesel.Substring(2, 2));
        int dzien = int.Parse(pesel.Substring(4, 2));
        int pelnyRok = 1900 + rok;

        if (miesiac > 80) { pelnyRok = 1800 + rok; miesiac -= 80; }
        else if (miesiac > 60) { pelnyRok = 2200 + rok; miesiac -= 60; }
        else if (miesiac > 40) { pelnyRok = 2100 + rok; miesiac -= 40; }
        else if (miesiac > 20) { pelnyRok = 2000 + rok; miesiac -= 20; }

        try
        {
            DataUrodzenia = new DateTime(pelnyRok, miesiac, dzien);
        }
        catch
        {
            throw new NiepoprawnyPeselException("Data urodzenia w PESEL jest niepoprawna.");
        }
    }
}