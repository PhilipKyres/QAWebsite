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
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QAWebsite.Tests
{
    [TestFixture]
    public class QuestionTest
    {
        private const string UserNameIdentifier = "testId";
        private const string UserName = "testName";

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private QuestionController _questionController;
        private FlagsController _flagController;
        private TagController _tagController;

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

            _tagController = new TagController(_context)
            {
                ControllerContext = new ControllerContext() { HttpContext = context }
            };
            _questionController = new QuestionController(_context, _userManager)
            {
                ControllerContext = new ControllerContext() {HttpContext = context}
            };
            _flagController = new FlagsController(_context, _userManager)
            {
                ControllerContext = new ControllerContext() { HttpContext = context }
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
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
            Assert.IsNotNull(question);
            Assert.AreEqual(question.AuthorId, UserNameIdentifier);
            Assert.AreEqual(question.Content, vm.Content);
            Assert.AreEqual(question.QuestionTags.Count, 2);
        }

        [Test]
        public async Task EditQuestionTag()
        {
            // Arrange

            // Act
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "InitialTag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");

            ModelStateDictionary modelState = new ModelStateDictionary();
            var tagsList = _tagController.ValidateParseTags("ChangedTag,NewTag", modelState);
            await _tagController.UpdateQuestionTags(question, tagsList);
            
            var questionAltered = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");

            var alteredTagList = questionAltered.QuestionTags.Select(a => a.Tag.Name);

            // Assert
            Assert.IsNotNull(questionAltered);
            Assert.AreEqual(questionAltered.Id, question.Id);
            Assert.IsTrue(tagsList.All(alteredTagList.Contains) && alteredTagList.All(tagsList.Contains));

        }

        [Test]
        public async Task ReportQuestion()
        {
            // Arrange

            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "InitialTag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");

            // Act
            await _flagController.Create(new FlagViewModel(question) {SelectedReason = Models.Enums.FlagType.Inappropriate, Content = "Test Content Report" });
            var flag = _context.Flag.Where(f => f.QuestionId == question.Id && f.Content == "Test Content Report" 
            && f.Reason == (int)Models.Enums.FlagType.Inappropriate).FirstOrDefault();
           
            // Assert
            Assert.IsNotNull(flag);
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
