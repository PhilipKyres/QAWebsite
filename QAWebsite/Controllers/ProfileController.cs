using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.AccountViewModels;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Properties;

namespace QAWebsite.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context,
          UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public static int GetRatingCount<T>(DbSet<T> dbSet,IList idList, Ratings ratingType) where T : Rating
        {
            return dbSet.Count(item => idList.Contains(item.FkId) && item.RatingValue == ratingType);
        }

        [AllowAnonymous]
        [Route("/Profile/{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            { 
                var QuestionIdList = await _context.Question.Where(q => q.AuthorId == id).Select(question => question.Id).ToListAsync();
                var AnswerIdList = await _context.Answer.Where(q => q.AuthorId == id).Select(answer => answer.Id).ToListAsync();

                var profileViewModel = new ProfileViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    QuestionUpvotes = GetRatingCount(_context.QuestionRating, QuestionIdList, Ratings.Upvote),
                    QuestionDownvotes = GetRatingCount(_context.QuestionRating, QuestionIdList, Ratings.Downvote),
                    AnswerUpvotes = GetRatingCount(_context.AnswerRating, AnswerIdList, Ratings.Upvote),
                    AnswerDownvotes = GetRatingCount(_context.AnswerRating, AnswerIdList, Ratings.Downvote),
                    AboutMe = user.AboutMe ?? Resources.aboutMeNullString,
                    QuestionList = await _context.Question.Where(q => q.AuthorId == id).OrderByDescending(q => q.CreationDate).Take(5).ToListAsync(),
                    AnswerList = await _context.Answer.Where(q => q.AuthorId == id).OrderByDescending(q => q.CreationDate).Take(5).ToListAsync()
                };

                if (user.UserImage != null)
                {
                    profileViewModel.UserImage = user.UserImage;
                }
                else using (var memStream = new MemoryStream())
                {
                    Resources.defaultUserImage.Save(memStream, Resources.defaultUserImage.RawFormat);
                    profileViewModel.UserImage = memStream.ToArray();
                }
                return View("Profile", profileViewModel);
            }
            
            return NotFound();
        }
    }
}
