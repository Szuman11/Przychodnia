using System;
using System.Collections.Generic;

namespace SystemObslugiPrzychodni;

/// <summary>
/// Klasa reprezentuj¹ca pacjenta przychodni.
/// </summary>
public class Pacjent : Osoba, ICloneable
{
    private List<string> historiaChorob = new List<string>();

    /// <summary>
    /// Lista przebytych chorób pacjenta.
    /// </summary>
    public List<string> HistoriaChorob
    {
        get { return historiaChorob; }
        set { historiaChorob = value; }
    }

    public Pacjent(string imie, string nazwisko, string pesel) : base(imie, nazwisko, pesel)
    {
        historiaChorob = new List<string>();
    }

    public Pacjent() : base()
    {
        historiaChorob = new List<string>();
    }

    public override string PobierzDane()
    {
        string choroby = string.Join(", ", HistoriaChorob);
        if (string.IsNullOrEmpty(choroby)) choroby = "brak";
        return $"Pacjent: {Imie} {Nazwisko}, Pesel: {Pesel}, Historia Chorob: {choroby}";
    }

    public void DodajChorobe(string nazwa)
    {
        HistoriaChorob.Add(nazwa);
    }

    public bool CzyChorowalNa(string nazwaChoroby)
    {
        foreach (var choroba in HistoriaChorob)
        {
            if (choroba.ToLower().Contains(nazwaChoroby.ToLower())) return true;
        }
        return false;
    }

    public object Clone()
    {
        Pacjent kopia = (Pacjent)this.MemberwiseClone();
        kopia.HistoriaChorob = new List<string>(this.HistoriaChorob);
        return kopia;
    }
}