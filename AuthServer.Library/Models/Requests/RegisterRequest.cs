namespace AuthServer.Library.Models.Requests
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValid() =>
            (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                ? false
                : true;
    }
}
