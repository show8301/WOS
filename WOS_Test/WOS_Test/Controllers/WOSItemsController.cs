using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WOS_Test.Dtos;
using WOS_Test.Models;
using WOS_Test.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WOS_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOSItemsController : ControllerBase
    {
        private readonly WOSContext _wosContext;
        private readonly UserDatumService _userDatumService;

        public WOSItemsController(
            WOSContext wosContext,
            UserDatumService userDatumService)
        {
            _wosContext = wosContext;
            _userDatumService = userDatumService;
        }

        // GET: api/<WOSController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? name, string? pw)
        {
            var result = await _userDatumService.GetData(name, pw);

            if(result == null || result.Count() <= 0)
            {
                return NotFound("查無此資料");
            }

            return Ok(result);
        }

        [HttpGet("GetSQL")]
        public async Task<IEnumerable<UserDatum>> GetSQL()
        {
            var result = await _userDatumService.GetData_SQL(); 

            return result;
        }

        // GET api/<WOSController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDatumDto>> Get(int id)
        {
            var result = await _userDatumService.GetID(id);

            if(result == null)
            {
                return NotFound("查無UserID:" + id + "的資料");
            }
            return result;
        }

        // POST api/<WOSController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDatumPostDto value)
        {
            var insert = await _userDatumService.PostNewUser(value);

            _wosContext.UserData.Add(insert);
            _wosContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new {id = insert.UserId}, insert);
        }

        // PUT api/<WOSController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDatumPutDto value)
        {
            if(id != value.UserId)
            {
                return BadRequest();
            }

            if(await _userDatumService.PutData(id, value) == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument value)
        {
            if(await _userDatumService.PatchData(id, value) == 1)
            {
                return Ok("更新成功");
            }
            else
            {
                return StatusCode(500, "更新失敗");
            }
        }

        // DELETE api/<WOSController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _userDatumService.DeleteData(id) == 0)
            {
                return NotFound("找不到欲刪除的資料");
            }
            return Ok("刪除成功");
        }

        // DELETE api/<WOSController>/list/5
        [HttpDelete("list/{ids}")]
        public async Task<IActionResult> ListDelete(string ids)
        {
            if (await _userDatumService.DeleteListData(ids) == 0)
            {
                return NotFound("找不到欲刪除的資料");
            }
            return Ok("刪除成功");
        }
    }
}
