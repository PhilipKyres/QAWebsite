using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;

namespace QAWebsite.Controllers
{
    [Produces("application/json")]
    [Route("api/Profile")]
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

        [AllowAnonymous]
        [Route("/profile/{id}")]
        public async Task<IActionResult> Profile(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            var profileViewModel = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                UserImage = user.UserImage,
                Upvotes = user.Upvotes,
                Downvotes = user.Downvotes,
                AboutMe = user.AboutMe
            };

            return View("Profile", profileViewModel);

        }
    }
}