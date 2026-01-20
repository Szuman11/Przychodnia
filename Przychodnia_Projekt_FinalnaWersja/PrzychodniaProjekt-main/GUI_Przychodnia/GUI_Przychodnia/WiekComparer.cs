using System;
using System.Collections.Generic;

namespace SystemObslugiPrzychodni;

/// <summary>
/// Klasa pomocnicza implementująca interfejs IComparer.
/// Służy do sortowania obiektów Osoba według daty urodzenia (wieku).
/// </summary>
public class WiekComparer : IComparer<Osoba>
{
    public int Compare(Osoba? x, Osoba? y)
    {
        if (x == null || y == null) return 0;
        return DateTime.Compare(x.DataUrodzenia, y.DataUrodzenia);
    }
}