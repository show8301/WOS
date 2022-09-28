using System.ComponentModel.DataAnnotations;
using WOS_Test.Dtos;
using WOS_Test.Models;

namespace WOS_Test.ValidationAttributes
{
    public class UsernameAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            WOSContext _wosContext = (WOSContext)validationContext.GetService(typeof(WOSContext));

            var name = (string)value;

            var findName = from a in _wosContext.UserData
                            where a.Username == name
                            select a;


            // ----如果是做PUT，就要排除掉相同Username的驗證-----------------------
            
            var dto = validationContext.ObjectInstance;
            
            if (dto.GetType() == typeof(UserDatumPutDto))
            {
                var dtoUpdate = (UserDatumPutDto)dto;
                findName = findName.Where(a => a.UserId != dtoUpdate.UserId);
            }
            
            // ------------------------------------------------------------------


            if(findName.FirstOrDefault() != null)
            {
                return new ValidationResult("已存在該使用者名稱");
            }

            return ValidationResult.Success;
        }
    }
}
