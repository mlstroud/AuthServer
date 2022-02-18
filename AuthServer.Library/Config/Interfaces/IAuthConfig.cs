using System;

namespace AuthServer.Library.Config
{
    public interface IAuthConfig
    {
        string Secret { get; set; }
        string TokenDuration { get; set; }
        DateTime GetTokenExpiration();
    }
}
