using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models.UserModels;
using QAWebsite.Services;

namespace QAWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAchievementDistributor _achievementDistributor;
        private readonly RatingController _ratingController;
        private readonly QuestionController _questionController;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAchievementDistributor achievementDistributor, DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _context = context;
            _userManager = userManager;
            _achievementDistributor = achievementDistributor;
            _ratingController = new RatingController(context, userManager, dbContextOptions);
            _questionController = new QuestionController(context, userManager, achievementDistributor, dbContextOptions);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_questionController.GetQuestionList().Take(10));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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