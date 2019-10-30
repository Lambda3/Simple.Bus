namespace Simple.Bus.Core.Cryptographers
{
    public interface ICryptography
    {
        string Encrypt(string message);
        string Decrypt(string message);
    }
}
