using Microsoft.AspNetCore.Mvc;

namespace QAWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "SB2 Team for SOEN 341.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("404")]
        public IActionResult Error()
        {
            return View();
        }
    }
}