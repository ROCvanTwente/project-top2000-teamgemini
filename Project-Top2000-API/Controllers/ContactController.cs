using Microsoft.AspNetCore.Mvc;

namespace TemplateJwtProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
