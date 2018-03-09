using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.QuestionViewModels;

namespace QAWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RatingController _ratingController;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _ratingController = new RatingController(context, userManager);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var questions = await _context.Question
                   .Include(x => x.QuestionTags)
                   .ThenInclude(x => x.Tag)
                   .OrderByDescending(q => q.CreationDate)
                   .Take(10)
                   .ToListAsync();

            IEnumerable<IndexViewModel> vms = questions.Select(q => new IndexViewModel(q,
                _context.Users.Where(u => u.Id == q.AuthorId).Select(x => x.UserName).SingleOrDefault(),
                _ratingController.GetRating(q.Id)));

            return View(vms);
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
