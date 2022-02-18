using System;

namespace AuthServer.Library.Config
{
    public class AuthConfig : IAuthConfig
    {
        private const int DefaultDuration = 30;
        public string Secret { get; set; }
        public string TokenDuration { get; set; }

        public DateTime GetTokenExpiration()
        {
            try
            {
                int.TryParse(TokenDuration, out int result);

                return DateTime.UtcNow.AddSeconds(result);
            }
            catch (Exception e)
            {
                return DateTime.UtcNow.AddSeconds(DefaultDuration);
            }
        }
    }
}
