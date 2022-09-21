using System;
using System.Collections.Generic;

namespace WOS_Test.Models
{
    public partial class UserDatum
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
