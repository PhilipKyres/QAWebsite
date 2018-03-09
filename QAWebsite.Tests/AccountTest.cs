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
using QAWebsite.Models.AccountViewModels;

namespace QAWebsite.Tests
{
    [TestFixture]
    public class AccountTest
    {
        private const string UserNameIdentifier = "testId";
        private const string UserName = "testName";

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private AccountController _accountController;

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

            _accountController = new AccountController(_context, _userManager)
            {
                ControllerContext = new ControllerContext() { HttpContext = context }
            };
        }

        [Test]
        public void AccountCreation()
        {
            // Arrange

            // Act

            // Assert
        }
        
    }
}
