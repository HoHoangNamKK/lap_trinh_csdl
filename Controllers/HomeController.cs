using Microsoft.AspNetCore.Mvc;

namespace demo2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
