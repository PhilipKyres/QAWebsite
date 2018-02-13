using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.FlagViewModels;

namespace QAWebsite.Controllers
{
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
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var flagModel = new FlagViewModel(question)
            {
                SelectedReason = "2"
            };
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
                    Id = Guid.NewGuid().ToString().Substring(0, 8),
                    Reason = fm.SelectedReason,
                    Content = fm.Content,
                    CreationDate = DateTime.Now,
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
            return View(await _context.Flag.ToListAsync());
        }

        // GET: Flags/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flag = await _context.Flag
                .SingleOrDefaultAsync(m => m.Id == id);
            if (flag == null)
            {
                return NotFound();
            }

            return View(flag);
        }        
 
        // GET: Flags/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flag = await _context.Flag
                .SingleOrDefaultAsync(m => m.Id == id);
            if (flag == null)
            {
                return NotFound();
            }

            return View(flag);
        }

        // POST: Flags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var flag = await _context.Flag.SingleOrDefaultAsync(m => m.Id == id);
            _context.Flag.Remove(flag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlagExists(string id)
        {
            return _context.Flag.Any(e => e.Id == id);
        }
    }
    #endregion
}
