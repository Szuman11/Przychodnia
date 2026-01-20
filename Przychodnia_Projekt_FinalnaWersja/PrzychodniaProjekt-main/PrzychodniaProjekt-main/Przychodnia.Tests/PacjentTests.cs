using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemObslugiPrzychodni;
using System;

namespace Przychodnia.Tests;

[TestClass]
public class LekarzTests
{
    const string PoprawnyPeselLekarza = "80051203454";
    const string PoprawnyPWZ = "1234567";

    [TestMethod]
    public void Konstruktor_PoprawneDane_TworzyLekarza()
    {
        string imie = "Grzegorz";
        string nazwisko = "House";

        Lekarz l = new Lekarz(imie, nazwisko, PoprawnyPeselLekarza, EnumSpecjalizacja.Kardiolog, PoprawnyPWZ);

        Assert.AreEqual(imie, l.Imie);
        Assert.AreEqual(EnumSpecjalizacja.Kardiolog, l.Specjalizacja);
        Assert.AreEqual(PoprawnyPWZ, l.NumerPWZ);
    }

    [TestMethod]
    public void NumerPWZ_ZaKrotkiPWZ_Exception()
    {
        const string krotkiPWZ = "123";

        Assert.ThrowsException<ArgumentException>(() => new Lekarz("Test", "Testowy", PoprawnyPeselLekarza, EnumSpecjalizacja.Internista, krotkiPWZ));
    }

    [TestMethod]
    public void NumerPWZ_ZadlugiPWZ_Exception()
    {
        const string dlugiPWZ = "12345678";

        Assert.ThrowsException<ArgumentException>(() => new Lekarz("Test", "Testowy", PoprawnyPeselLekarza, EnumSpecjalizacja.Internista, dlugiPWZ));
    }

    [TestMethod]
    public void NumerPWZ_ZlyFormatPWZ_Exception()
    {
        const string zlyformatPWZ = "PWZ4567";

        Assert.ThrowsException<ArgumentException>(() => new Lekarz("Test", "Testowy", PoprawnyPeselLekarza, EnumSpecjalizacja.Internista, zlyformatPWZ));
    }

    [TestMethod]
    public void PobierzDane_ZawieraSpecjalizacje()
    {
        var l = new Lekarz("Anna", "Bąk", PoprawnyPeselLekarza, EnumSpecjalizacja.Okulista, PoprawnyPWZ);

        string opis = l.PobierzDane();

        StringAssert.Contains(opis, "Lekarz");
        StringAssert.Contains(opis, "Okulista");
        StringAssert.Contains(opis, PoprawnyPWZ);
    }
}