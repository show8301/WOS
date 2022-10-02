using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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

        public async Task<List<UserDatumDto>> GetData(string? name, string? pw)
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

            var temp = await result.ToListAsync();

            return temp.Select(a => DatumToDto(a)).ToList();
        }

        public async Task<IEnumerable<UserDatum>> GetData_SQL()
        {
            var temp = _wosContext.UserData.FromSqlRaw("Select * From Userdata");

            var result = await temp.ToListAsync();

            return result;
        }

        public async Task<ActionResult<UserDatumDto>> GetID(int id)
        {
            var result = await (from a in _wosContext.UserData
                        where a.UserId == id
                        select a).SingleOrDefaultAsync();

            return DatumToDto(result);
        }

        public async Task<UserDatum> PostNewUser(UserDatumPostDto value)
        {
            int id = 0 ;

            // 因為若body內沒給UserId，預設是0
            if (value.UserId == 0)
            {
                var result = await (from a in _wosContext.UserData
                              orderby a.UserId
                              select a.UserId).ToListAsync();

                for(int i = 0; i < result.Count; i++)
                {
                    if(i != result[i])
                    {
                        id = i;
                        break;
                    }
                    if(i == result.Count - 1)
                    {
                        id = result[i] + 1;
                    }
                }
            }
            else
            {
                id = value.UserId;
            }

            UserDatum insert = new UserDatum
            {
                UserId = id,
                Username = value.Username,
                Password = value.Password
            };

            return insert;
        }

        public async Task<int> PutData(int id, UserDatumPutDto value)
        {
            var put = await _wosContext.UserData.FindAsync(id);

            if (put != null)
            {
                put.Username = value.Username;
                put.Password = value.Password;
            }

            return await _wosContext.SaveChangesAsync();
        }

        public async Task<int> PatchData(int id, JsonPatchDocument value)
        {
            var patch = await (from a in _wosContext.UserData
                         where id == a.UserId
                         select a).SingleOrDefaultAsync();

            if (patch != null)
            {
                value.ApplyTo(patch);
            }

            return await _wosContext.SaveChangesAsync();
        }

        public async Task<int> DeleteData(int id)
        {
            var delete = await _wosContext.UserData.FindAsync(id);

            if (delete != null)
            {
                _wosContext.UserData.Remove(delete);
            }

            return await _wosContext.SaveChangesAsync();
        }

        public async Task<int> DeleteListData(string ids)
        {
            List<int> deleteList = JsonSerializer.Deserialize<List<int>>(ids);

            var delete = (from a in _wosContext.UserData
                          where deleteList.Contains(a.UserId)
                          select a);
            if (delete != null)
            {
                _wosContext.UserData.RemoveRange(delete);
            }            

            return await _wosContext.SaveChangesAsync();
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
