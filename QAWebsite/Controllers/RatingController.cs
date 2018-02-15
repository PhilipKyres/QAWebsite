using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public int GetRating(string questionId)
        {
            int up = _context.QuestionRating.Count(qr => qr.QuestionId == questionId && qr.RatingValue == (int)Ratings.Upvote);
            int down = _context.QuestionRating.Count(qr => qr.QuestionId == questionId && qr.RatingValue == (int)Ratings.Downvote);

            return up - down;
        }

        // GET: api/Ratings/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RateQuestion(string questionId, Ratings rateValue)
        {
            if (questionId == null || !_context.Question.Any(e => e.Id == questionId))
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var rating = _context.QuestionRating.SingleOrDefault(q => q.QuestionId == questionId && userId == q.RatedBy);

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
                var userRating = new QuestionRating
                {
                    QuestionId = questionId,
                    RatedBy = userId,
                    RatingValue = (int)rateValue
                };
                _context.Add(userRating);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Question", new { id = questionId });
        }
    }
}