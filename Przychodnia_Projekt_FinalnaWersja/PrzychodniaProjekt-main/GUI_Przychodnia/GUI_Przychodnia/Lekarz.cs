using System;
using System.Text.RegularExpressions;

namespace SystemObslugiPrzychodni;

/// <summary>
/// Klasa reprezentująca lekarza. Posiada specjalizację oraz numer PWZ.
/// </summary>
public class Lekarz : Osoba
{
    private EnumSpecjalizacja specjalizacja;
    public EnumSpecjalizacja Specjalizacja
    {
        get { return specjalizacja; }
        set { specjalizacja = value; }
    }
    public string Typ { get; set; } = "Lekarz"; //Do Binding typ

    private string numerPWZ = string.Empty;
    public string NumerPWZ
    {
        get { return numerPWZ; }
        set
        {
            if (!Regex.IsMatch(value, @"^\d{7}$"))
            {
                throw new ArgumentException("Numer PWZ musi składać się z dokładnie 7 cyfr.");
            }
            numerPWZ = value;
        }
    }

    public Lekarz(string imie, string nazwisko, string pesel, EnumSpecjalizacja specjalizacja, string numerPwz)
        : base(imie, nazwisko, pesel)
    {
        Specjalizacja = specjalizacja;
        NumerPWZ = numerPwz;
    }

    public Lekarz() : base()
    {
        numerPWZ = string.Empty;
    }

    public bool CzyMozeLeczycSerce()
    {
        return Specjalizacja == EnumSpecjalizacja.Kardiolog || Specjalizacja == EnumSpecjalizacja.Internista;
    }

    public override string PobierzDane()
    {
        return $"Lekarz: {Imie} {Nazwisko}, Pesel: {Pesel}, Specjalizacja: {Specjalizacja}, PWZ: {NumerPWZ}";
    }
}