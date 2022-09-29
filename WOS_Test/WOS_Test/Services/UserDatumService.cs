using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WOS_Test.Dtos;
using WOS_Test.Models;

namespace WOS_Test.Services
{
    public class UserDatumService
    {
        private readonly WOSContext _wosContext;

        public UserDatumService(WOSContext wosContext)
        {
            _wosContext = wosContext;
        }

        public List<UserDatumDto> GetData(string? name, string? pw)
        {

            var result = _wosContext.UserData.Select(a => new UserDatum
            {
                Username = a.Username,
                Password = a.Password
            });

            if (!string.IsNullOrWhiteSpace(name))
            {
                result = result.Where(a => a.Username.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(pw))
            {
                result = result.Where(a => a.Password.Contains(pw));
            }


            return result.ToList().Select(a => DatumToDto(a)).ToList();
        }

        public IEnumerable<UserDatum> GetData_SQL()
        {
            var result = _wosContext.UserData.FromSqlRaw("Select * From Userdata");

            return result;
        }

        public ActionResult<UserDatumDto> GetID(int id)
        {
            var result = (from a in _wosContext.UserData
                        where a.UserId == id
                        select a).SingleOrDefault();

            return DatumToDto(result);
        }








        private static UserDatumDto DatumToDto(UserDatum item)
        {
            return new UserDatumDto
            {
                Username = item.Username,
                Password = item.Password
            };
        }
    }
}
