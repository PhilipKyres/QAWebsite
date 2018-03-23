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
using QAWebsite.Models.QuestionViewModels;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QAWebsite.Models.UserModels;
using QAWebsite.Services;
namespace QAWebsite.Tests
{
    [TestFixture]
    public class QuestionTest
    {
        private const string Name = "TESTEMAIL@EMAILSERVICE.COM";
        private const string UserNameIdentifier = Name;
        private const string Password = "!Qaz2wsx";

        private ApplicationDbContext _context;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private UserManager<ApplicationUser> _userManager;
        private AchievementDistributor _achievementDistributor;
        private QuestionController _questionController;
        private FlagsController _flagController;
        private TagController _tagController;
        private AnswerController _answerController;

        [SetUp]
        public async Task SetUp()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            var defaultHttpContext = new DefaultHttpContext();

            defaultHttpContext.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());

            defaultHttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserNameIdentifier),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, "ADMINISTRATOR")
            }));

            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = defaultHttpContext });
            var serviceProvider = services.BuildServiceProvider();

            var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            httpContext.RequestServices = serviceProvider;

            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _achievementDistributor = new AchievementDistributor();

            _tagController = new TagController(_context)
            {
                ControllerContext = new ControllerContext() { HttpContext = httpContext }
            };

            _questionController = new QuestionController(_context, _userManager, _achievementDistributor, _dbContextOptions)
            {
                ControllerContext = new ControllerContext() {HttpContext = httpContext }
            };

            _flagController = new FlagsController(_context, _userManager)
            {
                ControllerContext = new ControllerContext() { HttpContext = httpContext }
            };

            _answerController = new AnswerController(_context, _userManager, _achievementDistributor, _dbContextOptions)
            {
                ControllerContext = new ControllerContext() { HttpContext = httpContext }
            };

            await _userManager.CreateAsync(new ApplicationUser { Id = UserNameIdentifier, UserName = UserNameIdentifier, Email = UserNameIdentifier, EmailConfirmed = true }, Password);
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
            await _flagController.Create(new FlagViewModel(question.Id) {SelectedReason = Models.Enums.FlagType.Inappropriate, Content = "Test Content Report" });
            var flag = _context.Flag.Where(f => f.QuestionId == question.Id && f.Content == "Test Content Report" 
            && f.Reason == (int)Models.Enums.FlagType.Inappropriate).FirstOrDefault();
           
            // Assert
            Assert.IsNotNull(flag);
        }

        [Test]
        public async Task EditQuestion()
        {
            // Arrange

            // Act
            var vmCreate = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vmCreate);

            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title");
            var vmEdit = new EditViewModel(question) { Title = "New Title", Content = "Modified Content" };
            await _questionController.Edit(question.Id, vmEdit);

            var edit = await _questionController.EditHistory(question.Id) as ViewResult;
            var model = edit.Model as QuestionEditsListViewModel;

            // Assert
            var questionEdit = await _context.QuestionEdits.SingleOrDefaultAsync(x => x.NewTitle == "New Title");
            Assert.IsNotNull(questionEdit);
            Assert.AreNotEqual(vmCreate.Content, vmEdit.Content);
            Assert.AreNotEqual(questionEdit.Id, question.Id);
            Assert.AreEqual(questionEdit.NewTitle, question.Title);
            Assert.AreEqual(questionEdit.NewContent, question.Content);
            Assert.AreEqual(questionEdit.NewTitle, vmEdit.Title);
            Assert.AreEqual(questionEdit.NewContent, vmEdit.Content);
            Assert.AreEqual(model.Edits.Count, 1); // Check if history is updated
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

        [Test]
        public async Task SearchQuestion()
        {
            // Arrange

            // Act
            var match1 = new CreateViewModel() { Title = "How to Hello World?", Content = "Can someone show me how to Hello World in Java?", Tags = "Hello World, Java, Programming" };
            var match2 = new CreateViewModel() { Title = "How do you write a Python script?", Content = "Show me how to to automate some tests using Python.", Tags = "testing, Python, Programming" };
            var unique1 = new CreateViewModel() { Title = "Definition of RESTAPI", Content = "What exactly is RESTAPI?", Tags = "RESTAPI, api" };

            await _questionController.Create(match1);
            await _questionController.Create(match2);
            await _questionController.Create(unique1);

            // Search by Tag
            var resultTag1 = _questionController.Search("Programming") as ViewResult;
            var vmsTag1 = resultTag1.Model as IEnumerable<IndexViewModel>;
            var resultTag2 = _questionController.Search("testing") as ViewResult;
            var vmsTag2 = resultTag2.Model as IEnumerable<IndexViewModel>;

            // Search by Title
            var resultTitle1 = _questionController.Search("Definition of RESTAPI") as ViewResult;
            var vmsTitle1 = resultTitle1.Model as IEnumerable<IndexViewModel>;
            var resultTitle2 = _questionController.Search("How to Hello World?") as ViewResult;
            var vmsTitle2 = resultTitle1.Model as IEnumerable<IndexViewModel>;

            // Search by Content
            var resultContent1 = _questionController.Search("What exactly is RESTAPI?") as ViewResult;
            var vmsContent1 = resultContent1.Model as IEnumerable<IndexViewModel>;
            var resultContent2 = _questionController.Search("Show me how to to automate some tests using Python.") as ViewResult;
            var vmsContent2 = resultContent2.Model as IEnumerable<IndexViewModel>;

            // Search by Username
            //var resultUser = await _questionController.Search("testName") as ViewResult;
            //var vmsUser = resultUser.Model as IEnumerable<IndexViewModel>;

            // Assert
            Assert.AreEqual(vmsTag1.Count(), 2); // Number of questions in db with multiple matching Tags
            Assert.AreEqual(vmsTag2.Count(), 1); // Number of questions in db with unique matching Tag

            Assert.AreEqual(vmsTitle2.Count(), 2); // Number of questions in db with multiple matching strings in Title ("How")
            Assert.AreEqual(vmsTitle1.Count(), 1); // Number of questions in db with unique matching Title

            Assert.AreEqual(vmsContent2.Count(), 2); // Number of questions in db with multiple matching strings in Title ("Show me how to")
            Assert.AreEqual(vmsContent1.Count(), 1); // Number of questions in db with unique matching title

            //Assert.AreEqual(vmsUser.Count(), 0); // Number of questions in db with unique matching title
        }

        [Test]
        public async Task DeleteAnswer()
        {
            // Arrange

            // Act
            var vmQuestion = new CreateViewModel() { Title = "How to Hello World?", Content = "Can someone show me how to Hello World in Java?", Tags = "Hello World, Java, Programming" };
            await _questionController.Create(vmQuestion);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "How to Hello World?");

            var vmAnswer = new DetailsViewModel() {Id = question.Id, AnswerContent = "System.out.println(\"Hello World\");" };
            await _answerController.Create(vmAnswer);
            var answer = await _context.Answer.SingleOrDefaultAsync(x => x.QuestionId == question.Id);
            await _answerController.DeleteConfirmed(answer.Id);

            // Assert
            var deleted = await _context.Answer.SingleOrDefaultAsync(x => x.Id == answer.Id);
            Assert.IsNull(deleted);
        }
    }
}
