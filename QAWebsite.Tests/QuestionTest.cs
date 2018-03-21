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
using QAWebsite.Services;

namespace QAWebsite.Tests
{
    [TestFixture]
    public class QuestionTest
    {
        private const string UserNameIdentifier = "testId";
        private const string UserName = "testName";

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private AchievementDistributor _AchievementDistributor;
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
            _AchievementDistributor = new AchievementDistributor();


            _questionController = new QuestionController(_context, _userManager, _AchievementDistributor)
            {
                ControllerContext = new ControllerContext() {HttpContext = context}
            };
        }

        [Test]
        public async Task CreateQuestion()
        {
            // Arrange

            // Act
            var vm = new CreateViewModel() {Title = "Test Title", Content = "Test content", Tags = "test, another tag"};
            await _questionController.Create(vm);

            // Assert
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title");
            Assert.IsNotNull(question);
            Assert.AreEqual(question.AuthorId, UserNameIdentifier);
            Assert.AreEqual(question.Content, vm.Content);
            Assert.AreEqual(question.QuestionTags.Count, 2);
        }

        [Test]
        public void EditQuestion()
        {
            // Arrange

            // Act

            // Assert
        }

        [Test]
        public void VoteQuestion()
        {
            // Arrange

            // Act

            // Assert
        }

        [Test]
        public void BestAnswer()
        {
            // Arrange

            // Act

            // Assert
        }

        [Test]
        public void DeleteQuestion()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
