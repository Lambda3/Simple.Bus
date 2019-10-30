namespace Simple.Bus.Core.Cryptographers
{
    public class CryptographyDefault : ICryptography
    {
        public string Decrypt(string message)
        {
            return message;
        }

        public string Encrypt(string message)
        {
            return message;
        }
    }
}
