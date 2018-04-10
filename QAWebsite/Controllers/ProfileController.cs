using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models.AccountViewModels;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.UserModels;
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

        public static int GetRatingCount<T>(DbSet<T> dbSet, IList idList, Ratings ratingType) where T : Rating
        {
            return dbSet.Count(item => idList.Contains(item.FkId) && item.RatingValue == ratingType);
        }

        [AllowAnonymous]
        [Route("/Profile/{name}")]
        public async Task<IActionResult> Profile(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }

            var questionIdList = await _context.Question.Where(q => q.AuthorId == user.Id).Select(question => question.Id).ToListAsync();
            var answerIdList = await _context.Answer.Where(q => q.AuthorId == user.Id).Select(answer => answer.Id).ToListAsync();

            int rating = GetRatingCount(_context.QuestionRating, questionIdList, Ratings.Upvote) -
                         GetRatingCount(_context.QuestionRating, questionIdList, Ratings.Downvote) +
                         GetRatingCount(_context.AnswerRating, answerIdList, Ratings.Upvote) -
                         GetRatingCount(_context.AnswerRating, answerIdList, Ratings.Downvote);

            var profileViewModel = new ProfileViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                Rating = rating,
                AboutMe = user.AboutMe ?? Resources.aboutMeNullString,
                QuestionList = await _context.Question.Where(q => q.AuthorId == user.Id).OrderByDescending(q => q.CreationDate).Take(5).ToListAsync(),
                AnswerList = await _context.Answer.Where(q => q.AuthorId == user.Id).OrderByDescending(q => q.CreationDate).Take(5).ToListAsync()
            };

            if (user.UserImage != null)
            {
                profileViewModel.UserImage = "data:image/gif;base64," + Convert.ToBase64String(user.UserImage);
            }
/*            else using (var memStream = new MemoryStream())
                {
                    Resources.defaultUserImage.Save(memStream, Resources.defaultUserImage.RawFormat);
                    img = memStream.ToArray();
                }*/


            return View("Profile", profileViewModel);

        }

        [AllowAnonymous]
        [Route("/Achievements/{name}")]
        public async Task<IActionResult> Achievements(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }

            var achievementPairs = _context.UserAchievements.Where(achievement => achievement.UserId == user.Id).Select(ua =>
                new AchievementDisplayContainer
                {
                    Image = "data:image/gif;base64," + Convert.ToBase64String(ua.Achievement.AchievementImage),
                    Title = ua.Achievement.Title,
                    Description = ua.Achievement.Description,
                }).ToList();
                
            var viewModel = new AchievementViewModel
            {
                Username = user.UserName,
                Achievements = achievementPairs
            };

            return View(viewModel);
        }
    }
}
