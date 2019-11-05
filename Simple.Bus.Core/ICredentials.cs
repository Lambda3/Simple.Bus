namespace Simple.Bus.Core
{
    public interface ICredentials<T> where T : class
    {
        T Get();
    }
}
