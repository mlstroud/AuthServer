namespace AuthServer.Library.Services
{
    public interface ICryptographyService
    {
        string GetSalt();
        string GetHash(string password, string salt);
    }
}
