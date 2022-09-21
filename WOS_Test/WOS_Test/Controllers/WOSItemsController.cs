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
        public IEnumerable<UserDatumDto> Get([FromQuery] string? name, string? pw)
        {
            var result = _wosContext.UserData.Select(a=>new UserDatumDto
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
            return result;
            
        }

        // GET api/<WOSController>/5
        [HttpGet("{id}")]
        public ActionResult<UserDatum> Get(int id)
        {
            var result = _wosContext.UserData.Find(id);
            if(result == null)
            {
                return NotFound("找不到");
            }
            return result;
        }

        // POST api/<WOSController>
        [HttpPost]
        public ActionResult<UserDatum> Post([FromBody] UserDatum value)
        {
            _wosContext.UserData.Add(value);
            _wosContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new {id = value.UserId}, value);
        }

        // PUT api/<WOSController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDatum value)
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
    }
}
