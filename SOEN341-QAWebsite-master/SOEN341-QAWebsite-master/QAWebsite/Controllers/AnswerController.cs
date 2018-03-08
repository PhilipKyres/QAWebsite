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
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.QuestionViewModels;

namespace QAWebsite.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Answer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Answer.ToListAsync());  
        }

        // GET: Answer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsViewModel dvm)
        {
            if (!ModelState.IsValid)
            {
                var newDvm = await new QuestionController(_context, _userManager).GetDetailsViewModel(dvm.Id);
                newDvm.AnswerContent = dvm.AnswerContent;
                return View("~/Views/Question/Details.cshtml", newDvm);
            }

            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(),
                Content = dvm.AnswerContent,
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                QuestionId = dvm.Id,
                AuthorId = _userManager.GetUserId(User)
            };

            _context.Add(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Question", new { dvm.Id });
        }

        // GET: Answer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Answer am)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var answer = await _context.Answer.SingleOrDefaultAsync(a => a.Id == id);
                if (answer == null || answer.AuthorId != _userManager.GetUserId(User))
                {
                    return NotFound();
                }

                answer.Content = am.Content;
                answer.EditDate = DateTime.Now;

                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(am.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                id = answer.QuestionId;
                return RedirectToAction("Details", "Question", new { id } ); ;
            }
            return View(am);
        }

        // GET: Answer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var answer = await _context.Answer.SingleOrDefaultAsync(m => m.Id == id);
            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(string id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }

        public List<AnswerViewModel> GetAnswerList(string id)
        {
            var answer = _context.Answer.Where(a => a.QuestionId == id).OrderBy(o => o.CreationDate).ToList();

            List<AnswerViewModel> avm = new List<AnswerViewModel>();

            answer.ForEach(a =>
            {
                string name = _context.Users.Where(u => u.Id == a.AuthorId).Select(x => x.UserName).SingleOrDefault();
                avm.Add(new AnswerViewModel(a, name));
            });

            return avm;
        }
    }
}
