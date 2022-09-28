using System.ComponentModel.DataAnnotations;
using WOS_Test.Dtos;
using WOS_Test.Models;

namespace WOS_Test.ValidationAttributes
{
    public class PasswordAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pw = (string)value;
            string name="";

            if(validationContext.ObjectInstance.GetType() == typeof(UserDatumPostDto))
            {
                var user = (UserDatumPostDto)validationContext.ObjectInstance;
                name = user.Username;
            }
            else if(validationContext.ObjectInstance.GetType() == typeof(UserDatumPutDto))
            {
                var user = (UserDatumPutDto)validationContext.ObjectInstance;
                name = user.Username;
            }
 
            if (name == pw)
            {
                return new ValidationResult("密碼不得與使用者名稱相同");
            }

            return ValidationResult.Success;
        }
    }
}
