﻿using AuthServer.Library.Enums;

namespace AuthServer.Library.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public UserType UserType { get; set; }
    }
}
