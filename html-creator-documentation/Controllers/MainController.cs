using Microsoft.AspNetCore.Mvc;

namespace html_creator_documentation.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
