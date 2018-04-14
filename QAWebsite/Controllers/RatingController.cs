using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.UserModels;

namespace QAWebsite.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public RatingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _context = context;
            _userManager = userManager;
            _dbContextOptions = dbContextOptions;
        }

        public int GetRating<T>(string id) where T : Rating
        {
            using (var contextInstance = new ApplicationDbContext(_dbContextOptions))
            {
                var dbSet = contextInstance.Set<T>();

                int up = dbSet.Count(qr => qr.FkId == id && qr.RatingValue == Ratings.Upvote);
                int down = dbSet.Count(qr => qr.FkId == id && qr.RatingValue == Ratings.Downvote);

                return up - down;
            }
        }

        private async Task Rate<T>(DbSet<T> dbSet, Rating newRating) where T : Rating
        {
            var userId = _userManager.GetUserId(User);
            var dbRating = dbSet.SingleOrDefault(q => q.FkId == newRating.FkId && userId == q.RatedBy);

            if (dbRating == null)
            {
                newRating.RatedBy = userId;
                _context.Add(newRating);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (newRating.RatingValue == dbRating.RatingValue)
                {
                    dbSet.Remove(dbRating);
                }
                else
                {
                    dbRating.RatingValue = newRating.RatingValue;
                }
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RateQuestion(string questionId, Ratings rateValue)
        {
            if (questionId == null || _context.Question.FirstOrDefault(e => e.Id == questionId) == null)
            {
                return NotFound();
            }

            await Rate(_context.QuestionRating, new QuestionRating()
            {
                FkId = questionId,
                RatingValue = rateValue
            });

            return RedirectToAction("Details", "Question", new { id = questionId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RateAnswer(string answerId, Ratings rateValue)
        {
            Answer answer;
            if (answerId == null || (answer = _context.Answer.FirstOrDefault(e => e.Id == answerId)) == null)
            {
                return NotFound();
            }

            await Rate(_context.AnswerRating, new AnswerRating()
            {
                FkId = answerId,
                RatingValue = rateValue
            });

            return RedirectToAction("Details", "Question", new { id = answer.QuestionId });
        }
    }
}