using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QAWebsite.Controllers;
using QAWebsite.Data;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionViewModels;
using QAWebsite.Models.UserModels;
using QAWebsite.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QAWebsite.Tests
{
    class AdministratorTest
    {
        private const string Name = "TESTEMAIL@EMAILSERVICE.COM";
        private const string UserNameIdentifier = Name;
        private const string Password = "!Qaz2wsx";

        private ApplicationDbContext _context;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private UserManager<ApplicationUser> _userManager;
        private ManageController _manageController;
        private AccountController _accountController;
        private UrlEncoder _urlEncoder;
        private AchievementDistributor _achievementDistributor;
        private TagController _tagController;
        private QuestionController _questionController;
        private FlagsController _flagController;

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

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserNameIdentifier),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, Roles.ADMINISTRATOR.ToString())
            };

            defaultHttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = defaultHttpContext });
            var serviceProvider = services.BuildServiceProvider();

            var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            httpContext.RequestServices = serviceProvider;

            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var mLogger = serviceProvider.GetRequiredService<ILogger<ManageController>>();
            var aLogger = serviceProvider.GetRequiredService<ILogger<AccountController>>();
            _urlEncoder = serviceProvider.GetRequiredService<UrlEncoder>();
            _achievementDistributor = new AchievementDistributor();

            Mock<IEmailSender> emailSender = new Mock<IEmailSender>();
            emailSender.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost");
            urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("http://localhost");

            _manageController = new ManageController(_userManager, signInManager, emailSender.Object, mLogger, _urlEncoder)
            {
                Url = urlHelper.Object,
                ControllerContext = { HttpContext = httpContext }
            };

            _accountController = new AccountController(_userManager, signInManager, roleManager, emailSender.Object, aLogger)
            {
                Url = urlHelper.Object,
                ControllerContext = { HttpContext = httpContext }
            };

            var appUser = new ApplicationUser { Id = UserNameIdentifier, UserName = UserNameIdentifier, Email = UserNameIdentifier, EmailConfirmed = true };
            await _userManager.CreateAsync(appUser, Password);
            await _userManager.AddClaimsAsync(appUser, claims);
            await roleManager.CreateAsync(new ApplicationRole { Name = Roles.ADMINISTRATOR.ToString() });
            await _userManager.AddToRoleAsync(appUser, Roles.ADMINISTRATOR.ToString());

            _tagController = new TagController(_context)
            {
                ControllerContext = new ControllerContext() { HttpContext = httpContext }
            };

            _questionController = new QuestionController(_context, _userManager, _achievementDistributor, _dbContextOptions)
            {
                ControllerContext = new ControllerContext() { HttpContext = httpContext }
            };

            _flagController = new FlagsController(_context, _userManager)
            {
                ControllerContext = new ControllerContext() { HttpContext = httpContext }
            };
        }

        [Test]
        public async Task AdministratorQuestionEditing()
        {
            // Arrange
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
            question.AuthorId = "newIdentifier";
            _context.SaveChanges();

            // Act
            await _questionController.Edit(question.Id, new EditViewModel { Title = "NEW TITLE", Id = question.Id, Content = "NEW CONTENT" });

            var postEditQuestion = await _context.Question.SingleOrDefaultAsync(x => x.Id == question.Id);

            // Assert
            Assert.IsNotNull(postEditQuestion);
        }

        [Test]
        public async Task AdministratorQuestionDeleting()
        {
            // Arrange
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
           
            // Act
            _context.Question.Remove(question);


            _context.SaveChanges();

            var postRemoveQuestion = await _context.Question.SingleOrDefaultAsync(x =>x.Id == question.Id);

            // Assert
            Assert.IsNotNull(question);
            Assert.IsNull(postRemoveQuestion);
        }

        [Test]
        public async Task AdministratorTagDeleting()
        {
            // Arrange
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
            question.AuthorId = "newIdentifier";
            _context.SaveChanges();
            ModelStateDictionary modelState = new ModelStateDictionary();
            var tagsList = _tagController.ValidateParseTags("Tag1,Tag2", modelState);
            await _tagController.UpdateQuestionTags(question, tagsList);
            var questionTags = question.QuestionTags;

            Assert.IsTrue(questionTags.Count>0);
            // Act
            while (questionTags.Count > 0)
            {
                await _tagController.DeleteConfirmed(questionTags.First().TagId);
            }
           
            _context.SaveChanges();

            // Assert
            Assert.IsTrue(!_context.Tag.Any(tag => tagsList.Contains(tag.Name)));
        }

        [Test]
        public async Task AdministratorTagEditing()
        {
            // Arrange
            var vm = new CreateViewModel() { Title = "Test Title", Content = "Test content", Tags = "test, another tag" };
            await _questionController.Create(vm);
            var question = await _context.Question.SingleOrDefaultAsync(x => x.Title == "Test Title" && x.Content == "Test content");
            question.AuthorId = "newIdentifier";
            _context.SaveChanges();
            ModelStateDictionary modelState = new ModelStateDictionary();
            await _tagController.UpdateQuestionTags(question, _tagController.ValidateParseTags("Tag1,Tag2", modelState));
            var questionTags = question.QuestionTags.Select(questionTag => questionTag.Tag.Name).ToList();

            Assert.IsTrue(questionTags.Any());

            // Act
            await _tagController.UpdateQuestionTags(question, _tagController.ValidateParseTags("ByeOldTag,HiNewTag", modelState));

            var postEditQuestionTags = _context.Question.SingleOrDefaultAsync(x => x.Id == question.Id).Result.QuestionTags.Select(qt => qt.Tag.Name);

            // Assert
            Assert.IsTrue(postEditQuestionTags.Count() == 2 && postEditQuestionTags.Intersect(questionTags).Count() == 0);
        }
    }
}
