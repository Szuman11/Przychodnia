using System;
using System.Collections.Generic;

namespace SystemObslugiPrzychodni;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        KlinikaManager klinika = new KlinikaManager();
        Console.WriteLine("TESTOWANIE SYSTEMU PRZYCHODNI\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1. Testowanie Walidacji i Wyjątków");
        Console.ResetColor();
        try
        {
            Console.Write("Test błędnego PWZ: ");
            Lekarz zlyLekarz = new Lekarz("Jan", "Błąd", "80051203454", EnumSpecjalizacja.Internista, "123");
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Złapano błąd PWZ: {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Niewłaściwy wyjątek: {ex.Message}");
            Console.ResetColor();
        }

        try
        {
            Console.Write("Test błędnego PESEL: ");
            Pacjent zlyPesel = new Pacjent("Ewa", "Test", "44051401358");
        }
        catch (NiepoprawnyPeselException ex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Błąd PESEL: {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Niewłaściwy wyjątek: {ex.Message}");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("2. Dodawanie Poprawnych Osób");
        Console.ResetColor();

        try
        {
            Lekarz l1 = new Lekarz("Grzegorz", "House", "80051203454", EnumSpecjalizacja.Internista, "1234567");
            Lekarz l2 = new Lekarz("Anna", "Kliniczna", "85010112345", EnumSpecjalizacja.Kardiolog, "7654321");
            Pacjent p1 = new Pacjent("Jan", "Kowalski", "90031502345");
            p1.DodajChorobe("Grypa");
            Pacjent p2 = new Pacjent("Zofia", "Młoda", "05251501230");
            p2.DodajChorobe("Nadciśnienie");
            p2.DodajChorobe("Cukrzyca");

            klinika.DodajOsobe(l1);
            klinika.DodajOsobe(l2);
            klinika.DodajOsobe(p1);
            klinika.DodajOsobe(p2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Dodano");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Nie udało się dodać osób: {ex.Message}");
            Console.WriteLine("Dalsze testy mogą nie działać poprawnie.");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("3. Testy Logiki Biznesowej");
        Console.ResetColor();

        Console.WriteLine("Lista posortowana od najstarszego:");
        var posortowani = klinika.PobierzOsobyPosortowanePoWieku(rosnaco: false);
        if (posortowani.Count > 0)
        {
            foreach (var o in posortowani)
            {
                Console.WriteLine($"{o.Imie} {o.Nazwisko}, Wiek: {o.Wiek} lat ({o.DataUrodzenia:yyyy-MM-dd})");
            }
        }
        else
        {
            Console.WriteLine("Brak osób");
        }

        Console.WriteLine("\nRaport Specjalistów:");
        var kardiolodzy = klinika.ZnajdzLekarzySpecjalistow(EnumSpecjalizacja.Kardiolog);
        if (kardiolodzy.Count > 0)
        {
            foreach (var k in kardiolodzy)
            {
                Console.WriteLine($"   - {k.PobierzDane()}");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono kardiologów");
        }

        double sredniWiek = klinika.ObliczSredniWiekPacjentow();
        Console.WriteLine($"\nŚredni wiek pacjentów: {sredniWiek:F1} lat");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("4. Zaawansowane Interfejsy");
        Console.ResetColor();

        Console.WriteLine("Test IEquatable:");
        Pacjent pKlonPesel = new Pacjent("Inny", "Ktoś", "90031502345");

        Osoba znalezionyJan = klinika.ZnajdzPoPeselu("90031502345");

        if (znalezionyJan != null && pKlonPesel.Equals(znalezionyJan))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Znaleziono osobę z tym samym numerem PESEL.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Nie znaleziono osoby z tym samym numerem PESEL. ");
            Console.ResetColor();
        }

        Console.WriteLine("\nTest ICloneable:");
        if (znalezionyJan is Pacjent oryginal)
        {
            Pacjent klon = (Pacjent)oryginal.Clone();
            klon.Imie = "SKLONOWANY_JAN";
            klon.DodajChorobe("Złamana noga");
            Console.WriteLine($"   Oryginał: {oryginal.Imie}, Choroby: {string.Join(", ", oryginal.HistoriaChorob)}");
            Console.WriteLine($"   Klon:     {klon.Imie}, Choroby: {string.Join(", ", klon.HistoriaChorob)}");

            if (!oryginal.HistoriaChorob.Contains("Złamana noga") && klon.HistoriaChorob.Contains("Złamana noga"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Lista chorób jest niezależna.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Zmiana w klonie zmieniła oryginał.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Pominięto test klonowania");
        }
        Console.ReadKey();
    }
}