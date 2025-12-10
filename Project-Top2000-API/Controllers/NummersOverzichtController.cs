using Microsoft.AspNetCore.Mvc;

namespace TemplateJwtProject.Controllers
{
    public class NummersOverzichtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
