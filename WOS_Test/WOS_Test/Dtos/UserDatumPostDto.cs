using System.ComponentModel.DataAnnotations;
using WOS_Test.ValidationAttributes;

namespace WOS_Test.Dtos
{
    public class UserDatumPostDto
    {
        public int UserId { get; set; } = 0;

        [Username]
        public string Username { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{6,20}$", ErrorMessage ="開頭必須為英文字母，除底線以外不接受其他特殊符號")]
        public string Password { get; set; } = null!;
    }
}
