using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QAWebsite.Controllers;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.QuestionViewModels;
using QAWebsite.Models.UserModels;
using QAWebsite.Services;

namespace QAWebsite.Tests
{
    [TestFixture]
    public class RatingTest
    {
        private const string UserNameIdentifier = "testId";
        private const string UserName = "testName";

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private AchievementDistributor _achievementDistributor;
        private RatingController _ratingController;
        private QuestionController _questionController;

        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserNameIdentifier),
                new Claim(ClaimTypes.Name, UserName)
            }));

            var context = new DefaultHttpContext() { User = user };
            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            context.User = user;
            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = context });
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _achievementDistributor = new AchievementDistributor();

            _ratingController = new RatingController(_context, _userManager)
            {
                ControllerContext = new ControllerContext() { HttpContext = context }
            };
            _questionController = new QuestionController(_context, _userManager, _achievementDistributor)
            {
                ControllerContext = new ControllerContext() { HttpContext = context }
            };
        }


        [Test]
        public async Task UpvoteQuestion()
        {
            // Arrange

            // Act
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
            await _ratingController.RateQuestion(question.Id, Models.Enums.Ratings.Upvote);
            var rating = await _context.QuestionRating.SingleOrDefaultAsync(x => x.FkId == question.Id && x.RatedBy == UserNameIdentifier);
            // Assert

            Assert.IsNotNull(question);
            Assert.IsNotNull(rating);
            Assert.AreEqual(rating.RatingValue, Models.Enums.Ratings.Upvote);

        }
        [Test]
        public async Task DownvoteQuestion()
        {
            // Arrange

            // Act
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
            await _ratingController.RateQuestion(question.Id, Models.Enums.Ratings.Downvote);
            var rating = await _context.QuestionRating.SingleOrDefaultAsync(x => x.FkId == question.Id && x.RatedBy == UserNameIdentifier);
            // Assert

            Assert.IsNotNull(question);
            Assert.IsNotNull(rating);
            Assert.AreEqual(rating.RatingValue, Models.Enums.Ratings.Downvote);

        }
    }
}