using System.ComponentModel.DataAnnotations;
using WOS_Test.ValidationAttributes;

namespace WOS_Test.Dtos
{
    public class LoginPostDto
    { 
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
