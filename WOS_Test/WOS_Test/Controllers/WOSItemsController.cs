using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WOS_Test.Dtos;
using WOS_Test.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WOS_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOSItemsController : ControllerBase
    {
        private readonly WOSContext _wosContext;

        public WOSItemsController(WOSContext wosContext)
        {
            _wosContext = wosContext;
        }

        // GET: api/<WOSController>
        [HttpGet]
        public IActionResult Get([FromQuery] string? name, string? pw)
        {
            var result = _wosContext.UserData.Select(a=>new UserDatum
            {
                Username = a.Username,
                Password = a.Password
            });


            if (!string.IsNullOrWhiteSpace(name))
            {
                result = result.Where(a => a.Username.Contains(name));
            }
            if(!string.IsNullOrWhiteSpace(pw))
            {
                result = result.Where(a => a.Password.Contains(pw));
            }


            if(result == null || result.Count() <= 0)
            {
                return NotFound("查無此資料");
            }
            return Ok(result.ToList().Select(a => DatumToDto(a)));
            
        }

        [HttpGet("GetSQL")]
        public IEnumerable<UserDatum> GetSQL()
        {
            var result = _wosContext.UserData.FromSqlRaw("Select * From Userdata");

            return result;
        }

        // GET api/<WOSController>/5
        [HttpGet("{id}")]
        public ActionResult<UserDatumDto> Get(int id)
        {
            var result = (from a in _wosContext.UserData
                          where a.UserId == id
                          select a).SingleOrDefault();

            if(result == null)
            {
                return NotFound("查無UserID:" + id + "的資料");
            }
            return DatumToDto(result);
        }

        // POST api/<WOSController>
        [HttpPost]
        public IActionResult Post([FromBody] UserDatumPostDto value)
        {
            int id;

            // 因為若body內沒給UserId，預設是0
            if (value.UserId == 0)
            {
                var result = (from a in _wosContext.UserData
                              orderby a.UserId
                              select a.UserId).ToList();
                id = result[result.Count()-1] + 1;
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

            _wosContext.UserData.Add(insert);
            _wosContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new {id = insert.UserId}, insert);
        }

        // PUT api/<WOSController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDatumPutDto value)
        {
            if(id != value.UserId)
            {
                return BadRequest();
            }

            _wosContext.Entry(value).State = EntityState.Modified;

            try
            {
                _wosContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if(!_wosContext.UserData.Any(e=>e.UserId == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "存取發生錯誤");
                }
            }
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument value)
        {
            var patch = (from a in _wosContext.UserData
                         where id == a.UserId
                         select a).SingleOrDefault();

            if(patch != null)
            {
                value.ApplyTo(patch);
            }

            _wosContext.SaveChanges();

            return Ok("更新成功");
        }

        // DELETE api/<WOSController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var delete = _wosContext.UserData.Find(id);
            if(delete == null)
            {
                return NotFound();
            }
            _wosContext.UserData.Remove(delete);
            _wosContext.SaveChanges();
            return NoContent();
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
