using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.QuestionViewModels;

namespace QAWebsite.Controllers
{
    [Authorize]
    public class FlagsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlagsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region User Functionality
        // GET: Flags/Create
        public IActionResult Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flagModel = new FlagViewModel(id);

            return View(flagModel);
        }

        // POST: Flags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlagViewModel fm)
        {
            if (ModelState.IsValid)
            {
                var flag = new Flag
                {
                    Id = Guid.NewGuid().ToString(),
                    Reason = (int)fm.SelectedReason,
                    Content = fm.Content,
                    CreationDate = DateTime.Now,
                    QuestionId = fm.QuestionId,
                    AuthorId = _userManager.GetUserId(User)
                };

                _context.Add(flag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Question", flag.Id);
            }
            return View(fm);
        }
        #endregion

        #region Admin Functionality
        // GET: Flags
        public async Task<IActionResult> Index()
        {
            if (!this.IsAdmin())
                return NotFound();

            var flags = await _context.Flag.ToListAsync();

            return View(this.GetFlagViewModel(flags));
        }

        public async Task<IActionResult> FlagsByQuestion(string id)
        {
            if (!this.IsAdmin() || id == null)
                return NotFound();
                
            var flags = await _context.Flag.Where(f => f.QuestionId == id).ToListAsync();           

            return View("Index", this.GetFlagViewModel(flags));
        }

        private List<FlagViewModel> GetFlagViewModel(List<Flag> flags)
        {
            List<FlagViewModel> flagList = new List<FlagViewModel>();

            flags.ForEach(f =>
            {
                flagList.Add(new FlagViewModel(f, f.QuestionId, _context.Users.Where(u => u.Id == f.AuthorId).Select(x => x.UserName).SingleOrDefault()));
            });

            return flagList;
        }

        // GET: Flags/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if (id == null || !this.IsAdmin())
            {
                return NotFound();
            }

            var flag = await _context.Flag
                .SingleOrDefaultAsync(m => m.Id == id);

            if (flag == null)
            {
                return NotFound();
            }

            var fvm = new FlagViewModel(flag, flag.QuestionId, _context.Users.Where(u => u.Id == flag.AuthorId).Select(x => x.UserName).SingleOrDefault());

            return View(fvm);
        }        
 
        // GET: Flags/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || !this.IsAdmin())
            {
                return NotFound();
            }

            var flag = await _context.Flag
                .SingleOrDefaultAsync(m => m.Id == id);
            if (flag == null)
            {
                return NotFound();
            }
            var fvm = new FlagViewModel(flag, flag.QuestionId, _context.Users.Where(u => u.Id == flag.AuthorId).Select(x => x.UserName).SingleOrDefault());

            return View(fvm);
        }

        // POST: Flags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null || !this.IsAdmin())
            {
                return NotFound();
            }

            var flag = await _context.Flag.SingleOrDefaultAsync(m => m.Id == id);
            _context.Flag.Remove(flag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool FlagExists(string id)
        {
            return _context.Flag.Any(e => e.Id == id);
        }
        private bool IsAdmin()
        {          
           return _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result, Roles.ADMINISTRATOR.ToString()).Result;
        }
    }
    #endregion
}
