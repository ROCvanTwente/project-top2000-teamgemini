using Microsoft.AspNetCore.Mvc;

namespace TemplateJwtProject.Controllers
{
    public class InlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
