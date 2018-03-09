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

        [AllowAnonymous]
        [Route("/Profile/{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var profileViewModel = new ProfileViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Upvotes = 0, //TODO Fetch and calculate from upvotes of questions/answers
                    Downvotes = 0, //TODO Fetch and calculate from upvotes of questions/answers
                    AboutMe = user.AboutMe ?? Resources.aboutMeNullString,
                    QuestionList = await _context.Question.Where(q => q.AuthorId == id).OrderByDescending(q => q.CreationDate).Take(5).ToListAsync()
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
