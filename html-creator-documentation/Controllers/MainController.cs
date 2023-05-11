using Microsoft.AspNetCore.Mvc;

namespace html_creator_documentation.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        [HttpGet("/download")]
        public FileResult GetLibraryFile()
        {
            return File("Files/html-creator-library.dll", "application/octet-stream", "html-creator-library.dll");
        }
    }
}
