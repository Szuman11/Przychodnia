using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemObslugiPrzychodni;

/// <summary>
/// Główny kontroler logiki biznesowej. Zarządza listą osób.
/// </summary>
public class KlinikaManager
{
    private List<Osoba> listaOsob;

    public KlinikaManager()
    {
        listaOsob = new List<Osoba>();
    }

    /// <summary>
    /// Dodaje nową osobę do bazy danych.
    /// </summary>
    public void DodajOsobe(Osoba nowaOsoba)
    {
        if (listaOsob.Contains(nowaOsoba))
        {
            throw new InvalidOperationException("Taka osoba już istnieje w bazie (zgodność PESEL).");
        }
        listaOsob.Add(nowaOsoba);
    }

    public List<Pacjent> PobierzWszystkichPacjentow()
    {
        List<Pacjent> pacjenci = new List<Pacjent>();
        foreach (var osoba in listaOsob)
        {
            if (osoba is Pacjent p) pacjenci.Add(p);
        }
        return pacjenci;
    }
    public List<Lekarz> PobierzWszystkichLekarzy()
    {
        List<Lekarz> Lekarze = new List<Lekarz>();
        foreach (var osoba in listaOsob)
        {
            if (osoba is Lekarz l) Lekarze.Add(l);
        }
        return Lekarze;
    }

    /// <summary>
    /// Zwraca listę osób posortowaną według wieku.
    /// </summary>
    public List<Osoba> PobierzOsobyPosortowanePoWieku(bool rosnaco = true)
    {
        List<Osoba> kopiaListy = new List<Osoba>(listaOsob);
        kopiaListy.Sort(new WiekComparer());

        if (!rosnaco) kopiaListy.Reverse();
        return kopiaListy;
    }

    public List<Lekarz> ZnajdzLekarzySpecjalistow(EnumSpecjalizacja szukanaSpecjalizacja)
    {
        List<Lekarz> wyniki = new List<Lekarz>();
        foreach (var osoba in listaOsob)
        {
            if (osoba is Lekarz lekarz && lekarz.Specjalizacja == szukanaSpecjalizacja)
            {
                wyniki.Add(lekarz);
            }
        }
        return wyniki;
    }

    public double ObliczSredniWiekPacjentow()
    {
        var pacjenci = PobierzWszystkichPacjentow();
        if (pacjenci.Count == 0) return 0;

        double sumaLat = 0;
        foreach (var p in pacjenci) sumaLat += p.Wiek;

        return sumaLat / pacjenci.Count;
    }

    public Osoba? ZnajdzPoPeselu(string pesel)
    {
        foreach (var osoba in listaOsob)
        {
            if (osoba.Pesel == pesel) return osoba;
        }
        return null;
    }
}