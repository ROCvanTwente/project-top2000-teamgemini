using Microsoft.AspNetCore.Mvc;

namespace TemplateJwtProject.Controllers
{
    public class JaarOverzichtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
