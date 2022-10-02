using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WOS_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        
        private readonly IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public void Post(List<IFormFile> files, [FromQuery] int? id)
        {
            string root = _env.ContentRootPath + @"\wwwroot\" + id + "\\";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            foreach(var file in files)
            {
                string fileName = file.FileName;

                using (var stream = System.IO.File.Create(root + fileName))
                {
                    file.CopyTo(stream);
                }
            }
        }
    }
}
