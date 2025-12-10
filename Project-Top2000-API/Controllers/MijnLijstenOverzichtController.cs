using Microsoft.AspNetCore.Mvc;

namespace TemplateJwtProject.Controllers
{
    public class MijnLijstenOverzichtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
