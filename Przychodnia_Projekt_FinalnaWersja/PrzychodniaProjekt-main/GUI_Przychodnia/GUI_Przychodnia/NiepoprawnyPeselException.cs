namespace SystemObslugiPrzychodni;

public class NiepoprawnyPeselException : Exception
{
    public NiepoprawnyPeselException(string message) : base(message) { }
}