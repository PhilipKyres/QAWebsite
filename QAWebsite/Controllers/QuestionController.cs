using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.QuestionViewModels;
using QAWebsite.Models.MultipleViewModels;

namespace QAWebsite.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Question
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.ToListAsync();

            QuestionTagsViewModel model = new QuestionTagsViewModel();
            model.QuestionModel = await _context.Question.ToListAsync();
            model.TagsModel = await _context.Tags.ToListAsync();

            return View(model);
        }

        // GET: Question/Details/5
        [AllowAnonymous]
        [Route("/Question/{id:length(8)}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuestionTagsViewModel model = new QuestionTagsViewModel();
            model.QuestionModel = await _context.Question.ToListAsync();
            model.TagsModel = await _context.Tags.ToListAsync();
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Question/Create
        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var question = new Question
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8),
                    Title = vm.Title,
                    Content = vm.Content,
                    CreationDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    AuthorId = _userManager.GetUserId(User)
                };

                if (vm.Tags.Contains(','))
                {
                    String[] tagsList = vm.Tags.Split(',');

                    for (int i = 0; i < tagsList.Length; i++)
                    {
                        var tags = new TaggingViewModel
                        {
                            Id = Guid.NewGuid().ToString().Substring(0, 8),
                            Tags = tagsList[i],
                            QuestionId = question.Id
                        };
                        _context.Add(tags);
                    }
                }

                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            var tags = await _context.Tags.FirstOrDefaultAsync(m => m.QuestionId == id);

            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(new EditViewModel(question, tags));
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
                var tags = await _context.Tags.SingleOrDefaultAsync(m => m.QuestionId == id);
                if (question == null || tags == null || question.AuthorId != _userManager.GetUserId(User))
                {
                    return NotFound();
                }

                question.Title = vm.Title;
                question.Content = vm.Content;
                tags.Tags = vm.Tags;
                question.EditDate = DateTime.Now;

                try
                {
                    _context.Update(question);
                    _context.Update(tags);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuestionTagsViewModel model = new QuestionTagsViewModel();
            model.QuestionModel = await _context.Question.ToListAsync();
            model.TagsModel = await _context.Tags.ToListAsync();

            if (model == null || model.QuestionModel.FirstOrDefault().AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(string id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
