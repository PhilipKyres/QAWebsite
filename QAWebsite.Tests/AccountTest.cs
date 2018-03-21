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
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using QAWebsite.Models.ManageViewModels;
using QAWebsite.Services;
using System.IO;
using QAWebsite.Models.AccountViewModels;
using Microsoft.AspNetCore.Http.Internal;

namespace QAWebsite.Tests
{
    [TestFixture]
    public class AccountTest
    {
        private const string Name = "TESTEMAIL@EMAILSERVICE.COM";
        private const string UserNameIdentifier = Name;
        private const string Password = "!Qaz2wsx";

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private ManageController _manageController;
        private AccountController _accountController;
        private UrlEncoder _urlEncoder;

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
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var mLogger = serviceProvider.GetRequiredService<ILogger<ManageController>>();
            var aLogger = serviceProvider.GetRequiredService<ILogger<AccountController>>();
            _urlEncoder = serviceProvider.GetRequiredService<UrlEncoder>();

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

            await _userManager.CreateAsync(new ApplicationUser { Id = UserNameIdentifier, UserName = UserNameIdentifier, Email = UserNameIdentifier, EmailConfirmed = true }, Password);
        }


        [Test]
        public async Task AccountInfoEdit()
        {
            // Arrange

            // Act
            var vm = new IndexViewModel()
            {
                AboutMe = "new about me message",
                Email = UserNameIdentifier
            };

            await _manageController.Index(vm);

            // Assert
            var user = await _userManager.GetUserAsync(_manageController.User);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.AboutMe, vm.AboutMe);
        }

        [Test]
        public async Task ImageUpload()
        {
            // Arrange
            string strPhoto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../QAWebsite/wwwroot/images", "shiba.jpg");
            FileStream fs = new FileStream(strPhoto, FileMode.Open, FileAccess.Read);

            byte[] photoByte;
            photoByte = File.ReadAllBytes(strPhoto);

            // Act
            var vmRegister = new RegisterViewModel { Username = "testUser", Email = "test@test.com", Password = "Abcd1234!", ConfirmPassword = "Abcd1234!", UserImage = new FormFile(fs, 0, 357223, "shiba", "shiba.jpg") };
            await _accountController.Register(vmRegister); //Upload image through register

            var vmProfile = new IndexViewModel { Username = Name, Email = Name, UserImage = new FormFile(fs, 0, 357223, "shiba", "shiba.jpg") };
            await _manageController.Index(vmProfile); //Upload image through profile

            // Assert
            var register = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == "testUser");
            Assert.IsNotNull(register);
            Assert.IsNotNull(register.UserImage);
            Assert.AreEqual(photoByte, register.UserImage);

            var profile = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == Name);
            Assert.IsNotNull(profile);
            Assert.IsNotNull(profile.UserImage);
            Assert.AreEqual(photoByte, profile.UserImage);
        }
    }
}