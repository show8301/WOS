﻿using System.ComponentModel.DataAnnotations;

namespace WOS_Test.Dtos
{
    public class UserDatumPutDto
    {
        public int UserId { get; set; } = 0;
        public string Username { get; set; } = null!;
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{6,20}$", ErrorMessage ="開頭必須為英文字母，除底線以外不接受其他特殊符號")]
        public string Password { get; set; } = null!;
    }
}