using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;

namespace QAWebsite.Controllers
{
    [Produces("application/json")]
    [Route("api/Rating")]
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Ratings/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RateQuestion(string id, Ratings rateValue)
        {
            var userId = _userManager.GetUserId(User);
            var rating = _context.QuestionRating.Where(q => q.QuestionId == id && userId == q.RatedBy).FirstOrDefault();

            if (rating != null)
            {
                if ((int)rateValue != rating.RatingValue)
                {
                    rating.RatingValue = (int)rateValue;
                }
                else
                {
                    rating.RatingValue = (int)Ratings.Neutral;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                {
                    var userRating = new QuestionRating
                    {
                        QuestionId = id,
                        RatedBy = userId,
                        RatingValue = (int)rateValue
                    };
                    _context.Add(userRating);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Details","Question",new { id });
        }
    }
}